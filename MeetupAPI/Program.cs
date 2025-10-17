using System.Text;
using Authorization;
using Entities;
using ErrorHandling;
using Filters;
using FluentValidation;
using FluentValidation.AspNetCore;
using Health;
using HealthChecks.UI.Client;
using Identity;
using Interceptors;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Options;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddMediator();
builder.Services.AddScoped<DomainEventsInterceptor>();

var jwtOptions = builder.Configuration.GetSection("jwt").Get<JwtOptions>() ?? new JwtOptions();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("jwt"));
builder.Services.Configure<DbOptions>(builder.Configuration.GetSection("ConnectionStrings"));

builder.Logging.AddOpenTelemetry(x =>
{
    x.IncludeScopes = true;
    x.IncludeFormattedMessage = true;
});

builder.Services.AddOpenTelemetry()
    .WithMetrics(x =>
    {
        x.AddRuntimeInstrumentation()
            .AddMeter(
                "Microsoft.AspNetCore.Hosting",
                "Microsoft.AspNetCore.Server.Kestrel",
                "System.Net.Http",
                "MeetupAPI");
    })
    .WithTracing(x =>
    {
        if (builder.Environment.IsDevelopment()) x.SetSampler<AlwaysOnSampler>();

        x.AddAspNetCoreInstrumentation()
            .AddGrpcClientInstrumentation()
            .AddHttpClientInstrumentation();
    });

builder.Services.Configure<OpenTelemetryLoggerOptions>(logging => logging.AddOtlpExporter());
builder.Services.ConfigureOpenTelemetryMeterProvider(metrics => metrics.AddOtlpExporter());
builder.Services.ConfigureOpenTelemetryTracerProvider(tracing => tracing.AddOtlpExporter());

builder.Services.ConfigureHttpClientDefaults(http => { http.AddStandardResilienceHandler(); });

builder.Services.AddMetrics();
builder.Services.AddSingleton<IMeetupApiMetrics, MeetupApiMetrics>();

builder.Services.AddSingleton(jwtOptions);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = jwtOptions.JwtIssuer,
        ValidAudience = jwtOptions.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.JwtKey))
    };
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("HasNationality", policy => policy.RequireClaim("Nationality", "German", "English"))
    .AddPolicy("AtLeast18", policy => policy.AddRequirements(new MinimumAgeRequirement(18)));

builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("Database", tags: [ "ready", "startup"]);

//adding healthchecks UI
builder.Services.AddHealthChecksUI(opt =>
    {
        opt.SetEvaluationTimeInSeconds(15); //time in seconds between check
        opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks
        opt.SetApiMaxActiveRequests(1); //api requests concurrency

        opt.AddHealthCheckEndpoint("Health Check", "/healthz"); //map health check api
    })
    .AddInMemoryStorage();

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationRulesToSwagger();

builder.Services.AddScoped<TimeTrackFilter>();
builder.Services.AddScoped<IAuthorizationHandler, MeetupResourceOperationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeHandler>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddSingleton<DomainEventsInterceptor>();
// previously it was AddDbContext<MeetupContext>
// with the implementation that also has a scope:
// builder.Services.AddDbContext<MeetupContext>((sp, option) =>
// {
//     option.UseSqlServer(myDbConfig);
//     option.AddInterceptors(sp.GetRequiredService<DomainEventsInterceptor>());
// });
// but we changed it to use the factory:
builder.Services.AddDbContextFactory<MeetupContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("MeetupDb"));
});
builder.Services.AddScoped<MeetupSeeder>();

builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "MeetupAPI", Version = "v1" }); });

builder.Services.AddCors(options => { options.AddPolicy("FrontEndClient", 
    policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000")); });

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseResponseCaching();
app.UseStaticFiles();
app.UseCors("FrontEndClient");

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "MeetupAPI v1"); });

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

IdentityModelEventSource.ShowPII = true;

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse // Returns UI-compatible JSON format
});
app.MapHealthChecksUI();

app.MapHealthChecks("health/startup", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("startup")
});
app.MapHealthChecks("health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

//SeedDatabase();

app.Run();

void SeedDatabase()
{
    using var scope = app.Services.CreateScope();
    try
    {
        var meetupSeeder = scope.ServiceProvider.GetRequiredService<MeetupSeeder>();
        meetupSeeder.Seed();
    }
    catch
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError("An error occurred seeding the DB.");
        throw;
    }
}

public partial class Program;
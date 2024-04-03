using FluentValidation;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using MeetupAPI;
using MeetupAPI.Authorization;
using MeetupAPI.Entities;
using MeetupAPI.Filters;
using MeetupAPI.Health;
using MeetupAPI.Identity;
using MeetupAPI.Models;
using MeetupAPI.Options;
using MeetupAPI.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
var jwtOptions = new JwtOptions();
builder.Configuration.GetSection("jwt").Bind(jwtOptions);
builder.Services.Configure<DbOptions>(
    builder.Configuration.GetSection("ConnectionStrings"));
var myDbConfig = builder.Configuration.GetConnectionString("MeetupDb");

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
builder.Services.AddAuthorization(options =>
{
    // this policy will be met only by user with specified nationality
    options.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality", "German", "English"));
    options.AddPolicy("AtLeast18", builder => builder.AddRequirements(new MinimumAgeRequirement(18)));
});

builder.Services
    .AddHealthChecks()
    //failureStatus: HealthStatus.Unhealthy); // pre-set Health Check for MSSQL
    .AddCheck<DatabaseHealthCheck>("Database");

//adding healthchecks UI
builder.Services.AddHealthChecksUI(opt =>
{
    opt.SetEvaluationTimeInSeconds(15); //time in seconds between check
    opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks
    opt.SetApiMaxActiveRequests(1); //api requests concurrency

    opt.AddHealthCheckEndpoint("Health Check", "/healthz"); //map health check api
})
.AddInMemoryStorage();

builder.Services.AddScoped<TimeTrackFilter>();
builder.Services.AddScoped<IAuthorizationHandler, MeetupResourceOperationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeHandler>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserValidator>();
builder.Services.AddScoped<IValidator<UpdateUserDto>, UpdateUserValidator>();
builder.Services.AddScoped<IValidator<UserLoginDto>, UserLoginValidator>();
builder.Services.AddScoped<IValidator<MeetupQuery>, MeetupQueryValidator>();
builder.Services.AddDbContext<MeetupContext>(option => option.UseSqlServer(myDbConfig));
builder.Services.AddScoped<MeetupSeeder>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo() { Title = "MeetupAPI", Version = "v1" });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndClient", builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000"));
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseResponseCaching();
app.UseStaticFiles();
app.UseCors("FrontEndClient");
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MeetupAPI v1");
});

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

app.UseAuthentication();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    //adding endpoint of health check for the health check ui in UI format
    endpoints.MapHealthChecks("/healthz", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    //map healthcheck ui endpoing - default is /healthchecks-ui/
    endpoints.MapHealthChecksUI();
});

//SeedDatabase();

app.Run();


void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var meetupSeeder = scope.ServiceProvider.GetRequiredService<MeetupSeeder>();
            meetupSeeder.Seed();
        }
        catch
        {
            throw;
        }
    }
}

public partial class Program 
{
}
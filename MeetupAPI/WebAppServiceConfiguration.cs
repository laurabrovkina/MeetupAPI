using FluentValidation;
using FluentValidation.AspNetCore;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using System;
using System.Text;

namespace MeetupAPI;

public static class WebAppServiceConfiguration
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        var jwtOptions = new JwtOptions();
        builder.Configuration.GetSection("jwt").Bind(jwtOptions);
        builder.Services.Configure<DbOptions>(
            builder.Configuration.GetSection("ConnectionStrings"));
        var myDbConfig = builder.Configuration.GetConnectionString("MeetupDb");

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
                if (builder.Environment.IsDevelopment())
                {
                    x.SetSampler<AlwaysOnSampler>();
                }

                x.AddAspNetCoreInstrumentation()
                    .AddGrpcClientInstrumentation()
                    .AddHttpClientInstrumentation();
            });

        builder.Services.Configure<OpenTelemetryLoggerOptions>(logging => logging.AddOtlpExporter());
        builder.Services.ConfigureOpenTelemetryMeterProvider(metrics => metrics.AddOtlpExporter());
        builder.Services.ConfigureOpenTelemetryTracerProvider(tracing => tracing.AddOtlpExporter());

        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            http.AddStandardResilienceHandler();
        });

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

    }
}

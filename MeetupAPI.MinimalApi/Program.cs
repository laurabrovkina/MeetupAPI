using FastEndpoints;
using FastEndpoints.Swagger;
using MeetupAPI.MinimalApi;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Models;
using MinimalApi.Repositories;
using MinimalApi.Services;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDoc();

// .AddDbContext is scoped - which means that for each HTTP request, we have a single DB context and 
// this could be a problem because when we run multiple resolvers in parallel because the context that we share in this HTTP request
// is not thread-safe which leads to nasty exceptions happening from time to time
// .AddDbContextPool is doing the pooling without using the factory. We still have one single instance per HTTP request or per service scope
// So this instance is retrieved from the pool. We reuse the DB context and have a better memory footprint but we have the same usage as we had before
builder.Services.AddDbContextPool<MeetupContext>(option => option.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MeetupDb;Trusted_Connection=True;"));

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddSingleton<IAccountRepository, AccountRepository>();
builder.Services.AddSingleton<IAccountService, AccountService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndClient", builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000"));
});

var app = builder.Build();

app.UseFastEndpoints();
app.UseOpenApi();
app.UseSwaggerUi3(s => s.ConfigureDefaults());

app.Run();

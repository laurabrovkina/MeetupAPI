using HealthChecks.UI.Client;
using MeetupAPI;
using MeetupAPI.ErrorHandling;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.ConfigureServices();

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

app.UseMiddleware<ExceptionHandlingMiddleware>();
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
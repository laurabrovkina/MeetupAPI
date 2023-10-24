using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeetupAPI.Health;

public class DatabaseHealthCheck : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, 
        CancellationToken cancellationToken = new())
    {
        try
        {
            using var dbConnection = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=MeetupDb;Trusted_Connection=True;");
            dbConnection.Open();
            using var command = dbConnection.CreateCommand();
            command.CommandText = "SELECT 1";
            command.ExecuteScalar();
            dbConnection.Close();
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}

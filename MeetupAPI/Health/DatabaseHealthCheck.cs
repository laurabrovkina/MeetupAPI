using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeetupAPI.Health;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly DbOptions _options;

    public DatabaseHealthCheck(IOptions<DbOptions> options)
    {
        _options = options.Value;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, 
        CancellationToken cancellationToken = new())
    {
        try
        {
            using var dbConnection = new SqlConnection(_options.MeetupDb);
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

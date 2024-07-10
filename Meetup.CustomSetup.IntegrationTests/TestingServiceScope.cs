using MediatR;
using MeetupAPI.Entities;
using Microsoft.Extensions.DependencyInjection;
using static Meetup.CustomSetup.IntegrationTests.TestFixture;

namespace Meetup.CustomSetup.IntegrationTests;

public class TestingServiceScope
{
    private readonly IServiceScope _scope;

    public TestingServiceScope()
    {
        _scope = BaseScopeFactory.CreateScope();
    }

    public TScopedService GetService<TScopedService>()
    {
        var service = _scope.ServiceProvider.GetService<TScopedService>();
        return service;
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        var mediator = _scope.ServiceProvider.GetService<ISender>();
        return await mediator.Send(request);
    }

    public async Task<TEntity> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        var context = _scope.ServiceProvider.GetService<MeetupContext>();
        return await context.FindAsync<TEntity>(keyValues);
    }

    public async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        var context = _scope.ServiceProvider.GetService<MeetupContext>();
        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
    {
        var dbContext = _scope.ServiceProvider.GetRequiredService<MeetupContext>();
        var result = await action(_scope.ServiceProvider);
        return result;
    }

    public Task<T> ExecuteDbContextAsync<T>(Func<MeetupContext, Task<T>> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<MeetupContext>()));

    public Task<int> InsertAsync<T>(params T[] entities) where T : class
    {
        return ExecuteDbContextAsync(db =>
        {
            foreach (var entity in entities)
            {
                db.Set<T>().Add(entity);
            }
            return db.SaveChangesAsync();
        });
    }
}

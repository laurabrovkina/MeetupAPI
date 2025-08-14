using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Entities;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Interceptors;

public class DomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IMediator _mediator;

    public DomainEventsInterceptor(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        // Do nothing before save (events should be published AFTER commit)
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            await DispatchDomainEventAsync(eventData.Context);
        }
        
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchDomainEventAsync(DbContext context)
    {
        var entitiesWithEvents = context.ChangeTracker
            .Entries<Meetup>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();
        
        var domainEvents = entitiesWithEvents
            .SelectMany(e => e.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent);
        }
        
        entitiesWithEvents.ForEach(e => e.ClearDomainEvents());
    }
}
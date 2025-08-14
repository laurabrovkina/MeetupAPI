using System;
using System.Threading;
using System.Threading.Tasks;
using Entities;
using Mediator;

namespace Interceptors;

public class MeetupCreatedEventHandler : INotificationHandler<MeetupCreatedDomainEvent>
{
    public ValueTask Handle(MeetupCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[Handler] Meetup created: {notification.MeetupId}");
        return ValueTask.CompletedTask;
    }
}
using System;
using Mediator;

namespace Entities;

public class MeetupCreatedDomainEvent : INotification
{
    public Guid MeetupId { get; }
    public string UserId { get; }

    public MeetupCreatedDomainEvent(Guid meetupId, string userId)
    {
        MeetupId = meetupId;
        UserId = userId;
    }
}
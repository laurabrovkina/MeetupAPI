using Microsoft.AspNetCore.Authorization;

namespace Authorization;

public enum OperationType
{
    Create,
    Read,
    Update,
    Delete
}

public class ResourceOperationRequirement : IAuthorizationRequirement
{
    public ResourceOperationRequirement(OperationType operationType)
    {
        OperationType = operationType;
    }

    public OperationType OperationType { get; }
}
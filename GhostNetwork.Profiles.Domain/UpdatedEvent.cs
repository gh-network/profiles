using System;
using GhostNetwork.EventBus;

namespace GhostNetwork.Profiles
{
    public record UpdatedEvent(Guid Id, string FullName, string ProfilePicture) : TrackableEvent;
}

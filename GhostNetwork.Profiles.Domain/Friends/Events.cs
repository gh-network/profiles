using System;
using GhostNetwork.EventBus;

namespace GhostNetwork.Profiles.Friends;

public record RequestSent(Guid FromUser, Guid ToUser) : TrackableEvent;
public record RequestCancelled(Guid FromUser, Guid ToUser) : TrackableEvent;
public record RequestApproved(Guid User, Guid Requester) : TrackableEvent;
public record RequestDeclined(Guid User, Guid Requester) : TrackableEvent;
public record Deleted(Guid User, Guid Friend) : TrackableEvent;

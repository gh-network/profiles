using System;

namespace GhostNetwork.Profiles.Api.Models;

public record ProfileUpdateViewModel(
    string FirstName,
    string LastName,
    string Gender,
    DateTimeOffset? DateOfBirth,
    string City)
{
    [Obsolete("Use PUT /users/{id}/avatar instead")]
    public string ProfilePicture { get; set; }
}

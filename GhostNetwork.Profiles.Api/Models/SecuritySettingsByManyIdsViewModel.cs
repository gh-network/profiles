using System;
using System.Collections.Generic;

namespace GhostNetwork.Profiles.Api.Models;

public class SecuritySettingsByManyIdsViewModel
{
    public IEnumerable<Guid> UserIds { get; set; }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GhostNetwork.Profiles.Api.Models
{
    public class ProfilesQueryModel
    {
        [Required]
        public IEnumerable<Guid> Ids { get; set; }
    }
}
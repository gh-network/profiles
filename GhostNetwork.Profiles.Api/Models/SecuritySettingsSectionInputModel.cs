using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GhostNetwork.Profiles.SecuritySettings;

namespace GhostNetwork.Profiles.Api.Models
{
    public class SecuritySettingsSectionInputModel
    {
        [Required]
        public Access Access { get; set; }

        public IEnumerable<Guid> CertainUsers { get; set; }
    }
}
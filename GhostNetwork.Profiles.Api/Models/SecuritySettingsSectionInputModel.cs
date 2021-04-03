using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GhostNetwork.Profiles.SecuritySettings;

namespace GhostNetwork.Profiles.Api.Models
{
    public class SecuritySettingsSectionInputModel
    {
        [Required]
        [EnumDataType(typeof(Access), ErrorMessage = "Access not found.")]
        public Access Access { get; set; }

        public IEnumerable<Guid> CertainUsers { get; set; }
    }
}
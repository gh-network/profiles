using System.ComponentModel.DataAnnotations;

namespace GhostNetwork.Profiles.Api.Models
{
    public class SecuritySettingUpdateViewModel
    {
        [Required]
        public SecuritySettingsSectionInputModel Posts { get; set; }

        [Required]
        public SecuritySettingsSectionInputModel Friends { get;  set; }
    }
}

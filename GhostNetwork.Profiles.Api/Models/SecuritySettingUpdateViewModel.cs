using System.ComponentModel.DataAnnotations;

namespace GhostNetwork.Profiles.Api.Models
{
    public class SecuritySettingUpdateViewModel
    {
        [Required]
        public SecuritySettingsSectionInputModel Posts { get; set; }

        [Required]
        public SecuritySettingsSectionInputModel Friends { get;  set; }

        [Required]
        public SecuritySettingsSectionInputModel Comments { get; set; }

        [Required]
        public SecuritySettingsSectionInputModel Reactions { get; set; }

        [Required]
        public SecuritySettingsSectionInputModel Followers { get; set; }
    }
}

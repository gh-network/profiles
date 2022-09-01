namespace GhostNetwork.Profiles.Api.Models
{
    public class SecuritySettingUpdateViewModel
    {
        public SecuritySettingsSectionInputModel Posts { get; set; }

        public SecuritySettingsSectionInputModel Friends { get;  set; }

        public SecuritySettingsSectionInputModel Comments { get; set; }

        public SecuritySettingsSectionInputModel ProfilePhoto { get; set; }

        public SecuritySettingsSectionInputModel Followers { get; set; }
    }
}

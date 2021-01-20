using System;
using System.ComponentModel.DataAnnotations;
using GhostNetwork.Profiles.SecuritySettings;

namespace GhostNetwork.Profiles.Api.Models
{
    public class SecuritySettingUpdateViewModel
    {
        [Required]
        public Access AccessToPosts { get; set; }

        [Required]
        public Access AccessToFriends { get;  set; }

        public Guid[] CertainUsersForPosts { get; set; }

        public Guid[] CertainUsersForFriends { get; set; }
    }
}

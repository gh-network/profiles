using System;

namespace GhostNetwork.Profiles.Api.Models
{
    public class FriendRequestCreateViewModel
    {
        public Guid FromUser { get; set; }

        public string FromUserName { get; set; }

        public Guid ToUser { get; set; }

        public string ToUserName { get; set; }
    }
}
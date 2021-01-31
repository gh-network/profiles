using System;
using System.Linq;

namespace GhostNetwork.Profiles.SecuritySettings
{
    public class SecuritySetting
    {
        public SecuritySetting(
            Guid userId,
            SecuritySettingsSection posts,
            SecuritySettingsSection friends)
        {
            UserId = userId;
            Posts = posts;
            Friends = friends;
        }

        public Guid UserId { get; }

        public SecuritySettingsSection Posts { get; private set; }

        public SecuritySettingsSection Friends { get; private set; }

        public static SecuritySetting DefaultForUser(Guid userId)
        {
            return new SecuritySetting(
                userId,
                new SecuritySettingsSection(Access.Everyone, Enumerable.Empty<Guid>()),
                new SecuritySettingsSection(Access.Everyone, Enumerable.Empty<Guid>()));
        }

        public void Update(SecuritySettingsSection posts, SecuritySettingsSection friends)
        {
            Posts = posts;
            Friends = friends;
        }
    }
}

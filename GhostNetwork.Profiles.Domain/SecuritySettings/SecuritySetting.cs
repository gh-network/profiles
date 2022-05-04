using System;
using System.Linq;

namespace GhostNetwork.Profiles.SecuritySettings
{
    public class SecuritySetting
    {
        public SecuritySetting(
            Guid userId,
            SecuritySettingsSection posts,
            SecuritySettingsSection friends,
            SecuritySettingsSection comments,
            SecuritySettingsSection reactions,
            SecuritySettingsSection followers)
        {
            UserId = userId;
            Posts = posts;
            Friends = friends;
            Comments = comments;
            Reactions = reactions;
            Followers = followers;
        }

        public Guid UserId { get; }

        public SecuritySettingsSection Posts { get; private set; }

        public SecuritySettingsSection Friends { get; private set; }

        public SecuritySettingsSection Comments { get; private set; }

        public SecuritySettingsSection Reactions { get; private set; }

        public SecuritySettingsSection Followers { get; private set; }

        public static SecuritySetting DefaultForUser(Guid userId)
        {
            return new SecuritySetting(
                userId,
                new SecuritySettingsSection(Access.Everyone, Enumerable.Empty<Guid>()),
                new SecuritySettingsSection(Access.Everyone, Enumerable.Empty<Guid>()),
                new SecuritySettingsSection(Access.Everyone, Enumerable.Empty<Guid>()),
                new SecuritySettingsSection(Access.Everyone, Enumerable.Empty<Guid>()),
                new SecuritySettingsSection(Access.Everyone, Enumerable.Empty<Guid>()));
        }

        public void Update(
            SecuritySettingsSection posts,
            SecuritySettingsSection friends,
            SecuritySettingsSection comments,
            SecuritySettingsSection reactions,
            SecuritySettingsSection followers)
        {
            Posts = posts;
            Friends = friends;
            Comments = comments;
            Reactions = reactions;
            Followers = followers;
        }
    }
}

using System;
using System.Linq;

namespace GhostNetwork.Profiles.SecuritySettings
{
    public class SecuritySetting
    {
        public SecuritySetting(
            Guid userId,
            SecuritySettingsSection friends,
            SecuritySettingsSection followers,
            SecuritySettingsSection posts,
            SecuritySettingsSection comments,
            SecuritySettingsSection profilePhoto)
        {
            UserId = userId;
            Friends = friends;
            Followers = followers;
            Posts = posts;
            Comments = comments;
            ProfilePhoto = profilePhoto;
        }

        public Guid UserId { get; }

        public SecuritySettingsSection Friends { get; private set; }

        public SecuritySettingsSection Followers { get; private set; }

        public SecuritySettingsSection Posts { get; private set; }

        public SecuritySettingsSection Comments { get; private set; }

        public SecuritySettingsSection ProfilePhoto { get; private set; }

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
            SecuritySettingsSection friends,
            SecuritySettingsSection followers,
            SecuritySettingsSection posts,
            SecuritySettingsSection comments,
            SecuritySettingsSection profilePhoto)
        {
            Friends = friends;
            Followers = followers;
            Posts = posts;
            Comments = comments;
            ProfilePhoto = profilePhoto;
        }
    }
}

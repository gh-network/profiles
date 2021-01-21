using System;
using System.Collections.Generic;

namespace GhostNetwork.Profiles.SecuritySettings
{
    public static class DefaultSecuritySetting
    {
        private static readonly SecuritySetting SecuritySetting =
            new SecuritySetting(default, new List<Guid>(), new List<Guid>());

        private static readonly AccessProperties AccessProperties =
            new AccessProperties(Access.Everyone, Access.Everyone);

        public static (SecuritySetting, AccessProperties) GetDefaultSecuritySetting()
        {
            return (SecuritySetting, AccessProperties);
        }
    }
}

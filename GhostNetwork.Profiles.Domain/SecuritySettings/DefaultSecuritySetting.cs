using System;
using System.Collections.Generic;

namespace GhostNetwork.Profiles.SecuritySettings
{
    public static class DefaultSecuritySetting
    {
        private static readonly SecuritySetting SecuritySetting = new SecuritySetting(default, default, Access.Everyone, Access.Everyone, new List<Guid>(), new List<Guid>());

        public static SecuritySetting GetDefaultSecuritySetting()
        {
            return SecuritySetting;
        }
    }
}

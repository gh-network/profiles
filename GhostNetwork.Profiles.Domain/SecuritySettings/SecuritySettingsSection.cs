using System;
using System.Collections.Generic;

namespace GhostNetwork.Profiles.SecuritySettings
{
    public class SecuritySettingsSection
    {
        public SecuritySettingsSection(Access access, IEnumerable<Guid> certainUsers)
        {
            Access = access;
            CertainUsers = certainUsers;
        }

        public Access Access { get; }

        public IEnumerable<Guid> CertainUsers { get; }
    }
}
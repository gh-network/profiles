using System;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.SecuritySettings
{
    public interface IAccessResolver
    {
        Task<bool> ResolveAccessAsync(Guid userId, Guid toUserId, string sectionName);
    }
}

using System;
using System.Threading.Tasks;
using GhostNetwork.Profiles.Friends;

namespace GhostNetwork.Profiles.SecuritySettings
{
    public class AccessResolver : IAccessResolver
    {
        private readonly ISecuritySettingStorage securitySettingStorage;
        private readonly IRelationsService relationsService;

        public AccessResolver(ISecuritySettingStorage securitySettingStorage, IRelationsService relationsService)
        {
            this.relationsService = relationsService;
            this.securitySettingStorage = securitySettingStorage;
        }

        public async Task<bool> ResolveAccessAsync(Guid userId, Guid toUserId, string sectionName)
        {
            if (userId == toUserId)
            {
                return true;
            }

            var section = await securitySettingStorage.FindSectionByUserIdAsync(toUserId, sectionName);

            if (section.Access == Access.NoOne)
            {
                return false;
            }

            if (section.Access == Access.OnlyFriends)
            {
                if (!await relationsService.IsFriendAsync(userId, toUserId))
                {
                    return false;
                }
            }

            if (section.Access == Access.OnlyCertainUsers)
            {
                if (!await securitySettingStorage.ContainsInCertainUsersAsync(userId, toUserId, sectionName))
                {
                    return false;
                }
            }

            if (section.Access == Access.EveryoneExceptCertainUsers)
            {
                if (await securitySettingStorage.ContainsInCertainUsersAsync(userId, toUserId, sectionName))
                {
                    return false;
                }
            }

            return true;
        }
    }
}

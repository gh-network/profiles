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

            return await securitySettingStorage.GetSectionAccessAsync(toUserId, sectionName) switch
            {
                Access.Everyone => true,
                Access.NoOne => false,
                Access.OnlyFriends => await relationsService.IsFriendAsync(userId, toUserId),
                Access.OnlyCertainUsers => await securitySettingStorage
                    .ContainsInCertainUsersAsync(userId, toUserId, sectionName),
                Access.EveryoneExceptCertainUsers => !await securitySettingStorage
                    .ContainsInCertainUsersAsync(userId, toUserId, sectionName),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}

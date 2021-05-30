using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.FriendsFunctionality
{
    public interface IFriendsStorage
    {
        Task<(IEnumerable<FriendsResponseModel>, long)> GetFriendsAsync(int skip, int take, Guid id);

        Task<(IEnumerable<FriendsResponseModel>, long)> GetFollowersAsync(int skip, int take, Guid id);

        Task<(IEnumerable<FriendsResponseModel>, long)> GetFollowedAsync(int skip, int take, Guid id);

        Task UpsertAsync(Friends friends);

        Task DeleteAsync(Guid userOne, Guid userTwo);
    }
}
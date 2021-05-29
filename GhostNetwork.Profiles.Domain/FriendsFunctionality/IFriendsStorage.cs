using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.FriendsFunctionality
{
    public interface IFriendsStorage
    {
        Task<(IEnumerable<Response>, long)> GetFriendsAsync(int skip, int take, Guid id);

        Task<(IEnumerable<Response>, long)> GetFollowersAsync(int skip, int take, Guid id);

        Task<(IEnumerable<Response>, long)> GetFollowedAsync(int skip, int take, Guid id);

        Task UpsertAsync(Friends friends);

        Task DeleteAsync(Guid userOne, Guid userTwo);
    }
}
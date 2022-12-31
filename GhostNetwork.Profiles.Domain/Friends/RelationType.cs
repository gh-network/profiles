using System.ComponentModel;

namespace GhostNetwork.Profiles.Friends;

public enum RelationType
{
    [Description("NoRelation")]
    NoRelation = 0,
    [Description("Friend")]
    Friend = 1,
    [Description("Follower")]
    PendingFollower = 2,
    [Description("Follower")]
    DeclinedFollower = 3,
    [Description("Following")]
    Following = 4
}

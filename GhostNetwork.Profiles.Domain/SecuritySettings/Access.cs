using System.ComponentModel;
using System.Runtime.Serialization;

namespace GhostNetwork.Profiles.SecuritySettings
{
    public enum Access
    {
        [Description("Everyone")]
        Everyone = 0,
        [Description("OnlyFriends")]
        OnlyFriends = 1,
        [Description("NoOne")]
        NoOne = 2,
        [Description("OnlyCertainUsers")]
        OnlyCertainUsers = 3,
        [Description("EveryoneExceptCertainUsers")]
        EveryoneExceptCertainUsers = 4
    }
}

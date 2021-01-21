namespace GhostNetwork.Profiles.SecuritySettings
{
    public class AccessProperties
    {
        public AccessProperties(Access accessToPosts, Access accessToFriends)
        {
            AccessToPosts = accessToPosts;
            AccessToFriends = accessToFriends;
        }

        public Access AccessToPosts { get; private set; }

        public Access AccessToFriends { get; private set; }

        public AccessProperties Update(Access accessToPosts, Access accessToFriends)
        {
            AccessToPosts = accessToPosts;
            AccessToFriends = accessToFriends;

            return this;
        }
    }
}

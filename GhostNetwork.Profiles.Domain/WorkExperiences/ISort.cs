using System.Collections.Generic;

namespace GhostNetwork.Profiles.WorkExperiences
{
    public interface ISort<T>
    {
        IList<T> Sort(IList<T> array);
    }
}

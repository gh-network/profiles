using System.Collections.Generic;

namespace GhostNetwork.Profiles.WorkExperiences
{
    public interface IWorkExperienceSort<T>
    {
        IList<T> Sort(IList<T> array);
    }
}

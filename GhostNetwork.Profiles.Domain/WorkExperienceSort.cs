using System.Collections.Generic;
using GhostNetwork.Profiles.WorkExperiences;

namespace GhostNetwork.Profiles
{
    public class WorkExperienceSort
    {
        public IList<WorkExperience> Sort(IList<WorkExperience> array)
        {
            WorkExperience temp;
            for (int i = 0; i < array.Count - 1; i++)
            {
                for (int j = i + 1; j < array.Count; j++)
                {
                    if (!array[i].StartWork.HasValue)
                    {
                        temp = array[i];
                        array[i] = array[array.Count - 1];
                        array[array.Count - 1] = temp;
                    }
                    else if (!array[j].StartWork.HasValue)
                    {
                        temp = array[j];
                        array[j] = array[array.Count - 1];
                        array[array.Count - 1] = temp;
                    }
                    else if (array[i].StartWork > array[j].StartWork)
                    {
                        temp = array[i];
                        array[i] = array[j];
                        array[j] = temp;
                    }
                }
            }

            return array;
        }
    }
}

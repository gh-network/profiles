using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GhostNetwork.Profiles.WorkExperiences;
using NUnit.Framework;

namespace GhostNetwork.Profiles.Tests
{
    public class WorkExperienceSortTests
    {
        [Test]
        public void Test_Sort()
        {
            // Setup
            var sortService = new WorkExperienceSort();
            var list = new List<WorkExperience>();
            list.Add(new WorkExperience("1","1","Name", "Description", null, null));
            list.Add(new WorkExperience("2", "1", "Name", "Description", new DateTimeOffset(2020, 12, 20, 0, 0, 0, TimeSpan.Zero), null));
            list.Add(new WorkExperience("3", "1", "Name", "Description", new DateTimeOffset(2020, 1, 19, 0, 0, 0, TimeSpan.Zero), null));
            list.Add(new WorkExperience("4", "1", "Name", "Description", new DateTimeOffset(2020, 3, 10, 0, 0, 0, TimeSpan.Zero), null));
            


            // Act
            var result = sortService.Sort(list);

            // Assert
            Assert.IsTrue(result[0].Id == "3");
            Assert.IsTrue(result[1].Id == "4");
            Assert.IsTrue(result[2].Id == "2");
            Assert.IsTrue(result[3].Id == "1");
        }
    }
}

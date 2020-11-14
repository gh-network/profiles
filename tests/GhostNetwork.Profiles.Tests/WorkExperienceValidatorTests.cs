using System;
using System.Linq;
using GhostNetwork.Profiles.WorkExperiences;
using NUnit.Framework;

namespace GhostNetwork.Profiles.Tests
{
    public class WorkExperienceValidatorTests
    {
        [Test]
        public void Empty_Company_Name()
        {
            // Setup
            var validator = new WorkExperienceValidator();

            // Act
            var result = validator.Validate(new WorkExperienceContext("", "Description", DateTimeOffset.MinValue, null));

            // Assert
            Assert.IsFalse(result.Successed);
        }

        [Test]
        public void Start_Work_Greater_Than_Finish_Work()
        {
            // Setup
            var validator = new WorkExperienceValidator();

            // Act
            var result = validator.Validate(new WorkExperienceContext("CompanyName", "Description", DateTimeOffset.Now.AddDays(1), DateTimeOffset.Now));

            // Assert
            Assert.IsFalse(result.Successed);
        }

        [Test]
        public void Start_Work_Greater_Than_Date_Now()
        {
            // Setup
            var validator = new WorkExperienceValidator();

            // Act
            var result = validator.Validate(new WorkExperienceContext("CompanyName", "Description", DateTimeOffset.Now.AddDays(1), DateTimeOffset.Now));

            // Assert
            Assert.IsFalse(result.Successed);
        }

        [Test]
        public void Finish_Work_Greater_Than_Date_Now()
        {
            // Setup
            var validator = new WorkExperienceValidator();

            // Act
            var result = validator.Validate(new WorkExperienceContext("CompanyName", "Description",DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1)));

            // Assert
            Assert.IsFalse(result.Successed);
        }


        [Test]
        public void Correct_Request()
        {
            // Setup
            var validator = new WorkExperienceValidator();

            // Act
            var result = validator.Validate(new WorkExperienceContext("CompanyName", "Description", DateTimeOffset.MinValue, null));

            // Assert
            Assert.IsTrue(result.Successed);
        }
    }
}

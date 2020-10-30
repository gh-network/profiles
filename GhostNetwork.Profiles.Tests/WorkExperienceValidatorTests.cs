using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GhostNetwork.Profiles.Domain;
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
            var result = validator.Validate(new WorkExperienceContext("",DateTime.MinValue, null));

            // Assert
            Assert.IsFalse(result.Successed && result.Errors.Count() == 1);
        }

        [Test]
        public void Start_Work_Greater_Than_Finish_Work()
        {
            // Setup
            var validator = new WorkExperienceValidator();

            // Act
            var result = validator.Validate(new WorkExperienceContext("", DateTime.Now.AddDays(1), DateTime.Now));

            // Assert
            Assert.IsFalse(result.Successed && result.Errors.Count() == 2);
        }

        [Test]
        public void Start_Work_Greater_Than_Date_Now()
        {
            // Setup
            var validator = new WorkExperienceValidator();

            // Act
            var result = validator.Validate(new WorkExperienceContext("1", DateTime.Now.AddDays(1), DateTime.Now));

            // Assert
            Assert.IsFalse(result.Successed && result.Errors.Count() == 2);
        }

        [Test]
        public void Finish_Work_Greater_Than_Date_Now()
        {
            // Setup
            var validator = new WorkExperienceValidator();

            // Act
            var result = validator.Validate(new WorkExperienceContext("1", DateTime.Now, DateTime.Now.AddHours(1)));

            // Assert
            Assert.IsFalse(result.Successed && result.Errors.Count() == 1);
        }

        [Test]
        public void Correct_Request()
        {
            // Setup
            var validator = new WorkExperienceValidator();

            // Act
            var result = validator.Validate(new WorkExperienceContext("1", DateTime.MinValue, null));

            // Assert
            Assert.IsTrue(result.Successed);
        }
    }
}

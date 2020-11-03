using System;
using System.Linq;
using NUnit.Framework;

namespace GhostNetwork.Profiles.Tests
{
    public class ProfileValidatorTests
    {
        [Test]
        public void One_Null_Argument()
        {
            // Setup
            var validator = new ProfileValidator();

            // Act
            var result = validator.Validate(new ProfileContext("", "LastName", DateTime.Now, "City"));

            // Assert
            Assert.IsFalse(result.Successed && result.Errors.Count() == 1);
        }

        [Test]
        public void Date_Greater_Than_Date_Now()
        {
            // Setup
            var validator = new ProfileValidator();

            // Act
            var result = validator.Validate(new ProfileContext("", "LastName", DateTime.MaxValue, "City"));

            // Assert
            Assert.IsFalse(result.Successed && result.Errors.Count() == 2);
        }

        [Test]
        public void Correct_Data()
        {
            // Setup
            var validator = new ProfileValidator();

            // Act
            var result = validator.Validate(new ProfileContext("FirstName", "LastName", DateTime.Now, "City"));

            // Assert
            Assert.IsTrue(result.Successed);
        }
    }
}
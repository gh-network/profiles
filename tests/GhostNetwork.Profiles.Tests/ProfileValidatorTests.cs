using System;
using System.Linq;
using NUnit.Framework;

namespace GhostNetwork.Profiles.Tests
{
    public class ProfileValidatorTests
    {
        [Test]
        public void FirstName_Null_Argument()
        {
            // Setup
            var validator = new ProfileValidator();

            // Act
            var result = validator.Validate(new ProfileContext("", "LastName", "London", DateTime.Now, "man"));

            // Assert
            Assert.IsFalse(result.Successed && result.Errors.Count() == 1);
        }

        public void LastName_Null_Argument()
        {
            // Setup
            var validator = new ProfileValidator();

            // Act
            var result = validator.Validate(new ProfileContext("FirstName", "", "London", DateTime.Now, "man"));

            // Assert
            Assert.IsFalse(result.Successed && result.Errors.Count() == 1);
        }

        [Test]
        public void Date_Greater_Than_Date_Now()
        {
            // Setup
            var validator = new ProfileValidator();

            // Act
            var result = validator.Validate(new ProfileContext("FirstName", "LastName", "London", DateTime.Now.AddDays(1), "man"));

            // Assert
            Assert.IsFalse(result.Successed && result.Errors.Count() == 1);
        }

        [Test]
        public void Correct_Data()
        {
            // Setup
            var validator = new ProfileValidator();

            // Act
            var result = validator.Validate(new ProfileContext("FirstName", "LastName", "London", DateTime.Now, "man"));

            // Assert
            Assert.IsTrue(result.Successed);
        }

        [Test]
        public void Null_Data()
        {
            // Setup
            var validator = new ProfileValidator();

            // Act
            var result = validator.Validate(new ProfileContext("FirstName", "LastName", "London", null, "man"));

            // Assert
            Assert.IsTrue(result.Successed);
        }

        [Test]
        public void Null_Gender()
        {
            // Setup
            var validator = new ProfileValidator();

            // Act
            var result = validator.Validate(new ProfileContext("FirstName", "LastName", "London", DateTime.Now, null));

            // Assert
            Assert.IsTrue(result.Successed);
        }

        [Test]
        public void Null_City()
        {
            // Setup
            var validator = new ProfileValidator();

            // Act
            var result = validator.Validate(new ProfileContext("FirstName", "LastName", null, DateTime.Now, "gender"));

            // Assert
            Assert.IsTrue(result.Successed);
        }
    }
}
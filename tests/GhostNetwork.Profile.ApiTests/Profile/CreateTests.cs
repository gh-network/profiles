using System;
using System.Net;
using System.Threading.Tasks;
using Domain;
using GhostNetwork.Profiles;
using GhostNetwork.Profiles.Api.Models;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace GhostNetwork.Profile.ApiTests.Profile
{
    [TestFixture]
    public class CreateTests
    {
        //O Kurva!
        /*[Test]
        public async Task Create_Created()
        {
            //Setup
            var input = new ProfileCreateViewModel
            {
                Id = Guid.NewGuid(),
                FirstName = "First",
                LastName = "Last",
                Gender = "Gender",
                DateOfBirth = DateTimeOffset.Now,
                City = "City"
            };

            var id = Guid.NewGuid();

            var profile = new Profiles.Profile(id, input.FirstName, input.LastName, input.Gender, input.DateOfBirth,
                input.City);

            var serviceMock  = new Mock<IProfileService>();

            serviceMock 
                .Setup(x => x.CreateAsync(id, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    DateTimeOffset.Now, It.IsAny<string>()))
                .ReturnsAsync((DomainResult.Success(), profile));

            serviceMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(profile);

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock .Object);
            });
            
            //Act
            var response = await client.PostAsync("/profiles/", input.AsJsonContent());
            
            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }
        
        [Test]
        public async Task Create_BadRequest()
        {
            //Setup
            var input = new ProfileCreateViewModel
            {
                Id = Guid.NewGuid(),
                FirstName = "First",
                LastName = "Last",
                Gender = "Gender",
                DateOfBirth = DateTimeOffset.Now,
                City = "City"
            };
            
            var id = Guid.NewGuid();

            var serviceMock  = new Mock<IProfileService>();

            serviceMock 
                .Setup(x => x.CreateAsync(id, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    DateTimeOffset.Now, It.IsAny<string>()))
                .ReturnsAsync((DomainResult.Error("Some"), default));

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock .Object);
            });
            
            //Act
            var response = await client.PostAsync("/profiles/", input.AsJsonContent());
            
            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }*/
    }
}
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GhostNetwork.Profiles;
using GhostNetwork.Profiles.Api.Models;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace GhostNetwork.Profile.ApiTests.Profile
{
    [TestFixture]
    public class GetTests
    {
        [Test]
        public async Task GetById_Ok()
        {
            //Setup
            var id = Guid.NewGuid();

            var profile = new Profiles.Profile(id, "firstName", "lastName", "Male", DateTimeOffset.Now, "City");

            var serviceMock = new Mock<IProfileService>();

            serviceMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(profile);

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
            });
            
            //Act
            var response = await client.GetAsync($"profiles/{id}");
            
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Test]
        public async Task GetById_NotFound()
        {
            //Setup
            var id = Guid.NewGuid();

            var serviceMock = new Mock<IProfileService>();

            serviceMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(default(Profiles.Profile));

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
            });
            
            //Act
            var response = await client.GetAsync($"profiles/{id}");
            
            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task SearchByIds_Ok()
        {
            //Setup
            var model = new ProfilesQueryModel()
            {
                Ids = new List<Guid>()
            };

            var serviceMock = new Mock<IProfileService>();

            serviceMock
                .Setup(x => x.SearchByIdsAsync(model.Ids))
                .ReturnsAsync(new List<Profiles.Profile>());

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
            });
            
            //Act
            var response = await client.PostAsync($"profiles/search", model.AsJsonContent());
            
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
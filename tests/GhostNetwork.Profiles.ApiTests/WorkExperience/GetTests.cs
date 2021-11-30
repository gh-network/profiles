using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GhostNetwork.Profiles;
using GhostNetwork.Profiles.WorkExperiences;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace GhostNetwork.Profile.ApiTests.WorkExperience
{
    [TestFixture]
    public class GetTests
    {
        [Test]
        public async Task GetById_Ok()
        {
            //Setup
            var id = Guid.NewGuid();

            var profileServiceMock = new Mock<IProfileService>();
            var serviceMock = new Mock<IWorkExperienceService>();

            serviceMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(new Profiles.WorkExperiences.WorkExperience(id, It.IsAny<string>(), It.IsAny<string>(), DateTimeOffset.Now,
                    DateTimeOffset.Now.AddDays(1), Guid.NewGuid()));

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
                collection.AddScoped(_ => profileServiceMock.Object);
            });
            
            //Act
            var response = await client.GetAsync($"/WorkExperiences/{id}");
            
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Test]
        public async Task GetById_NotFound()
        {
            //Setup
            var id = Guid.NewGuid();

            var profileServiceMock = new Mock<IProfileService>();
            var serviceMock = new Mock<IWorkExperienceService>();

            serviceMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(default(Profiles.WorkExperiences.WorkExperience));

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
                collection.AddScoped(_ => profileServiceMock.Object);
            });
            
            //Act
            var response = await client.GetAsync($"/WorkExperiences/{id}");
            
            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task GetByProfileId_NotFound()
        {
            //Setup
            var profileId = Guid.NewGuid();

            var profileServiceMock = new Mock<IProfileService>();
            var serviceMock = new Mock<IWorkExperienceService>();

            serviceMock
                .Setup(x => x.FindByProfileId(profileId))
                .ReturnsAsync(default(IEnumerable<Profiles.WorkExperiences.WorkExperience>));
            
            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
                collection.AddScoped(_ => profileServiceMock.Object);
            });
            
            //Act
            var response = await client.GetAsync($"/WorkExperiences/byprofile/{profileId}");
            
            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
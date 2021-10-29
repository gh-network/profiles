using System;
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
    public class DeleteTests
    {
        [Test]
        public async Task Delete_NoContent()
        {
            //Setup
            var id = Guid.NewGuid();

            var serviceMock = new Mock<IWorkExperienceService>();
            var profileServiceMock = new Mock<IProfileService>();

            serviceMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(new Profiles.WorkExperiences.WorkExperience(id, It.IsAny<string>(), It.IsAny<string>(),
                    DateTimeOffset.Now, DateTimeOffset.Now, Guid.NewGuid()));
            
            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
                collection.AddScoped(_ => profileServiceMock.Object);
            });
            
            //Act
            var response = await client.DeleteAsync($"/WorkExperiences/{id}");
            
            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }
        
        [Test]
        public async Task Delete_NotFound()
        {
            //Setup
            var id = Guid.NewGuid();

            var serviceMock = new Mock<IWorkExperienceService>();
            var profileServiceMock = new Mock<IProfileService>();

            serviceMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(default(Profiles.WorkExperiences.WorkExperience));
            
            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
                collection.AddScoped(_ => profileServiceMock.Object);
            });
            
            //Act
            var response = await client.DeleteAsync($"/WorkExperiences/{id}");
            
            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
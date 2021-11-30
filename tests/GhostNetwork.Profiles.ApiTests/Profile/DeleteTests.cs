using System;
using System.Net;
using System.Threading.Tasks;
using GhostNetwork.Profiles;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace GhostNetwork.Profile.ApiTests.Profile
{
    [TestFixture]
    public class DeleteTests
    {
        [Test]
        public async Task Delete_NoContent()
        {
            //Setup
            var id = Guid.NewGuid();
            
            var profile = new Profiles.Profile(id, "firstName", "lastName", "Male", DateTimeOffset.Now, "City", string.Empty);

            var serviceMock = new Mock<IProfileService>();

            serviceMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(profile);

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
            });
            
            //Act
            var response = await client.DeleteAsync($"profiles/{id}");
            
            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }
        
        [Test]
        public async Task Delete_NotFound()
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
            var response = await client.DeleteAsync($"profiles/{id}");
            
            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
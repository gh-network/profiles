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
    public class UpdateTests
    {
        [Test]
        public async Task Update_NoContent()
        {
            //Setup
            var id = Guid.NewGuid();

            var input = new ProfileUpdateViewModel()
            {
                FirstName = "Upd",
                LastName = "Upd",
                Gender = "Upd",
                DateOfBirth = DateTimeOffset.Now.AddDays(1),
                City = "Ct"
            };

            var serviceMock = new Mock<IProfileService>();

            serviceMock
                .Setup(x => x.UpdateAsync(id, input.FirstName, input.LastName, input.Gender, input.DateOfBirth,
                    input.City))
                .ReturnsAsync(DomainResult.Success);

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
            });
            
            //Act 
            var response = await client.PutAsync($"profiles/{id}", input.AsJsonContent());
            
            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }
        
        [Test]
        public async Task Update_NotFound()
        {
            //Setup
            var id = Guid.NewGuid();

            var input = new ProfileUpdateViewModel()
            {
                FirstName = "Upd",
                LastName = "Upd",
                Gender = "Upd",
                DateOfBirth = DateTimeOffset.Now.AddDays(1),
                City = "Ct"
            };

            var serviceMock = new Mock<IProfileService>();

            serviceMock
                .Setup(x => x.UpdateAsync(id, input.FirstName, input.LastName, input.Gender, input.DateOfBirth,
                    input.City))
                .ReturnsAsync(DomainResult.Error("NotFound"));

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
            });
            
            //Act 
            var response = await client.PutAsync($"profiles/{id}", input.AsJsonContent());
            
            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
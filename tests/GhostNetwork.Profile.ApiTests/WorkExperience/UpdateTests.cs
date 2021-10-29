using System;
using System.Net;
using System.Threading.Tasks;
using Domain;
using GhostNetwork.Profiles;
using GhostNetwork.Profiles.Api.Models;
using GhostNetwork.Profiles.WorkExperiences;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace GhostNetwork.Profile.ApiTests.WorkExperience
{
    [TestFixture]
    public class UpdateTests
    {
        [Test]
        public async Task Update_NoContent()
        {
            //Setup
            var id = Guid.NewGuid();

            var model = new WorkExperienceUpdateViewModel
            {
                CompanyName = "Company",
                Description = "Desc",
                StartWork = DateTimeOffset.Now,
                FinishWork = DateTimeOffset.Now.AddDays(1)
            };

            var profileServiceMock = new Mock<IProfileService>();
            var experienceServiceMock = new Mock<IWorkExperienceService>();

            experienceServiceMock
                .Setup(x => x.UpdateAsync(id, model.CompanyName, model.Description, model.StartWork, model.FinishWork))
                .ReturnsAsync(DomainResult.Success);
            
            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => experienceServiceMock.Object);
                collection.AddScoped(_ => profileServiceMock.Object);
            });
            
            //Act
            var response = await client.PutAsync($"/WorkExperiences/{id}", model.AsJsonContent());
            
            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }
        
        [Test]
        public async Task Update_NotFound()
        {
            //Setup
            var id = Guid.NewGuid();

            var model = new WorkExperienceUpdateViewModel
            {
                CompanyName = "Company",
                Description = "Desc",
                StartWork = DateTimeOffset.Now,
                FinishWork = DateTimeOffset.Now.AddDays(1)
            };

            var profileServiceMock = new Mock<IProfileService>();
            var experienceServiceMock = new Mock<IWorkExperienceService>();

            experienceServiceMock
                .Setup(x => x.UpdateAsync(id, model.CompanyName, model.Description, model.StartWork, model.FinishWork))
                .ReturnsAsync(DomainResult.Error("SomeError"));
            
            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => experienceServiceMock.Object);
                collection.AddScoped(_ => profileServiceMock.Object);
            });
            
            //Act
            var response = await client.PutAsync($"/WorkExperiences/{id}", model.AsJsonContent());
            
            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
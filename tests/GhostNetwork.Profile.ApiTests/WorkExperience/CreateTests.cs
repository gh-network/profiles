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
    public class CreateTests
    {
        [Test]
        public async Task Create_Created()
        {
            //Setup
            var id = Guid.NewGuid();

            var model = new WorkExperienceCreateViewModel
            {
                CompanyName = "Some",
                Description = "Desc",
                StartWork = DateTimeOffset.Now,
                FinishWork = DateTimeOffset.Now.AddDays(1),
                ProfileId = Guid.NewGuid()
            };

            var serviceMock = new Mock<IWorkExperienceService>();
            var profileServiceMock = new Mock<IProfileService>();

            serviceMock
                .Setup(x => x.CreateAsync(model.CompanyName, model.Description, model.StartWork, model.FinishWork,
                    model.ProfileId))
                .ReturnsAsync((DomainResult.Success(), id));

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
                collection.AddScoped(_ => profileServiceMock.Object);
            });
            
            //Act
            var response = await client.PostAsync($"/WorkExperiences", model.AsJsonContent());
            
            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }
        
        [Test]
        public async Task Create_BadRequest()
        {
            //Setup
            var id = Guid.NewGuid();

            var model = new WorkExperienceCreateViewModel
            {
                CompanyName = null,
                Description = null,
                StartWork = DateTimeOffset.Now,
                FinishWork = DateTimeOffset.Now.AddDays(1),
                ProfileId = Guid.Empty
            };

            var serviceMock = new Mock<IWorkExperienceService>();
            var profileServiceMock = new Mock<IProfileService>();

            serviceMock
                .Setup(x => x.CreateAsync(model.CompanyName, model.Description, model.StartWork, model.FinishWork,
                    model.ProfileId))
                .ReturnsAsync((DomainResult.Error("Smt error"), default));

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
                collection.AddScoped(_ => profileServiceMock.Object);
            });
            
            //Act
            var response = await client.PostAsync($"/WorkExperiences", model.AsJsonContent());
            
            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
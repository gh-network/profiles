using Domain;
using GhostNetwork.Profiles.Api.Models;
using GhostNetwork.Profiles.SecuritySettings;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GhostNetwork.Profile.ApiTests.SecuritySetting
{
    [TestFixture]
    class UpdateTests
    {
        [Test]
        public async Task TestUpdate_NoContent()
        {
            //Setup

            var userId = Guid.NewGuid();

            var model = new SecuritySettingUpdateViewModel
            {
                Posts = new SecuritySettingsSectionInputModel { Access = Access.Everyone, CertainUsers = Enumerable.Empty<Guid>() },
                Friends = new SecuritySettingsSectionInputModel { Access = Access.Everyone, CertainUsers = Enumerable.Empty<Guid>() },
                Comments = new SecuritySettingsSectionInputModel { Access = Access.Everyone, CertainUsers = Enumerable.Empty<Guid>() },
                Reactions = new SecuritySettingsSectionInputModel { Access = Access.Everyone, CertainUsers = Enumerable.Empty<Guid>() },
                Followers = new SecuritySettingsSectionInputModel { Access = Access.Everyone, CertainUsers = Enumerable.Empty<Guid>() }
            };
            

            var serviceMock = new Mock<ISecuritySettingService>();

            serviceMock.Setup(x => x.UpsertAsync(
                    userId, 
                    It.IsAny<SecuritySettingsSection>(), 
                    It.IsAny<SecuritySettingsSection>(), 
                    It.IsAny<SecuritySettingsSection>(), 
                    It.IsAny<SecuritySettingsSection>(), 
                    It.IsAny<SecuritySettingsSection>()))
                .ReturnsAsync(DomainResult.Success());

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
            });

            //Act
            var response = await client.PutAsync($"/profiles/{userId}/security-settings", model.AsJsonContent());

            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task TestUpdate_BadRequest()
        {
            //Setup

            var userId = Guid.NewGuid();

            var model = new SecuritySettingUpdateViewModel
            {
                Posts = new SecuritySettingsSectionInputModel { Access = Access.Everyone, CertainUsers = Enumerable.Empty<Guid>() },
                Friends = new SecuritySettingsSectionInputModel { Access = Access.Everyone, CertainUsers = Enumerable.Empty<Guid>() },
                Comments = new SecuritySettingsSectionInputModel { Access = Access.Everyone, CertainUsers = Enumerable.Empty<Guid>() },
                Reactions = new SecuritySettingsSectionInputModel { Access = Access.Everyone, CertainUsers = Enumerable.Empty<Guid>() },
                Followers = new SecuritySettingsSectionInputModel { Access = Access.Everyone, CertainUsers = Enumerable.Empty<Guid>() }
            };


            var serviceMock = new Mock<ISecuritySettingService>();

            serviceMock.Setup(x => x.UpsertAsync(
                    userId,
                    It.IsAny<SecuritySettingsSection>(),
                    It.IsAny<SecuritySettingsSection>(),
                    It.IsAny<SecuritySettingsSection>(),
                    It.IsAny<SecuritySettingsSection>(),
                    It.IsAny<SecuritySettingsSection>()))
                .ReturnsAsync(DomainResult.Error(string.Empty));

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
            });

            //Act
            var response = await client.PutAsync($"/profiles/{userId}/security-settings", model.AsJsonContent());

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}

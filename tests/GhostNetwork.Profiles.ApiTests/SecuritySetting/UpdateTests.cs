using Domain;
using GhostNetwork.Profiles;
using GhostNetwork.Profiles.Api.Models;
using GhostNetwork.Profiles.SecuritySettings;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net;
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
                Friends = new SecuritySettingsSectionInputModel { Access = Access.Everyone, CertainUsers = Enumerable.Empty<Guid>() },
                Followers = new SecuritySettingsSectionInputModel { Access = Access.Everyone, CertainUsers = Enumerable.Empty<Guid>() },
                Posts = new SecuritySettingsSectionInputModel { Access = Access.Everyone, CertainUsers = Enumerable.Empty<Guid>() },
                Comments = new SecuritySettingsSectionInputModel { Access = Access.Everyone, CertainUsers = Enumerable.Empty<Guid>() },
                ProfilePhoto = new SecuritySettingsSectionInputModel { Access = Access.Everyone, CertainUsers = Enumerable.Empty<Guid>() },
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
            var profileServiceMock = new Mock<IProfileService>();
            var accessResolverMock = new Mock<IAccessResolver>();

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
                collection.AddScoped(_ => profileServiceMock.Object);
                collection.AddScoped(_ => accessResolverMock.Object);
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
                Friends = new SecuritySettingsSectionInputModel { Access = Access.Everyone, CertainUsers = Enumerable.Empty<Guid>() },
                Followers = new SecuritySettingsSectionInputModel { Access = Access.Everyone, CertainUsers = Enumerable.Empty<Guid>() },
                Posts = new SecuritySettingsSectionInputModel { Access = Access.Everyone, CertainUsers = Enumerable.Empty<Guid>() },
                Comments = new SecuritySettingsSectionInputModel { Access = Access.Everyone, CertainUsers = Enumerable.Empty<Guid>() },
                ProfilePhoto = new SecuritySettingsSectionInputModel { Access = Access.Everyone, CertainUsers = Enumerable.Empty<Guid>() },
            };


            var serviceMock = new Mock<ISecuritySettingService>();
            var profileServiceMock = new Mock<IProfileService>();
            var accessResolverMock = new Mock<IAccessResolver>();

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
                collection.AddScoped(_ => profileServiceMock.Object);
                collection.AddScoped(_ => accessResolverMock.Object);
            });

            //Act
            var response = await client.PutAsync($"/profiles/{userId}/security-settings", model.AsJsonContent());

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}

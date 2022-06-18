using GhostNetwork.Profiles;
using GhostNetwork.Profiles.Api.Models;
using GhostNetwork.Profiles.SecuritySettings;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace GhostNetwork.Profile.ApiTests.SecuritySetting
{
    [TestFixture]
    public class CheckAccessTests
    {
        [Test]
        public async Task CheckAccess_Ok()
        {
            // Setup
            const string sectionName = "friends";

            var userId = Guid.NewGuid();
            var toUserId = Guid.NewGuid();

            var inputModel = new SecuritySettingResolvingInputModel
            {
                ToUserId = toUserId,
                SectionName = sectionName
            };

            var profile = new Profiles.Profile(toUserId, "SomeName", "SomeLastName", "male", null, "Odessa", string.Empty);

            var settingsServiceMock = new Mock<ISecuritySettingService>();
            var accessResolverMock = new Mock<IAccessResolver>();
            accessResolverMock.Setup(s => s.ResolveAccessAsync(userId, inputModel.ToUserId, inputModel.SectionName))
                .ReturnsAsync(true);

            var profileServiceMock = new Mock<IProfileService>();
            profileServiceMock.Setup(s => s.GetByIdAsync(toUserId))
                .ReturnsAsync(profile);

            var client = TestServerHelper.New(services =>
            {
                services.AddScoped(_ => accessResolverMock.Object);
                services.AddScoped(_ => profileServiceMock.Object);
                services.AddScoped(_ => settingsServiceMock.Object);
            });

            // Act
            var response = await client.PostAsync($"/profiles/{userId}/security-settings/check-access", inputModel.AsJsonContent());

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task CheckAccess_NotFound()
        {
            // Setup
            const string sectionName = "friends";

            var userId = Guid.NewGuid();
            var toUserId = Guid.NewGuid();

            var inputModel = new SecuritySettingResolvingInputModel
            {
                ToUserId = toUserId,
                SectionName = sectionName
            };

            var settingsServiceMock = new Mock<ISecuritySettingService>();
            var accessResolverMock = new Mock<IAccessResolver>();
            accessResolverMock.Setup(s => s.ResolveAccessAsync(userId, inputModel.ToUserId, inputModel.SectionName))
                .ReturnsAsync(true);

            var profileServiceMock = new Mock<IProfileService>();
            profileServiceMock.Setup(s => s.GetByIdAsync(toUserId))
                .ReturnsAsync(default(Profiles.Profile));

            var client = TestServerHelper.New(services =>
            {
                services.AddScoped(_ => accessResolverMock.Object);
                services.AddScoped(_ => profileServiceMock.Object);
                services.AddScoped(_ => settingsServiceMock.Object);
            });

            // Act
            var response = await client.PostAsync($"/profiles/{userId}/security-settings/check-access", inputModel.AsJsonContent());

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task CheckAccess_Forbid()
        {
            // Setup
            const string sectionName = "friends";

            var userId = Guid.NewGuid();
            var toUserId = Guid.NewGuid();

            var inputModel = new SecuritySettingResolvingInputModel
            {
                ToUserId = toUserId,
                SectionName = sectionName
            };

            var profile = new Profiles.Profile(toUserId, "SomeName", "SomeLastName", "male", null, "Odessa", string.Empty);

            var settingsServiceMock = new Mock<ISecuritySettingService>();
            var accessResolverMock = new Mock<IAccessResolver>();
            accessResolverMock.Setup(s => s.ResolveAccessAsync(userId, inputModel.ToUserId, inputModel.SectionName))
                .ReturnsAsync(false);

            var profileServiceMock = new Mock<IProfileService>();
            profileServiceMock.Setup(s => s.GetByIdAsync(toUserId))
                .ReturnsAsync(profile);

            var client = TestServerHelper.New(services =>
            {
                services.AddScoped(_ => accessResolverMock.Object);
                services.AddScoped(_ => profileServiceMock.Object);
                services.AddScoped(_ => settingsServiceMock.Object);
            });

            // Act
            var response = await client.PostAsync($"/profiles/{userId}/security-settings/check-access", inputModel.AsJsonContent());

            // Assert
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}

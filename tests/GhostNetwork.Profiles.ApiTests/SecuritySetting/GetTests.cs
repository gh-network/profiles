using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GhostNetwork.Profiles.SecuritySettings;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace GhostNetwork.Profile.ApiTests.SecuritySetting
{
    [TestFixture]
    public class GetTests
    {
        [Test]
        public async Task GetByUserId_Ok()
        {
            //Setup
            var userId = Guid.NewGuid();

            var securitySetting = new Profiles.SecuritySettings.SecuritySetting(
                userId,
                new SecuritySettingsSection(Access.Everyone, Enumerable.Empty<Guid>()),
                new SecuritySettingsSection(Access.Everyone, Enumerable.Empty<Guid>()),
                new SecuritySettingsSection(Access.Everyone, Enumerable.Empty<Guid>()),
                new SecuritySettingsSection(Access.Everyone, Enumerable.Empty<Guid>()),
                new SecuritySettingsSection(Access.Everyone, Enumerable.Empty<Guid>()));

            var serviceMock = new Mock<ISecuritySettingService>();

            serviceMock
                .Setup(x => x.GetByUserIdAsync(userId))
                .ReturnsAsync(securitySetting);

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
            });
            
            //Act
            var response = await client.GetAsync($"/profiles/{userId}/security-settings");
            
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Test]
        public async Task GetByUserId_NotFound()
        {
            //Setup
            var userId = Guid.NewGuid();

            var serviceMock = new Mock<ISecuritySettingService>();

            serviceMock
                .Setup(x => x.GetByUserIdAsync(userId))
                .ReturnsAsync(default(Profiles.SecuritySettings.SecuritySetting));

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
            });
            
            //Act
            var response = await client.GetAsync($"/profiles/{userId}/security-settings");
            
            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Domain;
using GhostNetwork.Profiles.Api.Models;
using GhostNetwork.Profiles.SecuritySettings;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace GhostNetwork.Profile.ApiTests.SecuritySetting
{
    [TestFixture]
    public class UpdateTests
    {
        //Object reference not set to an instance of an object
        // [Test]
        // public async Task Update_NoContent()
        // {
        //     //Setup
        //     var userId = Guid.NewGuid();
        //
        //     var model = new SecuritySettingUpdateViewModel
        //     {
        //         Friends = new SecuritySettingsSectionInputModel{ Access = Access.OnlyCertainUsers, CertainUsers = It.IsAny<List<Guid>>() },
        //         Posts = new SecuritySettingsSectionInputModel{ Access = Access.OnlyCertainUsers, CertainUsers = It.IsAny<List<Guid>>() }
        //     };
        //
        //     var serviceMock = new Mock<ISecuritySettingService>();
        //
        //     serviceMock
        //         .Setup(x => x.UpsertAsync(userId,
        //                 new SecuritySettingsSection(model.Posts.Access, model.Posts.CertainUsers ?? Enumerable.Empty<Guid>()),
        //                 new SecuritySettingsSection(model.Friends.Access, model.Friends.CertainUsers ?? Enumerable.Empty<Guid>())))
        //         .ReturnsAsync(DomainResult.Success);
        //
        //     var client = TestServerHelper.New(collection =>
        //     {
        //         collection.AddScoped(_ => serviceMock.Object);
        //     });
        //     
        //     //Act
        //     var response = await client.PutAsync($"/profiles/{userId}/security-settings", model.AsJsonContent());
        //     
        //     //Assert
        //     Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        // }
    }
}
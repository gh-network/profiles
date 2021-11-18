using System;
using System.Net;
using System.Threading.Tasks;
using GhostNetwork.Profiles.Friends;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace GhostNetwork.Profile.ApiTests.Friends
{
    [TestFixture]
    public class CreateTests
    {
        [Test]
        public async Task SendFriendRequest_NoContent()
        {
            //Setup
            var fromUser = Guid.NewGuid();
            var toUser = Guid.NewGuid();

            var serviceMock = new Mock<IRelationsService>();

            serviceMock
                .Setup(x => x.SendRequestAsync(fromUser, toUser));

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
            });
            
            //Act
            var response = await client.PostAsync($"/Relations/{fromUser}/friends/{toUser}", null!);
            
            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }
        
        [Test]
        public async Task SendFriendRequest_BadRequest()
        {
            //Setup
            var fromUser = Guid.NewGuid();
            var toUser = fromUser;

            var serviceMock = new Mock<IRelationsService>();

            serviceMock
                .Setup(x => x.SendRequestAsync(fromUser, toUser));

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
            });
            
            //Act
            var response = await client.PostAsync($"/Relations/{fromUser}/friends/{toUser}", null!);
            
            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
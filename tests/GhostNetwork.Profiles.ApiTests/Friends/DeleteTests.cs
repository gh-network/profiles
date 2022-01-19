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
    public class DeleteTests
    {
        [Test]
        public async Task Delete_Friend_NoContent()
        {
            //Setup
            var user = Guid.NewGuid();
            var friend = Guid.NewGuid();

            var serviceMock = new Mock<IRelationsService>();

            serviceMock
                .Setup(x => x.DeleteFriendAsync(user, friend));

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
            });
            
            //Act
            var response = await client.DeleteAsync($"/Relations/{user}/friends/{friend}");
            
            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }
        
        [Test]
        public async Task Delete_Friend_BadRequest()
        {
            //Setup
            var user = Guid.NewGuid();
            var friend = user;

            var serviceMock = new Mock<IRelationsService>();

            serviceMock
                .Setup(x => x.DeleteFriendAsync(user, friend));

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
            });
            
            //Act
            var response = await client.DeleteAsync($"/Relations/{user}/friends/{friend}");
            
            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task Delete_OutgoingRequest_NoContent()
        {
            //Setup
            var user = Guid.NewGuid();
            var requester = Guid.NewGuid();

            var serviceMock = new Mock<IRelationsService>();

            serviceMock
                .Setup(x => x.CancelOutgoingRequestAsync(user, requester));

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
            });
            
            //Act
            var response = await client.DeleteAsync($"/Relations/{user}/friends/{requester}/outgoing-request");
            
            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task Delete_OutgoingRequest_BadRequest()
        {
            //Setup
            var user = Guid.NewGuid();
            var requester = user;

            var serviceMock = new Mock<IRelationsService>();

            serviceMock
                .Setup(x => x.CancelOutgoingRequestAsync(user, requester));

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
            });
            
            //Act
            var response = await client.DeleteAsync($"/Relations/{user}/friends/{requester}/outgoing-request");
            
            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
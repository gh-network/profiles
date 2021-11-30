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
    public class UpdateTests
    {
        [Test]
        public async Task ApproveFriendRequest_NoContent()
        {
            //Setup
            var user = Guid.NewGuid();
            var requester = Guid.NewGuid();

            var serviceMock = new Mock<IRelationsService>();

            serviceMock
                .Setup(x => x.ApproveRequestAsync(user, requester));

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
            });
            
            //Act
            var response = await client.PutAsync($"/Relations/{user}/friends/{requester}/approve", null!);
            
            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }
        
        [Test]
        public async Task ApproveFriendRequest_BadRequest()
        {
            //Setup
            var user = Guid.NewGuid();
            var requester = user;

            var serviceMock = new Mock<IRelationsService>();

            serviceMock
                .Setup(x => x.ApproveRequestAsync(user, requester));

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
            });
            
            //Act
            var response = await client.PutAsync($"/Relations/{user}/friends/{requester}/approve", null!);
            
            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task DeclineFriendRequest_NoContent()
        {
            //Setup
            var user = Guid.NewGuid();
            var requester = Guid.NewGuid();
            
            var serviceMock = new Mock<IRelationsService>();

            serviceMock
                .Setup(x => x.DeclineRequestAsync(user, requester));

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
            });
            
            //Act
            var response = await client.PutAsync($"/Relations/{user}/friends/{requester}/decline", null!);
            
            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }
        
        [Test]
        public async Task DeclineFriendRequest_BadRequest()
        {
            //Setup
            var user = Guid.NewGuid();
            var requester = user;
            
            var serviceMock = new Mock<IRelationsService>();

            serviceMock
                .Setup(x => x.DeclineRequestAsync(user, requester));

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
            });
            
            //Act
            var response = await client.PutAsync($"/Relations/{user}/friends/{requester}/decline", null!);
            
            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
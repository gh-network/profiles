using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GhostNetwork.Profiles.Grpc.Services
{
    public class ProfilesService : Profiles.ProfilesBase
    {
        private readonly Status userNotFound = new Status(StatusCode.NotFound, "User not found");
        private readonly Status invalidData = new Status(StatusCode.InvalidArgument, "User not found");
        
        private readonly IProfileService profileService;

        public ProfilesService(IProfileService profileService)
        {
            this.profileService = profileService;
        }

        public override async Task<Profile> GetById(ByIdRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.Id, out var id))
            {
                throw new RpcException(userNotFound);
            }
            
            var profile = await profileService.GetByIdAsync(id);

            if (profile == null)
            {
                throw new RpcException(userNotFound);
            }

            return ToGrpcModel(profile);
        }

        public override async Task<Profile> Create(Profile request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.Id, out var id))
            {
                id = Guid.NewGuid();
            }

            var (result, profile) = await profileService.CreateAsync(id, request.FirstName, request.LastName, request.Gender, request.DateOfBirth?.ToDateTime(), request.City);

            if (!result.Successed)
            {
                throw new RpcException(invalidData, ToMetadata(result.Errors));
            }

            return ToGrpcModel(profile);
        }

        public override async Task<Empty> Update(Profile request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.Id, out var id))
            {
                id = Guid.NewGuid();
            }

            var result = await profileService.UpdateAsync(id, request.FirstName, request.LastName, request.Gender, request.DateOfBirth?.ToDateTime(), request.City);

            if (!result.Successed)
            {
                throw new RpcException(invalidData, ToMetadata(result.Errors));
            }

            return new Empty();
        }

        public override async Task<Empty> Delete(ByIdRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.Id, out var id))
            {
                throw new RpcException(userNotFound);
            }

            if (await profileService.GetByIdAsync(id) == null)
            {
                throw new RpcException(userNotFound);
            }

            await profileService.DeleteAsync(id);

            return new Empty();
        }

        private static Profile ToGrpcModel(GhostNetwork.Profiles.Profile profile)
        {
            return new Profile
            {
                Id = profile.Id.ToString(),
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Gender = profile.Gender,
                DateOfBirth = profile.DateOfBirth.HasValue
                    ? Timestamp.FromDateTimeOffset(profile.DateOfBirth.Value)
                    : null,
                City = profile.City
            };
        }

        private static Metadata ToMetadata(IEnumerable<DomainError> errors)
        {
            var meta = new Metadata();
            foreach (var error in errors)
            {
                meta.Add("Validation", error.Message);
            }

            return meta;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Domain;

namespace GhostNetwork.Profiles.Domain
{
    public interface IValidator<in T>
    {
        DomainResult Validate(T param);
    }

    public class ProfileValidator : IValidator<ProfileContext>
    {
        public DomainResult Validate(ProfileContext param)
        {
            var result = Validate(param.DateOfBirth, (nameof(param.FirstName), param.FirstName), (nameof(param.LastName), param.LastName), (nameof(param.City), param.City));

            return result;
        }

        private DomainResult Validate(DateTime date, params (string name, string value)[] str)
        {
            List<DomainError> results = new List<DomainError>();
            foreach (var (name, value) in str)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    results.Add(new DomainError($"{name} is null or empty."));
                }
            }

            if (date > DateTime.Now)
            {
                results.Add(new DomainError("Date is greater than date now"));
            }

            return !results.Any() ? DomainResult.Success() : DomainResult.Error(results);
        }
    }
}

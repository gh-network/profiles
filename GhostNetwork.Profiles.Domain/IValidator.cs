using System;
using System.Collections.Generic;
using System.Linq;
using Domain;

namespace GhostNetwork.Profiles
{
    public interface IValidator<in T>
    {
        DomainResult Validate(T param);
    }

    public class ProfileValidator : IValidator<ProfileContext>
    {
        public DomainResult Validate(ProfileContext param)
        {
            var result = Validate(param.DateOfBirth, param.FirstName, param.LastName, param.Gender);

            return result;
        }

        private DomainResult Validate(DateTime? date, string firstName, string lastName, string gender)
        {
            List<DomainError> results = new List<DomainError>();

            if (firstName == null || firstName.Length > 150 || string.IsNullOrEmpty(firstName))
            {
                results.Add(new DomainError($"{nameof(firstName)} can not be null, empty or more than 150 chars"));
            }

            if (lastName == null || lastName.Length > 150 || string.IsNullOrEmpty(lastName))
            {
                results.Add(new DomainError($"{nameof(lastName)} can not be null, empty or more than 150 chars"));
            }

            if (gender != null && (gender.Length > 150 || string.IsNullOrEmpty(gender)))
            {
                results.Add(new DomainError($"{nameof(gender)} can not be empty or more than 150 chars"));
            }

            if (date != null && date > DateTime.Now)
            {
                results.Add(new DomainError("Date is greater than date now"));
            }

            return !results.Any() ? DomainResult.Success() : DomainResult.Error(results);
        }
    }
}

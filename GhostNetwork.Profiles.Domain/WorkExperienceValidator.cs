﻿using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace GhostNetwork.Profiles.Domain
{
    public class WorkExperienceValidator : IValidator<WorkExperienceContext>
    {
        public DomainResult Validate(WorkExperienceContext param)
        {
            List<DomainError> errors = new List<DomainError>();

            if (string.IsNullOrEmpty(param.CompanyName))
            {
                errors.Add(new DomainError("Company name is null or empty."));
            }

            if (param.FinishWork != null && param.FinishWork > DateTime.Now)
            {
                errors.Add(new DomainError("Finish work can not be greater than date now."));
            }

            if (param.StartWork > DateTime.Now)
            {
                errors.Add(new DomainError("Start work can not be greater than date now."));
            }

            if (param.StartWork > param.FinishWork)
            {
                errors.Add(new DomainError("Finish work must be greater than start work."));
            }

            if (param.StartWork > param.FinishWork)
            {
                errors.Add(new DomainError("Start work is greater than finish work."));
            }

            return DomainResult.Error(errors);
        }
    }
}

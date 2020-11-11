using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace GhostNetwork.Profiles.MsSQL
{
    public class ProfileEntity
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(150)]
        public string LastName { get; set; }

        [MaxLength(150)]
        public string Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(150)]
        public string City { get; set; }

        public virtual List<WorkExperienceEntity> WorkExperience { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.Api.Models
{
    public class WorkExperienceCreateViewModel
    {
        public string CompanyName { get; set; }

        public DateTime StartWork { get; set; }

        public DateTime FinishWork { get; set; }

        public long ProfileId { get; set; }
    }
}

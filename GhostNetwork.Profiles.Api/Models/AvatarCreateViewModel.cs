using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.Api.Models
{
    public class AvatarCreateViewModel
    {
        public string FilePath { get; set; }

        public string FileName { get; set; }

        public string ProfileId { get; set; }
    }
}

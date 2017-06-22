using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PORTFOLIO.api.Models
{
    public class Stuff
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Categories { get; set; }
        public string Link { get; set; }
        public string Year { get; set; }
        public int Order { get; set; }
        public string RepoLink { get; set; }
        public string Role { get; set; }
    }
}

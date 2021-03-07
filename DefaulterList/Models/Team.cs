using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaulterList.Models
{
    public class Team
    {
        public int Id { get; set; }        
        public string NameTeam { get; set; }
        public string Descriptions { get; set; }
                 
        public List<Result> Results { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaulterList.Models
{
    public class TotalList
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }

        public List<Defaulter> Defaulters { get; set; }
    }
}

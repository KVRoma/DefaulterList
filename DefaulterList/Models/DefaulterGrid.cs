using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaulterList.Models
{
    public class DefaulterGrid
    {
        public string Number { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public decimal DebtTOV { get; set; }
        public decimal DebtRZP { get; set; }
        public int? TotalListId { get; set; }
        public int? DefaulterId { get; set; }
    }
}

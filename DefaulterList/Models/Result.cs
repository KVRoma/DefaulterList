using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaulterList.Models
{
    public class Result
    {
        public int Id { get; set; }
        public DateTime DateResult { get; set; }
        public string Description { get; set; }
        public decimal Payment { get; set; }
        public bool IsDisabled { get; set; } = false;

        public int? TeamId { get; set; }
        public Team Team { get; set; }

        public int? TotalListId { get; set; }
        public TotalList TotalList { get; set; }

    }
}

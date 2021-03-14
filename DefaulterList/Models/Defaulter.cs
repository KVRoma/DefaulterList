using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaulterList.Models
{
    public class Defaulter
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal DebtTOV { get; set; }
        public decimal DebtRZP { get; set; }

        public DateTime? DateResult { get; set; }
        public string DescriptionResult { get; set; }
        public decimal PaymentTOVResult { get; set; }
        public decimal PaymentRZPResult { get; set; }
        public bool IsDisabled { get; set; } = false;

        public string NameTeam { get; set; }
        public string Descriptions { get; set; }

        public string Color { get; set; } = "White";

        public int? TotalListId { get; set; }
        public TotalList TotalList { get; set; }

        public string FullNameItem 
        {
            get 
            { 
                return NameTeam +  " - ( О/р  " + TotalList.Number + "   < " + TotalList.Address + "  -  " + TotalList.Name + " > )"; 
            }
        }

        public string Search
        {
            get
            {
                return TotalList.Number + TotalList.City;
            }
        }

        
    }
}

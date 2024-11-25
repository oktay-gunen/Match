using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match.Entities.Models.ReportDbModel
{

    //FP-01			
    public class FP_GeneralSummary_01
    {
        public string DisplayId { get; set; }
        public string DisplayTitle { get; set; }
        public string ReportYear { get; set; }
        public int ReportMonth { get; set; }
        public double Price { get; set; }
    }
}
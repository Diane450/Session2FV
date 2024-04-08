using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session2v2.Models
{
    public class AcceptedGroupRequest
    {
        public int GroupRequestId { get; set; }

        public DateOnly DateVisit { get; set; }

        public TimeOnly Time { get; set; }

    }
}

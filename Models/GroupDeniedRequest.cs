using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session2v2.Models
{
    public class GroupDeniedRequest
    {
        public int Id { get; set; }

        public int GroupRequestId { get; set; }

        public int DeniedReasonId { get; set; }

    }
}

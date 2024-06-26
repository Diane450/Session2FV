﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session2v2.Models
{
    public class AcceptedPrivateRequest
    {
        public int PrivateRequestId { get; set; }

        public DateOnly DateVisit { get; set; }

        public TimeOnly Time { get; set; }

        public string ClientEmail { get; set; } = null!;

        public DateOnly CreationDate { get; set; }
    }
}

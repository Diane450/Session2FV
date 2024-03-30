using Avalonia.Media.Imaging;
using Session2v2.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session2v2.Models
{
    public class PrivateRequest : Request
    {
        public async override Task GetDeniedReason()
        {
            await DBCall.GetPrivateRequestDeniedReasonAsync(Meeting.Id);
        }
    }
}

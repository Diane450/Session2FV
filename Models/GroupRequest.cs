using Avalonia.Media.Imaging;
using Session2v2.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Session2v2.Models
{
    public class GroupRequest : Request
    {
        public override async Task DenyRequest()
        {
            await DBCall.DenyGroupRequestAsync();
        }

        public override async Task<DeniedReason> GetDeniedReason()
        {
            return await DBCall.GetGroupRequestDeniedReasonAsync(Meeting.Id);
        }
    }
}

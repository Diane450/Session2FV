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
            GroupDeniedRequest groupDeniedRequest = new GroupDeniedRequest
            {
                GroupRequestId = Meeting.Id,
                DeniedReasonId = Meeting.DeniedReason!.Id,
                CreationDate = Meeting.DateFrom,
            };
            await DBCall.DenyGroupRequestAsync(groupDeniedRequest);
        }

        public override async Task<DeniedReason> GetDeniedReason()
        {
            return await DBCall.GetGroupRequestDeniedReasonAsync(Meeting.Id);
        }

        public override async Task<TimeOnly> GetTimeAsync()
        {
            return await DBCall.GetGroupRequestVisitTimeAsync(Meeting.Id);
        }

        public override async Task<DateOnly> GetVisitDateAsync()
        {
            return await DBCall.GetGroupRequestVisitDateAsync(Meeting.Id);
        }

        public override async Task AcceptRequestAsync()
        {
            AcceptedGroupRequest acceptedGroupRequest = new AcceptedGroupRequest
            {
                GroupRequestId = Meeting.Id,
                DateVisit = (DateOnly)Meeting.DateVisit,
                Time = (TimeOnly)Meeting.Time,
                CreationDate = Meeting.DateTo
            };
            await DBCall.AcceptGroupRequest(acceptedGroupRequest);
        }
    }
}
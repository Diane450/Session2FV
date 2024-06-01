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
        public async override Task<DeniedReason> GetDeniedReason()
        {
            return await DBCall.GetPrivateRequestDeniedReasonAsync(Meeting.Id);
        }


        public async override Task DenyRequest()
        {
            PrivateDeniedRequest privateDeniedRequest = new PrivateDeniedRequest
            {
                PrivateRequestId = Meeting.Id,
                DeniedReasonId = Meeting.DeniedReason!.Id,
                CreationDate = Meeting.DateFrom,
                ClientEmail = Guest.Email
            };
            await DBCall.DenyPrivateRequestAsync(privateDeniedRequest);
        }

        public override async Task<DateOnly> GetVisitDateAsync()
        {
            return await DBCall.GetPivateRequestVisitDateAsync(Meeting.Id);
        }

        public override async Task<TimeOnly> GetTimeAsync()
        {
            return await DBCall.GetPivateRequestVisitTimeAsync(Meeting.Id);
        }

        public override async Task AcceptRequestAsync()
        {
            AcceptedPrivateRequest acceptedPrivateRequest = new AcceptedPrivateRequest
            {
                PrivateRequestId = Meeting.Id,
                Time = (TimeOnly)Meeting.Time,
                DateVisit = (DateOnly)Meeting.DateVisit,
                ClientEmail = Guest.Email,
                CreationDate = Meeting.DateFrom
            };
            await DBCall.AcceptPrivateRequest(acceptedPrivateRequest);
        }
    }
}

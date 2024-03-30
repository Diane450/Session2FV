using Avalonia.Controls;
using ReactiveUI;
using Session2v2.Models;
using Session2v2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session2v2.ViewModels
{
    public class RequestWindowViewModel : ViewModelBase
    {

        public Request SelectedRequest { get; set; }

        private Status _selectedStatus;

        public Status SelectedStatus
        {
            get { return _selectedStatus; }
            set { _selectedStatus = this.RaiseAndSetIfChanged(ref _selectedStatus, value); }
        }

        private DateTime _selectedDate;

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { _selectedDate = this.RaiseAndSetIfChanged(ref _selectedDate, value); }
        }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        private TimeSpan _selectedTime;

        public TimeSpan SelectedTime
        {
            get { return _selectedTime; }
            set { _selectedTime = this.RaiseAndSetIfChanged(ref _selectedTime, value); }
        }

        public static ObservableCollection<Status> StatusesList { get; set; } = null!;

        public ObservableCollection<DeniedReason> DeniedReasonsList { get; set; } = null!;

        private DeniedReason _selectedDeniedReason;

        public DeniedReason SelectedDeniedReason
        {
            get { return _selectedDeniedReason; }
            set { _selectedDeniedReason = this.RaiseAndSetIfChanged(ref _selectedDeniedReason, value); }
        }

        private bool _isRegularPermissionShown;

        public bool IsRegularPermissionShown
        {
            get { return _isRegularPermissionShown; }
            set { _isRegularPermissionShown = this.RaiseAndSetIfChanged(ref _isRegularPermissionShown, value); }
        }



        private RequestWindowViewModel(Request selectedRequest)
        {
            SelectedRequest = selectedRequest;
        }

        public static async Task<RequestWindowViewModel> CreateAsync(Request request)
        {
            try
            {
                var instance = new RequestWindowViewModel(request);
                await instance.GetContent();
                return instance;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private async Task GetContent()
        {
            await GetListData();

            await SetPermissions();
            //if (SelectedRequest.Meeting.Status.Id == 3)
            //    await DeniedRequestPermissions();
            //else if (await DBCall.IsGuestBlackListedAsync(SelectedRequest.Guest.Id))
            //    BlackListPermissions();
            //else
            //SetPemissions();

            //DeniedReasonsList = await DBCall.GetDeniedReasonsAsync();

            //StatusesList = await DBCall.GetStatusesAsync();
            //StatusesList.RemoveAt(0);
            //SelectedStatus = SelectedRequest.Meeting.Status;
        }

        private async Task GetListData()
        {
            if (StatusesList == null && DeniedReasonsList == null)
            {
                var statusTask = DBCall.GetStatusesAsync();
                var reasonTask = DBCall.GetDeniedReasonsAsync();
                await Task.WhenAll(statusTask, reasonTask);

                StatusesList = await statusTask;
                DeniedReasonsList = await reasonTask;
            }
        }

        private async Task SetPermissions()
        {
            bool a = IsRegularPermissionShown;
            //if (SelectedRequest.Meeting.Id == 3)
            //    await DeniedRequestPermissions();
            //else if (SelectedRequest.Guest.IsBlackListed)
            //    await BlackListGuestPermissions();
            //else
                await SetRegularPermissions();
        }
        private async Task SetRegularPermissions()
        {
            try
            {
                SelectedStatus = StatusesList[0];
                IsRegularPermissionShown = true;
                SelectedDate = DateTime.Parse(SelectedRequest.Meeting.DateFrom.ToString());
                DateStart = DateTime.Parse(SelectedRequest.Meeting.DateFrom.ToString());
                DateEnd = DateTime.Parse(SelectedRequest.Meeting.DateTo.ToString());

            }
            catch (Exception ex)
            {
                throw;
            }
        }


















        private async Task DeniedRequestPermissions()
        {
            try
            {
                SelectedStatus = SelectedRequest.Meeting.Status;
                await SelectedRequest.GetDeniedReason();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void BlackListPermissions()
        {

        }
    }
}

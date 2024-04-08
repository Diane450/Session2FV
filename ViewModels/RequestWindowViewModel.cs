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
            set
            {
                _selectedStatus = this.RaiseAndSetIfChanged(ref _selectedStatus, value);
                if (SelectedStatus.Id == StatusesList[0].Id)
                    IsRegularPermissionShown = true;
                else
                {
                    IsRegularPermissionShown = false;
                    SelectedDeniedReason = DeniedReasonsList[1];
                }
            }
        }

        private DateTime _selectedDate;

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { _selectedDate = this.RaiseAndSetIfChanged(ref _selectedDate, value); }
        }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        private TimeSpan _selectedTime = new TimeSpan(8, 0, 0);

        public TimeSpan SelectedTime
        {
            get { return _selectedTime; }
            set { _selectedTime = this.RaiseAndSetIfChanged(ref _selectedTime, value); }
        }

        public static ObservableCollection<Status> StatusesList { get; set; } = null!;

        public static ObservableCollection<DeniedReason> DeniedReasonsList { get; set; } = null!;

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

        private bool _isChangesEnable = true;

        public bool IsChangesEnable
        {
            get { return _isChangesEnable; }
            set { _isChangesEnable = this.RaiseAndSetIfChanged(ref _isChangesEnable, value); }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = this.RaiseAndSetIfChanged(ref _message, value); }
        }

        private bool _isSaveChangesEnabled;

        public bool IsSaveChangesEnabled
        {
            get { return _isSaveChangesEnabled; }
            set { _isSaveChangesEnabled = this.RaiseAndSetIfChanged(ref _isSaveChangesEnabled, value); ; }
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
            try
            {
                await GetListData();

                await SetPermissions();
            }
            catch (Exception)
            {
                Message = "Ошибка соединения";
            }
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
                StatusesList.RemoveAt(0);
            }
        }

        private async Task SetPermissions()
        {
            if (SelectedRequest.Meeting.Status.Id == 3)
                await DeniedRequestPermissions();
            else if (SelectedRequest.Meeting.Status.Id == 2)
                await AcceptedRequestPermissions();
            else if (await SelectedRequest.Guest.IsBlackListed())
                await BlackListGuestPermissions();
            else
                SetRegularPermissions();
        }

        private async Task AcceptedRequestPermissions()
        {
            if (SelectedRequest.Meeting.DateVisit == null && SelectedRequest.Meeting.Time == null)
            {
                var dateTask = SelectedRequest.GetVisitDateAsync();
                var timeTask = SelectedRequest.GetTimeAsync();
                await Task.WhenAll(dateTask, timeTask);
                SelectedRequest.Meeting.DateVisit = await dateTask;
                SelectedRequest.Meeting.Time = await timeTask;
            }
            SelectedStatus = StatusesList.First(s => s.Id == SelectedRequest.Meeting.Status.Id);
            SelectedTime = TimeSpan.Parse(SelectedRequest.Meeting.Time.ToString());
            SelectedDate = DateTime.Parse(SelectedRequest.Meeting.DateVisit.ToString());
            IsRegularPermissionShown = true;
            IsChangesEnable = false;
            Message = "Заявка уже одобрена";
        }

        private void SetRegularPermissions()
        {
            SelectedStatus = StatusesList[0];
            IsRegularPermissionShown = true;
            IsChangesEnable = true;
            SelectedDate = DateTime.Parse(SelectedRequest.Meeting.DateFrom.ToString());
            DateStart = DateTime.Parse(SelectedRequest.Meeting.DateFrom.ToString());
            DateEnd = DateTime.Parse(SelectedRequest.Meeting.DateTo.ToString());
        }

        private async Task DeniedRequestPermissions()
        {
            try
            {
                if (SelectedRequest.Meeting.DeniedReason == null)
                    SelectedRequest.Meeting.DeniedReason = await SelectedRequest.GetDeniedReason();
                SelectedDeniedReason = DeniedReasonsList.First(r => r.Id == SelectedRequest.Meeting.DeniedReason.Id);
                SetDeniedStatus();
                Message = "Заявка уже отклонена";
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task BlackListGuestPermissions()
        {
            SelectedDeniedReason = DeniedReasonsList[0];
            SelectedRequest.Meeting.DeniedReason = SelectedDeniedReason;
            
            SetDeniedStatus();

            await SelectedRequest.DenyRequest();
            Message = "Пользователь в черном списке. Заявка отклонена";
        }

        private void SetDeniedStatus()
        {
            IsRegularPermissionShown = false;
            SelectedStatus = StatusesList[2];
            SelectedRequest.Meeting.Status = SelectedStatus;
            IsChangesEnable = false;
        }
        
        public async Task SaveChanges()
        {

            try
            {
                if (SelectedStatus.Id == StatusesList[0].Id)
                {
                    SelectedRequest.Meeting.Status = SelectedStatus;
                    SelectedRequest.Meeting.Time = new TimeOnly(SelectedTime.Hours, SelectedTime.Minutes);
                    SelectedRequest.Meeting.DateVisit = new DateOnly(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day);
                    await SelectedRequest.AcceptRequestAsync();
                }
                else
                {
                    SelectedRequest.Meeting.Status = SelectedStatus;
                    SelectedRequest.Meeting.DeniedReason = SelectedDeniedReason;
                    await SelectedRequest.DenyRequest();
                }
                Message = "Изменения сохранены";
                IsChangesEnable = false;
            }
            catch (Exception)
            {
                Message = "Ошибка соединения";
            }
        }
    }
}

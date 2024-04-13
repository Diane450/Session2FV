using DynamicData;
using ReactiveUI;
using Session2v2.Models;
using Session2v2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session2v2.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private List<Request> requests = null!;

        private ObservableCollection<Request> _filteredRequests = null!;

        public ObservableCollection<Request> FilteredRequests
        {
            get { return _filteredRequests; }
            set { _filteredRequests = this.RaiseAndSetIfChanged(ref _filteredRequests, value); }
        }

        private Request _selectedRequest = null!;

        public Request SelectedRequest
        {
            get { return _selectedRequest; }
            set
            {
                _selectedRequest = this.RaiseAndSetIfChanged(ref _selectedRequest, value);
                if (SelectedRequest != null && SelectedRequest.Guest.AvatarBytes != null)
                {
                    SelectedRequest.ConvertAvatarByteToBitmap();
                    IsAvatarEqualsNull = false;
                }

                else
                    IsAvatarEqualsNull = true;
            }
        }

        private ObservableCollection<Department> _departmantList = null!;

        public ObservableCollection<Department> DepartmentList
        {
            get { return _departmantList; }
            set { _departmantList = this.RaiseAndSetIfChanged(ref _departmantList, value); }
        }

        private ObservableCollection<Status> _statusesList = null!;

        public ObservableCollection<Status> StatusesList
        {
            get { return _statusesList; }
            set { _statusesList = this.RaiseAndSetIfChanged(ref _statusesList, value); }
        }

        private ObservableCollection<MeetingType> _typeList = null!;

        public ObservableCollection<MeetingType> TypeList
        {
            get { return _typeList; }
            set { _typeList = this.RaiseAndSetIfChanged(ref _typeList, value); }
        }

        private Department _selectedDepartment = null!;

        public Department SelectedDepartment
        {
            get { return _selectedDepartment; }
            set { _selectedDepartment = this.RaiseAndSetIfChanged(ref _selectedDepartment, value); Filter(); }
        }

        private Status _selectedStatus = null!;

        public Status SelectedStatus
        {
            get { return _selectedStatus; }
            set { _selectedStatus = this.RaiseAndSetIfChanged(ref _selectedStatus, value); Filter(); }
        }

        private MeetingType _selectedType = null!;

        public MeetingType SelectedType
        {
            get { return _selectedType; }
            set { _selectedType = this.RaiseAndSetIfChanged(ref _selectedType, value); Filter(); }
        }

        private string _errorMessage = null!;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = this.RaiseAndSetIfChanged(ref _errorMessage, value); }
        }

        private bool _isAvatarEqualsNull;

        public bool IsAvatarEqualsNull
        {
            get { return _isAvatarEqualsNull; }
            set { _isAvatarEqualsNull = this.RaiseAndSetIfChanged(ref _isAvatarEqualsNull, value); }
        }

        private string? _passportNumber;

        public string? PassportNumber
        {
            get { return _passportNumber; }
            set { _passportNumber = this.RaiseAndSetIfChanged(ref _passportNumber, value); }
        }

        private bool _isPassportSearchEnable;

        public bool IsPassportSearchEnable
        {
            get { return _isPassportSearchEnable; }
            set { _isPassportSearchEnable = this.RaiseAndSetIfChanged(ref _isPassportSearchEnable, value); }
        }

        public MainWindowViewModel()
        {
            GetContentAsync();
            this.WhenAnyValue(x => x.PassportNumber).Subscribe(_ => PassportSearchEnable());
        }

        private void PassportSearchEnable()
        {
            IsPassportSearchEnable = PassportNumber?.Length == 6;
        }
        /// <summary>
        /// Возвращает содержимое для списков заявок, отделений, типов заявок и статусов
        /// </summary>
        /// <returns></returns>
        private async Task GetContentAsync()
        {
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                await GetListsContentAsync();
                SetSelectedValues();
                sw.Stop();
            }
            catch
            {
                ErrorMessage = "Ошибка соединения";
            }
        }

        /// <summary>
        /// Фильтрует список заявок по отделу, типу и статусу
        /// </summary>
        private void Filter()
        {
            var filteredList = new List<Request>(requests);

            if (SelectedType != TypeList[0] && SelectedType != null)
            {
                filteredList = filteredList.Where(r => r.Meeting.MeetingType.Id == SelectedType.Id).ToList();
            }

            if (SelectedDepartment != null)
            {
                filteredList = filteredList.Where(r => r.Meeting.Department.Id == SelectedDepartment.Id).ToList();
            }

            if (SelectedStatus != null)
            {
                filteredList = filteredList.Where(r => r.Meeting.Status.Id == SelectedStatus.Id).ToList();
            }

            FilteredRequests.Clear();
            FilteredRequests.AddRange(filteredList);
            if (FilteredRequests.Count != 0)
            {
                ErrorMessage = "";
                SelectedRequest = FilteredRequests[0];
            }
            else
            {
                ErrorMessage = "Нет заявок по выбранным категориям";
            }
        }
        public void FindByPassportNumber()
        {
            if (PassportNumber != null)
            {
                Request? request = FilteredRequests.FirstOrDefault(r => r.Guest.PassportNumber == PassportNumber);
                if (request == null)
                    ErrorMessage = "Гостей с таким номером паспорта не найдено";
                else
                    SelectedRequest = request;
            }
            else
            {
                Filter();
            }

        }
        private async Task GetListsContentAsync()
        {
            var requestsTask = DBCall.GetAllRequestsAsync();
            var departmentTask = DBCall.GetDepartmentsAsync();
            var statusTask = DBCall.GetStatusesAsync();
            var typeTask = DBCall.GetMeetingTypesAsync();

            await Task.WhenAll(requestsTask, departmentTask, statusTask, typeTask);

            requests = await requestsTask;
            FilteredRequests = new ObservableCollection<Request>(requests);
            DepartmentList = await departmentTask;
            StatusesList = await statusTask;
            TypeList = await typeTask;
        }
        private void SetSelectedValues()
        {
            SelectedRequest = FilteredRequests[0];
            SelectedDepartment = DepartmentList[0];
            SelectedStatus = StatusesList[0];
            SelectedType = TypeList[0];
        }
    }
}

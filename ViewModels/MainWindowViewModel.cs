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
    public class MainWindowViewModel:ViewModelBase
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
            set { _selectedRequest = this.RaiseAndSetIfChanged(ref _selectedRequest, value); SelectedRequest?.ConvertAvatarByteToBitmap(); }
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


        public MainWindowViewModel()
        {
            GetContentAsync();
        }

        /// <summary>
        /// Возвращает содержимое для списков заявок, отделений, типов заявок и статусов
        /// </summary>
        /// <returns></returns>
        private async Task GetContentAsync()
        {
            try
            {
                await GetListsContentAsync();
                SetSelectedValues();
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

            Request r = filteredList.Where(q => q.Meeting.Id == 65).First();

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

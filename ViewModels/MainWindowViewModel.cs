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
    public class MainWindowViewModel:ViewModelBase
    {
        private List<Request> requests;

        private List<Request> _filteredRequests;

        public List<Request> FilteredRequests
        {
            get { return _filteredRequests; }
            set { _filteredRequests = this.RaiseAndSetIfChanged(ref _filteredRequests, value); }
        }

        private Request _selectedRequest;

        public Request SelectedRequest
        {
            get { return _selectedRequest; }
            set { _selectedRequest = this.RaiseAndSetIfChanged(ref _selectedRequest, value); }
        }

        private ObservableCollection<string> _departmantList;

        public ObservableCollection<string> DepartmentList
        {
            get { return _departmantList; }
            set { _departmantList = this.RaiseAndSetIfChanged(ref _departmantList, value); }
        }

        private ObservableCollection<string> _statusesList;

        public ObservableCollection<string> StatusesList
        {
            get { return _statusesList; }
            set { _statusesList = this.RaiseAndSetIfChanged(ref _statusesList, value); }
        }

        private ObservableCollection<string> _typeList;

        public ObservableCollection<string> TypeList
        {
            get { return _typeList; }
            set { _typeList = this.RaiseAndSetIfChanged(ref _typeList, value); }
        }

        private string _selectedDepartment;

        public string SelectedDepartment
        {
            get { return _selectedDepartment; }
            set { _selectedDepartment = this.RaiseAndSetIfChanged(ref _selectedDepartment, value); }
        }

        private string _selectedStatus;

        public string SelectedStatus
        {
            get { return _selectedStatus; }
            set { _selectedStatus = this.RaiseAndSetIfChanged(ref _selectedStatus, value); }
        }

        private string _selectedType;

        public string SelectedType
        {
            get { return _selectedType; }
            set { _selectedType = this.RaiseAndSetIfChanged(ref _selectedType, value);; }
        }

        private string _errorMessage;

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
                FilteredRequests = await DBCall.GetAllRequestsAsync();
                DepartmentList = await DBCall.GetDepartmentsAsync();
                StatusesList = await DBCall.GetStatusesAsync();
                TypeList = await DBCall.GetMeetingTypesAsync();

                SelectedRequest = FilteredRequests[0];
                SelectedDepartment = DepartmentList[0];
                SelectedStatus = StatusesList[0];
                SelectedType = TypeList[0];
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Фильтрует список заявок по отделу, типу и статусу
        /// </summary>
        private void Filter()
        {

        }
    }
}

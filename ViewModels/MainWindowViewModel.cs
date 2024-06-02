using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Styling;
using DynamicData;
using ReactiveUI;
using Session2v2.Models;
using Session2v2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
                }
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

        private string _message = null!;

        public string Message
        {
            get { return _message; }
            set { _message = this.RaiseAndSetIfChanged(ref _message, value); }
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

        private bool _isDataLoaded = false;

        public bool IsDataLoaded
        {
            get { return _isDataLoaded; }
            set { _isDataLoaded = this.RaiseAndSetIfChanged(ref _isDataLoaded, value); }
        }

        private bool _isFilteredListNotNull = false;

        public bool IsFilteredListNotNull
        {
            get { return _isFilteredListNotNull; }
            set { _isFilteredListNotNull = this.RaiseAndSetIfChanged(ref _isFilteredListNotNull, value); }
        }


        private string _loadingText;

        public string LoadingText
        {
            get { return _loadingText; }
            set { _loadingText = this.RaiseAndSetIfChanged(ref _loadingText, value); }
        }

        private bool _isDataLoadedSuccess;

        public bool IsDataLoadedSuccess
        {
            get { return _isDataLoadedSuccess; }
            set { _isDataLoadedSuccess = this.RaiseAndSetIfChanged(ref _isDataLoadedSuccess, value); }
        }

        private bool _isDataLoading;

        public bool IsDataLoading
        {
            get { return _isDataLoading; }
            set { _isDataLoading = this.RaiseAndSetIfChanged(ref _isDataLoading, value); }
        }

        private Bitmap _changeThemeButtonIcon;

        public Bitmap ChangeThemeButtonIcon
        {
            get { return _changeThemeButtonIcon; }
            set { _changeThemeButtonIcon = this.RaiseAndSetIfChanged(ref _changeThemeButtonIcon, value); }
        }

        private bool _isAdmin = false;

        public bool IsAdmin
        {
            get { return _isAdmin; }
            set { _isAdmin = this.RaiseAndSetIfChanged(ref _isAdmin, value); }
        }

        private ObservableCollection<Employee> _employees;

        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set { _employees = this.RaiseAndSetIfChanged(ref _employees, value); }
        }

        private Employee _selectedEmployee;

        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = this.RaiseAndSetIfChanged(ref _selectedEmployee, value);
                if (SelectedEmployee != null && SelectedEmployee.Photo != null)
                {
                    SelectedEmployee.ConvertAvatarByteToBitmap();
                }

            }
        }

        private ObservableCollection<Employee> _filteredEmployees;

        public ObservableCollection<Employee> FilteredEmployees
        {
            get { return _filteredEmployees; }
            set { _filteredEmployees = this.RaiseAndSetIfChanged(ref _filteredEmployees, value); }
        }

        private string? _searchEmployee;

        public string? SearchEmployee
        {
            get { return _searchEmployee; }
            set
            {
                _searchEmployee = this.RaiseAndSetIfChanged(ref _searchEmployee, value);
                var list = Employees.Where(e => e.FullName.ToLower().StartsWith(SearchEmployee)).ToList();
                FilteredEmployees = new ObservableCollection<Employee>(list);
                if (FilteredEmployees.Count<1)
                {
                    IsEmployeeFound = false;
                    MessageEmployeeFound = "Совпадения не найдены";
                }
                else
                {
                    SelectedEmployee = FilteredEmployees[0];
                    IsEmployeeFound = true;
                    MessageEmployeeFound = "";
                }
            }
        }

        private bool _isEmployeeFound = true;

        public bool IsEmployeeFound
        {
            get { return _isEmployeeFound; }
            set { _isEmployeeFound = this.RaiseAndSetIfChanged(ref _isEmployeeFound, value); }
        }

        private string _messageEmployeeFound;

        public string MessageEmployeeFound
        {
            get { return _messageEmployeeFound; }
            set { _messageEmployeeFound = this.RaiseAndSetIfChanged(ref _messageEmployeeFound, value); }
        }

        public MainWindowViewModel()
        {
            this.WhenAnyValue(x => x.PassportNumber, x=>x.SearchEmployee).Subscribe(_ => PassportSearchEnable());

            ChangeThemeButtonIcon = new Bitmap(AssetLoader.Open(new Uri("avares://Session2v2/Assets/moon.png")));

            CreateAsync();
        }

        public async Task CreateAsync()
        {
            GetContentAsync();
            LoadText();
        }

        private async Task LoadText()
        {
            LoadingText = "Загрузка";
            int count = 0;
            while (!IsDataLoaded)
            {
                LoadingText += '.';
                await Task.Delay(1000);
                count++;
                if (count == 3)
                {
                    LoadingText = LoadingText.Remove(8, 3);
                    count = 0;
                }
            }
        }

        private void PassportSearchEnable()
        {
            IsPassportSearchEnable = PassportNumber?.Length == 6;
        }

        private async Task GetContentAsync()
        {
            try
            {
                IsDataLoadedSuccess = true;
                IsDataLoading = true;
                await GetListsContentAsync();
                SetSelectedValues();
                IsDataLoaded = true;
                IsDataLoading = false;
                IsFilteredListNotNull = true;
                if (CurrentUser.Employee.EmployeeUserType.Id == 1)
                {
                    IsAdmin = true;
                    Employees = new ObservableCollection<Employee>(await DBCall.GetEmployees());
                    FilteredEmployees = new ObservableCollection<Employee>(Employees);
                    SelectedEmployee = FilteredEmployees[0];
                    //TODO: get employee data.
                }
            }
            catch
            {
                IsDataLoading = false;
                IsDataLoaded = false;
                IsDataLoadedSuccess = false;
            }
        }

        public void ChangeTheme()
        {
            if (Application.Current.RequestedThemeVariant == ThemeVariant.Light || Application.Current.RequestedThemeVariant == null)
            {
                Application.Current.RequestedThemeVariant = ThemeVariant.Dark;
                ChangeThemeButtonIcon = new Bitmap(AssetLoader.Open(new Uri("avares://Session2v2/Assets/sun.png")));
            }
            else
            {
                Application.Current.RequestedThemeVariant = ThemeVariant.Light;
                ChangeThemeButtonIcon = new Bitmap(AssetLoader.Open(new Uri("avares://Session2v2/Assets/moon.png")));
            }
        }

        private void Filter()
        {
            if (SelectedType != null && SelectedDepartment != null && SelectedStatus != null)
            {

            }
            var filteredList = new List<Request>(requests);

            if (SelectedType != TypeList[0] && SelectedType != null)
            {
                filteredList = filteredList.Where(r => r.Meeting.MeetingType.Id == SelectedType.Id).ToList();
            }

            if (SelectedDepartment != null && SelectedDepartment != DepartmentList[0])
            {
                filteredList = filteredList.Where(r => r.Meeting.Department.Id == SelectedDepartment.Id).ToList();
            }

            if (SelectedStatus != null && SelectedStatus != StatusesList[0])
            {
                filteredList = filteredList.Where(r => r.Meeting.Status.Id == SelectedStatus.Id).ToList();
            }

            FilteredRequests.Clear();
            FilteredRequests.AddRange(filteredList);

            if (FilteredRequests.Any())
            {
                Message = "";
                IsFilteredListNotNull = true;
                SelectedRequest = FilteredRequests[0];
            }
            else
            {
                IsFilteredListNotNull = false;
                Message = "Нет заявок по выбранным категориям";
            }
        }

        public void FindByPassportNumber()
        {
            SelectedDepartment = DepartmentList[0];
            SelectedStatus = StatusesList[0];
            SelectedType = TypeList[0];

            if (PassportNumber != null)
            {
                Request? request = FilteredRequests.FirstOrDefault(r => r.Guest.PassportNumber == PassportNumber);
                if (request == null)
                {
                    Message = "Не найдено";
                }
                else
                    SelectedRequest = request;
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

            DepartmentList = new ObservableCollection<Department>()
            {
                new Department()
                {
                    Id = 0,
                    Name = "Все"
                },
                await departmentTask
            };

            StatusesList = new ObservableCollection<Status>()
            {
                new Status()
                {
                    Id = 0,
                    Name = "Все"
                },
                await statusTask
            };
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

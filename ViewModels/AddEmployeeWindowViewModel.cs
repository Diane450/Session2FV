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
    public class AddEmployeeWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel Model { get; set; }

        private Employee _employee;

        public Employee Employee
        {
            get { return _employee; }
            set { _employee = this.RaiseAndSetIfChanged(ref _employee, value); }
        }

        private ObservableCollection<EmployeeUserType> _types;

        public ObservableCollection<EmployeeUserType> Types
        {
            get { return _types; }
            set { _types = this.RaiseAndSetIfChanged(ref _types, value); }
        }

        private ObservableCollection<Department> _departments;

        public ObservableCollection<Department> Departments
        {
            get { return _departments; }
            set { _departments = this.RaiseAndSetIfChanged(ref _departments, value); }
        }

        private ObservableCollection<Subdepartment> _subdepartments;

        public ObservableCollection<Subdepartment> Subdepartments
        {
            get { return _subdepartments; }
            set { _subdepartments = this.RaiseAndSetIfChanged(ref _subdepartments, value); }
        }

        private EmployeeUserType _selectedType;

        public EmployeeUserType SelectedType
        {
            get { return _selectedType; }
            set { _selectedType = this.RaiseAndSetIfChanged(ref _selectedType, value); }
        }

        private Department? _selectedDepartment;

        public Department? SelectedDepartment
        {
            get { return _selectedDepartment; }
            set { _selectedDepartment = this.RaiseAndSetIfChanged(ref _selectedDepartment, value); }
        }

        private Subdepartment? _selectedSubdepartment;

        public Subdepartment? SelectedSubdepartment
        {
            get { return _selectedSubdepartment; }
            set { _selectedSubdepartment = this.RaiseAndSetIfChanged(ref _selectedSubdepartment, value); }
        }

        private string? _passportNumber;

        public string? PassportNumber
        {
            get { return _passportNumber; }
            set
            {
                if (int.TryParse(value, out int result) || value == "")
                    _passportNumber = this.RaiseAndSetIfChanged(ref _passportNumber, value);

            }
        }

        private string? _passportSeries;

        public string? PassportSeries
        {
            get { return _passportSeries; }
            set
            {
                if (int.TryParse(value, out int result) || value == "")
                    _passportSeries = this.RaiseAndSetIfChanged(ref _passportSeries, value);

            }
        }

        private string? _code;

        public string? Code
        {
            get { return _code; }
            set
            {
                if (int.TryParse(value, out int result) || value == "")
                    _code = this.RaiseAndSetIfChanged(ref _code, value);

            }
        }

        private bool _isButtonEnabled;

        public bool IsButtonEnabled
        {
            get { return _isButtonEnabled; }
            set { _isButtonEnabled = this.RaiseAndSetIfChanged(ref _isButtonEnabled, value); }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = this.RaiseAndSetIfChanged(ref _message, value); }
        }

        public List<string> Deps { get; set; } = new List<string>
        {
            "Отдел",
            "Подразделение"
        };

        private string _selectedDeps;

        public string SelectedDeps
        {
            get { return _selectedDeps; }
            set
            {
                _selectedDeps = this.RaiseAndSetIfChanged(ref _selectedDeps, value);
                if (_selectedDeps == Deps[0])
                {
                    IsDepartmentsSelected = true;
                    SelectedSubdepartment = null;
                    SelectedDepartment = Departments[0];
                }
                else
                {
                    IsDepartmentsSelected = false;
                    SelectedDepartment = null;
                    SelectedSubdepartment = Subdepartments[0];
                }
            }
        }

        private bool _isDepartmentsSelected = true;

        public bool IsDepartmentsSelected
        {
            get { return _isDepartmentsSelected; }
            set { _isDepartmentsSelected = this.RaiseAndSetIfChanged(ref _isDepartmentsSelected, value); }
        }



        public AddEmployeeWindowViewModel(MainWindowViewModel model)
        {
            Model = model;
            Employee = new Employee();
            GetContent();
            this.WhenAnyValue(x => x.Employee.FullName,
                x => x.PassportSeries,
                x => x.PassportNumber,
                x => x.Code,
                x => x.Employee.Photo)
            .Subscribe(_ => ButtonEnable());

        }

        private void ButtonEnable()
        {
            IsButtonEnabled = !string.IsNullOrEmpty(PassportNumber) &&
                !string.IsNullOrEmpty(PassportSeries) &&
                !string.IsNullOrEmpty(Employee.FullName) &&
                !string.IsNullOrEmpty(Code) &&
                Employee.Photo != null;
        }
        private async Task GetContent()
        {
            Types = await DBCall.GetUserTypes();
            Departments = await DBCall.GetDepartmentsAsync();
            Subdepartments = await DBCall.GetSubdepartment();

            SelectedType = Types[0];
            SelectedDepartment = Departments[0];
            SelectedSubdepartment = Subdepartments[0];
            SelectedDeps = Deps[0];
        }

        public async Task Add()
        {
            try
            {
                Employee.Code = int.Parse(Code!);
                Employee.PassportNumber = int.Parse(PassportNumber!);
                Employee.PassportSeries = int.Parse(PassportSeries!);
                Employee.Department = SelectedDepartment;
                Employee.Subdepartment = SelectedSubdepartment;
                Employee.EmployeeUserType = SelectedType;
                Message = await DBCall.AddNewEmployee(Employee);
                if (Message == null)
                {
                    Model.Employees.Add(Employee);
                    Message = "Сотрудник успешно добавлен";
                }
            }
            catch
            {
                Message = "Не удалось добавить сотрудника";
            }
        }
    }
}

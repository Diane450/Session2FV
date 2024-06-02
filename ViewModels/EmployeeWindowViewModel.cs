using Avalonia.Interactivity;
using ReactiveUI;
using Session2v2.Models;
using Session2v2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Session2v2.ViewModels
{
    public class EmployeeWindowViewModel : ViewModelBase
    {
        public Employee Employee { get; set; }

        private Employee _selectedEmployee;

        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { _selectedEmployee = this.RaiseAndSetIfChanged(ref _selectedEmployee, value); }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = this.RaiseAndSetIfChanged(ref _message, value); }
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

        private bool _isButtonEnabled;

        public bool IsButtonEnabled
        {
            get { return _isButtonEnabled; }
            set { _isButtonEnabled = this.RaiseAndSetIfChanged(ref _isButtonEnabled, value); }
        }

        public EmployeeWindowViewModel(Employee employee)
        {
            Employee = employee;

            SelectedEmployee = new Employee
            {
                IdEmployees = employee.IdEmployees,
                FullName = employee.FullName,
                Code = employee.Code,
                EmployeeUserType = employee.EmployeeUserType,
                PassportNumber = employee.PassportNumber,
                PassportSeries = employee.PassportSeries,
                Department = employee.Department,
                Subdepartment = employee.Subdepartment,
                Photo = employee.Photo,
                AvatarBitmap = employee.AvatarBitmap
            };
            PassportNumber = employee.PassportNumber.ToString();
            PassportSeries = employee.PassportSeries.ToString();

            this.WhenAnyValue(x => x.SelectedEmployee.FullName, x => x.PassportSeries, x => x.PassportNumber)
                .Subscribe(_ => ButtonEnable());
        }

        private void ButtonEnable()
        {
            IsButtonEnabled = !string.IsNullOrEmpty(PassportNumber) && !string.IsNullOrEmpty(PassportSeries) && !string.IsNullOrEmpty(SelectedEmployee.FullName);
        }
        public async Task SaveChanges()
        {
            try
            {
                SelectedEmployee.PassportNumber = int.Parse(PassportNumber!);
                SelectedEmployee.PassportSeries = int.Parse(PassportSeries!);

                await DBCall.SaveChangesEmployee(SelectedEmployee);

                Employee.FullName = SelectedEmployee.FullName;
                Employee.PassportNumber = SelectedEmployee.PassportNumber;
                Employee.PassportSeries = SelectedEmployee.PassportSeries;
                Employee.Photo = SelectedEmployee.Photo;
                Employee.AvatarBitmap = SelectedEmployee.AvatarBitmap;

                Message = "Успешно обновлено";
            }
            catch (Exception)
            {
                Message = "Не удалось сохранить изменения";
            }
        }
    }
}

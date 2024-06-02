using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Session2v2.Models
{

    public class Employee : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public int IdEmployees { get; set; }

        private string _fullName = null!;
        public string FullName
        {
            get => _fullName;
            set
            {
                if (_fullName != value)
                {
                    _fullName = value;
                    OnPropertyChanged();
                }
            }
        }

        private Department? _department;
        public Department? Department
        {
            get => _department;
            set
            {
                if (_department != value)
                {
                    _department = value;
                    OnPropertyChanged();
                }
            }
        }

        private Subdepartment? _subdepartment;
        public Subdepartment? Subdepartment
        {
            get => _subdepartment;
            set
            {
                if (_subdepartment != value)
                {
                    _subdepartment = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _code;
        public int Code
        {
            get => _code;
            set
            {
                if (_code != value)
                {
                    _code = value;
                    OnPropertyChanged();
                }
            }
        }

        private byte[]? _photo;
        public byte[]? Photo
        {
            get => _photo;
            set
            {
                if (_photo != value)
                {
                    _photo = value;
                    OnPropertyChanged();
                }
            }
        }

        private Bitmap _avatarBitmap = null!;

        public Bitmap AvatarBitmap
        {
            get { return _avatarBitmap; }
            set
            {
                _avatarBitmap = value;
                OnPropertyChanged();
            }
        }


        private int? _passportNumber;
        public int? PassportNumber
        {
            get => _passportNumber;
            set
            {
                if (_passportNumber != value)
                {
                    _passportNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private int? _passportSeries;
        public int? PassportSeries
        {
            get => _passportSeries;
            set
            {
                if (_passportSeries != value)
                {
                    _passportSeries = value;
                    OnPropertyChanged();
                }
            }
        }

        private EmployeeUserType _employeeUserType = null!;
        public EmployeeUserType EmployeeUserType
        {
            get => _employeeUserType;
            set
            {
                if (_employeeUserType != value)
                {
                    _employeeUserType = value;
                    OnPropertyChanged();
                }
            }
        }

        public void ConvertAvatarByteToBitmap()
        {
            if (Photo != null && AvatarBitmap == null)
            {
                MemoryStream ms = new(Photo);
                AvatarBitmap = new Bitmap(ms);
            }
        }
    }
}

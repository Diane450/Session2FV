using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tmds.DBus.SourceGenerator;

namespace Session2v2.Models
{
    public class Meeting: INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string DateTo { get; set; } = null!;
        public string DateFrom { get; set; } = null!;

        private string? _dateVisit;
        public string? DateVisit
        {
            get
            {
                return _dateVisit;
            }
            set
            {
                _dateVisit = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateVisit)));
            }
        }

        private string? _time;
        public string? Time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Time)));
            }
        }

        private string _status = null!;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
            }
        }

        public string VisitPurpose { get; set; } = null!;

        public string Department { get; set; } = null!;

        public string FullNameEmployee { get; set; } = null!;

        public event PropertyChangedEventHandler? PropertyChanged;

    }
}

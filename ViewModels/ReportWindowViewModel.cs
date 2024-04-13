using ReactiveUI;
using Session2v2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session2v2.ViewModels
{
    public class ReportWindowViewModel : ViewModelBase
    {

        public DateTime Today { get; set; } = DateTime.Now;

        private DateTime _selectedDateStart = DateTime.Now;

        public DateTime SelectedDateStart
        {
            get { return _selectedDateStart; }
            set { _selectedDateStart = this.RaiseAndSetIfChanged(ref _selectedDateStart, value); }
        }

        private DateTime _selectedDateEnd = DateTime.Now;

        public DateTime SelectedDateEnd
        {
            get { return _selectedDateEnd; }
            set { _selectedDateEnd = this.RaiseAndSetIfChanged(ref _selectedDateEnd, value); }
        }

        public ReportWindowViewModel()
        {

        }


        public async void CreateReport()
        {
            try
            {
                
                var task = await DBCall.GetReportData(new DateOnly[]
                {

                });
            }
            catch (Exception)
            {

            }
        }
    }
}

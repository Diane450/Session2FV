using ReactiveUI;
using Session2v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session2v2.ViewModels
{
    public class PassportWindowViewModel:ViewModelBase
    {
        private Request _request;

        public Request Request
        {
            get { return _request; }
            set { _request = this.RaiseAndSetIfChanged(ref _request, value); }
        }

        public PassportWindowViewModel(Request request)
        {
            Request = request;
            Request.ConvertPassportByteToBitmap();
        }
    }
}

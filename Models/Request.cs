using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Newtonsoft.Json;

namespace Session2v2.Models
{
    public class Request
    {
        public Guest Guest { get; set; } = null!;

        public Meeting Meeting { get; set; } = null!;
    }
}

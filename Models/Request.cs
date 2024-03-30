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

        public void ConvertAvatarByteToBitmap()
        {
            if (IsPhotoesEmptyOrNull(Guest.AvatarBytes))
            {
                MemoryStream ms = new(Guest.AvatarBytes);
                Guest.AvatarBitmap = new Bitmap(ms);
            }
        }

        public void ConvertPassportByteToBitmap()
        {
            if (IsPhotoesEmptyOrNull(Guest.PassportBytes))
            {
                MemoryStream ms = new(Guest.PassportBytes);
                Guest.PassportBitmap = new Bitmap(ms);
            }
        }

        private static bool IsPhotoesEmptyOrNull(byte[] photo)
        {
            return photo != null && photo.Length > 0;
        }

        public virtual async Task GetDeniedReason()
        {

        }
    }
}

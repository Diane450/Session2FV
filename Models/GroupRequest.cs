﻿using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session2v2.Models
{
    public class GroupRequest : Request
    {
        public Guest Guest { get; set; } = null!;
        public Meeting Meeting { get; set; } = null!;

        public static List<GroupRequest> ConvertByteToBitmap(List<GroupRequest> requests)
        {
            for (int i = 0; i < requests.Count; i++)
            {
                if (IsPhotoesEmptyOrNull(requests[i].Guest.AvatarBytes))
                {
                    MemoryStream ms = new MemoryStream(requests[i].Guest.AvatarBytes);
                    requests[i].Guest.AvatarBitmap = new Bitmap(ms);
                }
                if (IsPhotoesEmptyOrNull(requests[i].Guest.PassportBytes))
                {
                    MemoryStream ms = new MemoryStream(requests[i].Guest.PassportBytes);
                    requests[i].Guest.AvatarBitmap = new Bitmap(ms);
                }
            }
            return requests;
        }
        private static bool IsPhotoesEmptyOrNull(byte[] photo)
        {
            return photo != null && photo.Length > 0;
        }

        public List<Request> ConvertByteToBitmap(List<Request> requests)
        {
            throw new NotImplementedException();
        }
    }
}

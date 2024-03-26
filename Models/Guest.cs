using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tmds.DBus.SourceGenerator;

namespace Session2v2.Models
{
    /// <summary>
    /// Информация о госте в заявке
    /// </summary>
    public class Guest: INotifyPropertyChanged
    {
        public int Id { get; set; }

        public string LastName { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Patronymic { get; set; }

        public string? Phone { get; set; }

        public string Email { get; set; } = null!;

        public string? Organization { get; set; }

        public string Note { get; set; } = null!;

        public string Birthday { get; set; } = null!;

        public string PassportSeries { get; set; } = null!;

        public string PassportNumber { get; set; } = null!;

        public byte[]? AvatarBytes { get; set; }

        private Bitmap _avatarBitmap = null!;


        public Bitmap AvatarBitmap
        {
            get { return _avatarBitmap; }
            set { _avatarBitmap = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AvatarBitmap))); }
        }

        public byte[]? PassportBytes { get; set; }

        private Bitmap _passportBitmap = null!;
        public Bitmap PassportBitmap
        {
            get { return _passportBitmap; }
            set { _passportBitmap = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PassportBitmap))); }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

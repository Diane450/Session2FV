using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Session2v2.Models;
using Session2v2.ViewModels;
using System.IO;

namespace Session2v2;

public partial class EmployeeWindow : Window
{
    public EmployeeWindow(Employee employee)
    {
        InitializeComponent();
        DataContext = new EmployeeWindowViewModel(employee);
    }

    private async void ChangePhoto(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var context = (EmployeeWindowViewModel)button.DataContext;

        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Filters.Add(new FileDialogFilter() { Name = "Images", Extensions = { "jpg", "png", "jpeg" } });

        string[] result = await dialog.ShowAsync(this);

        if (result != null && result.Length > 0)
        {
            using (FileStream fs = File.OpenRead(result[0]))
            {
                Avalonia.Media.Imaging.Bitmap bp = new Avalonia.Media.Imaging.Bitmap(fs);

                using (MemoryStream ms = new MemoryStream())
                {
                    bp.Save(ms);
                    context!.SelectedEmployee.Photo = ms.ToArray();
                    context!.SelectedEmployee.AvatarBitmap = bp;
                }
            }
        }
    }
}
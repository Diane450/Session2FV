using Avalonia.Controls;
using Avalonia.Interactivity;
using Session2v2.ViewModels;
using System.Security.Cryptography;

namespace Session2v2.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
        private async void Change(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var dataContext = (MainWindowViewModel)button.DataContext;
            var selectedRequest = dataContext.SelectedRequest;
            var window = await RequestWindow.CreateAsync(selectedRequest);
            window.ShowDialog(this);
        }

        private void OpenReportWindow(object sender, RoutedEventArgs e)
        {
            var window = new ReportWindow();
            window.ShowDialog(this);
        }


    }
}

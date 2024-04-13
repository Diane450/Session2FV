using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Session2v2.Models;
using Session2v2.ViewModels;
using System.Threading.Tasks;

namespace Session2v2;

public partial class RequestWindow : Window
{
    public RequestWindow()
    {
        InitializeComponent();
    }
    public static async Task <RequestWindow> CreateAsync(Request selectedRequest)
    {
        RequestWindowViewModel Context = await RequestWindowViewModel.CreateAsync(selectedRequest);
        RequestWindow requestWindow = new RequestWindow
        {
            DataContext = Context
        };
        return requestWindow;
    }
    public void ViewPassport(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var dataContext = (RequestWindowViewModel)button.DataContext;
        var selectedRequest = dataContext.SelectedRequest;
        var window = new PassportWindow(selectedRequest);
        window.ShowDialog(this);
    }
} 
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Session2v2.Models;
using Session2v2.ViewModels;
using System.Threading.Tasks;

namespace Session2v2;

public partial class RequestWindow : Window
{
    private RequestWindow(RequestWindowViewModel model)
    {
        InitializeComponent();
        DataContext = model;
    }
    public static async Task <RequestWindow> CreateAsync(Request selectedRequest)
    {
        RequestWindowViewModel Context = await RequestWindowViewModel.CreateAsync(selectedRequest);
        return new RequestWindow(Context);
    }
} 
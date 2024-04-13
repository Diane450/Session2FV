using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Session2v2.Models;
using Session2v2.ViewModels;

namespace Session2v2;

public partial class PassportWindow : Window
{
    public PassportWindow(Request request)
    {
        InitializeComponent();
        DataContext = new PassportWindowViewModel(request);
    }
}
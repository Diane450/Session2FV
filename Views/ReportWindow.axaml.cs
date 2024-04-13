using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Session2v2.ViewModels;

namespace Session2v2;

public partial class ReportWindow : Window
{
    public ReportWindow()
    {
        InitializeComponent();
        DataContext = new ReportWindowViewModel();
    }
}
using Avalonia.Controls;
using Session2v2.ViewModels;

namespace Session2v2.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}

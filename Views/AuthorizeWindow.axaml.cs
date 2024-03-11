using Avalonia.Controls;
using Avalonia.Interactivity;
using Session2v2.Services;
using System;

namespace Session2v2.Views;

public partial class AuthorizeWindow : Window
{
    public AuthorizeWindow()
    {
        InitializeComponent();
    }

    private async void Authorize(object sender, RoutedEventArgs e)
    {
        try
        {
            TextBox textBox = this.Find<TextBox>("Code")!;
            if (await DBCall.AuthorizeAsync(textBox.Text!))
            {
                var window = new MainWindow();
                window.Show();
                Close();
            }
            else
            {
                Label ErrorLabel = this.Find<Label>("ErrorLabel");
                ErrorLabel.IsVisible = true;
                ErrorLabel.Content = "Неправильный код";
            }
        }
        catch(Exception ex)
        {
            Label ErrorLabel = this.Find<Label>("ErrorLabel");
            ErrorLabel.IsVisible = true;
            ErrorLabel.Content = "Ошибка соединения";
        }
    }
}
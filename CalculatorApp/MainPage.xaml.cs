using CalculatorApp.ViewModels;

namespace CalculatorApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new CalculatorViewModel();
    }
}

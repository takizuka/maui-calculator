using Microsoft.Extensions.DependencyInjection;

namespace CalculatorApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var window = new Window(new AppShell());
		window.MinimumWidth = 350;
		window.MinimumHeight = 550;
		return window;
	}
}
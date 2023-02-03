using Base.Views;

namespace Base;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(ScannedList), typeof(ScannedList));
	}
}

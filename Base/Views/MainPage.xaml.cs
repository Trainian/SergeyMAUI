namespace Base.Views;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Кликов: {count}";
		else if(count > 1 && count <= 10)
			CounterBtn.Text = $"Кликов: {count}, хватит?";
        else if (count > 10)
            CounterBtn.Text = $"Кликов: {count}, ПРЕКРАТИ... мне щикотно !";
	}
}


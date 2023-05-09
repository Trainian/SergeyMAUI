namespace Base.CustomControls;

public partial class LoadControl : StackLayout
{
	public LoadControl()
	{
		InitializeComponent();
	}

	#region Indicator Color
    public static readonly BindableProperty IndicatorColorProperty = BindableProperty.Create
		(
			 propertyName: nameof(IndicatorColor),
			 returnType: typeof(Color),
			 declaringType: typeof(LoadControl),
			 defaultValue: Colors.Black,
			 defaultBindingMode: BindingMode.OneWay
		);
	public Color IndicatorColor 
	{ 
		get => (Color)GetValue(IndicatorColorProperty);
		set => SetValue(IndicatorColorProperty, value);
	}
	#endregion

	#region Indicator Text
	public static readonly BindableProperty IndicatorTextProperty = BindableProperty.Create
		(
			propertyName: nameof(IndicatorText),
			returnType: typeof(String),
			declaringType: typeof(LoadControl),
			defaultValue: "Загрузка...Подождите...",
			defaultBindingMode: BindingMode.OneWay
		);
	public string IndicatorText 
	{ 
		get => (string)GetValue(IndicatorTextProperty); 
		set => SetValue(IndicatorTextProperty, value);
	}
	#endregion
}
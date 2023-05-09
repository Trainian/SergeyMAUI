using CommunityToolkit.Maui.Views;

namespace Base.CustomControls;

public partial class MessagePopup : Popup
{
	Timer _timer = null;
	public MessagePopup(string message,int timer)
	{
		InitializeComponent();
		_timer = new Timer(ClosePopup, null, timer, 0);
	}

    private void ClosePopup(object state)
    {
		Close();
    }
}
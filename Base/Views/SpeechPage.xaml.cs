using Base.Interfaces;
using Base.Platforms;
using Base.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui;

namespace Base.Views;

public partial class SpeechPage : ContentPage
{
    private ISpeechToText _speechRecongnitionInstance;
    private SpeechPageViewModel _speechModel { get; set; }

    [Obsolete]
    public SpeechPage(ISpeechToText speechToText)
	{
		InitializeComponent();
        _speechModel = Resources["speechText"] as SpeechPageViewModel;

        try
        {
            _speechRecongnitionInstance = speechToText;
        }
        catch (Exception ex)
        {
            _speechModel.SpeechText = ex.Message;
        }

        MessagingCenter.Subscribe<ISpeechToText, string>(this, "STT", (sender, args) =>
        {
            SpeechToTextFinalResultRecieved(args);
        });

        MessagingCenter.Subscribe<ISpeechToText>(this, "Final", (sender) =>
        {
            start.IsEnabled = true;
        });

        MessagingCenter.Subscribe<IMessageSender, string>(this, "STT", (sender, args) =>
        {
            SpeechToTextFinalResultRecieved(args);
        });
    }

    private void SpeechToTextFinalResultRecieved(string args)
    {
        _speechModel.SpeechText = args;
    }

    private void Start_Clicked(object sender, EventArgs e)
    {
        try
        {
            _speechRecongnitionInstance.StartSpeechToText();
        }
        catch (Exception ex)
        {
            _speechModel.SpeechText = ex.Message;
        }

        if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            start.IsEnabled = false;
        }
    }
}
using Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Views;
using Android.Widget;
using Base.Platforms;


namespace Base.Platforms
{
    public class SpeechToTextService : ISpeechToText
    {
        private readonly int VOICE = 10;
        private Activity _activity;
        public SpeechToTextService()
        {
            _activity = Platform.CurrentActivity;
        }
        public void StartSpeechToText()
        {
            StartRecordingAndRecognizing();
        }

        private void StartRecordingAndRecognizing()
        {
            string rec = global::Android.Content.PM.PackageManager.FeatureMicrophone;
            if (rec == "android.hardware.microphone")
            {
                try
                {
                    var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);


                    voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Говорите:");

                    voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
                    _activity.StartActivityForResult(voiceIntent, VOICE);
                }
                catch (ActivityNotFoundException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    String appPackageName = "com.google.android.googlequicksearchbox";
                    try
                    {
                        Intent intent = new Intent(Intent.ActionView, global::Android.Net.Uri.Parse("market://details?id=" + appPackageName));
                        _activity.StartActivityForResult(intent, VOICE);

                    }
                    catch (ActivityNotFoundException e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        Intent intent = new Intent(Intent.ActionView, global::Android.Net.Uri.Parse("https://play.google.com/store/apps/details?id=" + appPackageName));
                        _activity.StartActivityForResult(intent, VOICE);
                    }
                }

            }
            else
            {
                throw new Exception("No mic found");
            }
        }

        public void StopSpeechToText()
        {
            
        }
    }
}

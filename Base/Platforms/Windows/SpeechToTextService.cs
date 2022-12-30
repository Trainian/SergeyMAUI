using Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Platforms;

[assembly: Dependency(typeof(SpeechToTextService))]
namespace Base.Platforms
{
    public class SpeechToTextService : ISpeechToText
    {
        public SpeechToTextService()
        {

        }
        public void StartSpeechToText()
        {
            StartRecordingAndRecognizing();
        }

        private void StartRecordingAndRecognizing()
        {

        }

        public void StopSpeechToText()
        {
            
        }
    }
}

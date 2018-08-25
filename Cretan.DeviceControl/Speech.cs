using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Cretan.DeviceControl
{
    public class Speech
    {
        public Speech()
        {

        }

        public async Task SpeakText(string text)
        {
            await TextToSpeech.SpeakAsync(text);
        }

        public async Task SpeakSentences(IEnumerable<string> sentences, double pauseBetweenInSec)
        {
            foreach(var sentence in sentences)
            {
                await TextToSpeech.SpeakAsync(sentence);
                await Task.Delay((int)(pauseBetweenInSec * 1000));
            }
        }
    }
}

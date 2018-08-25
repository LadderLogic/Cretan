using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Cretan.DeviceControl
{
    public class Haptic
    {
        public Haptic()
        {

        }

        public async Task Pulse(int onDuty, int offDuty, TimeSpan totalDuration, CancellationToken token)
        {
            await Task.Factory.StartNew(() =>
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                while (stopWatch.Elapsed < totalDuration)
                {
                    Vibration.Vibrate(onDuty);
                    Task.Delay(offDuty).Wait();
                }
            }, token).ContinueWith((tsk)=>Vibration.Cancel());
        }
    }
}

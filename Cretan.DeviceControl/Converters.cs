﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Cretan.DeviceControl
{
    public static class Converters
    {
        public static double MeterPerSecondToMilesPerHour(double metersPerSecond)
        {
            var kmpH = MeterPerSecondToKmph(metersPerSecond);
            return KmphToMph(kmpH);

        }

        public static double KmphToMph(double kmpH)
        {
            return 0.62137119 * kmpH;
        }

        public static double MeterPerSecondToKmph(double metersPerSecond)
        {
            var kmpH = metersPerSecond * (60 * 60) / 1000;
            return kmpH;
        }

        public static double MphToMinutesPerMile(double mph)
        {
            var minutesPerMile = 60.0/mph;
            return minutesPerMile;
        }

        public static double MinutesPerMileToMph(double minutesPerMile)
        {
            var mph = 60.0 / minutesPerMile ;
            return mph;
        }

    }
}

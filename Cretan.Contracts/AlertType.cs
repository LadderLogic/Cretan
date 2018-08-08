using System;

namespace Cretan.Contracts
{
    /// <summary>
    /// Types of alert when off the pace. One or more can be selected
    /// </summary>
    [Flags]
    public enum AlertType
    {
        /// <summary>
        /// Haptic feedback - Long vibrate for too fast, multiple small vibrates for slow
        /// </summary>
        Haptic = 0x01,

        /// <summary>
        /// The audio notification
        /// </summary>
        AudioNotification = 0x02,

        /// <summary>
        /// system media volume lowered when too fast, increased when slow
        /// </summary>
        MediaVolume = 0x04
    }
}

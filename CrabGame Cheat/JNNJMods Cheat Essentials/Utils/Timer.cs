using System;
using System.Threading;

namespace JNNJMods.Utils
{
    public class Timer
    {

        /// <summary>
        /// Runs an action delayed
        /// </summary>
        /// <param name="action">Action to execute</param>
        /// <param name="seconds">Delay in seconds</param>
        public static void Run(Action action, float seconds)
        {
            new Thread(() =>
            {
                Thread.Sleep((int)(seconds * 1000));
                action.Invoke();
            }).Start();
        }
    }
}

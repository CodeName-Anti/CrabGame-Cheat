using CodeStage.AntiCheat.Detectors;

namespace JNNJMods.CrabGameCheat.Util
{
    public static class AntiCheat
    {
        public static void StopAntiCheat(bool dispose = true)
        {
            TimeCheatingDetector.StopDetection();
            InjectionDetector.StopDetection();
            WallHackDetector.StopDetection();
            SpeedHackDetector.StopDetection();
            ObscuredCheatingDetector.StopDetection();

            if(dispose) DisposeAntiCheat();
        }

        public static void StartAntiCheat()
        {
            TimeCheatingDetector.StartDetection();
            InjectionDetector.StartDetection();
            WallHackDetector.StartDetection();
            SpeedHackDetector.StartDetection();
            ObscuredCheatingDetector.StartDetection();
        }

        public static void DisposeAntiCheat()
        {
            TimeCheatingDetector.Dispose();
            InjectionDetector.Dispose();
            WallHackDetector.Dispose();
            SpeedHackDetector.Dispose();
            ObscuredCheatingDetector.Dispose();
        }
    }
}

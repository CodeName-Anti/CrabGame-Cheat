using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace JNNJMods.CrabGameCheat.Util
{
    public static class AntiCheat
    {
        public static void StopAntiCheat()
        {

            // Find all Detectors and disable them.
            var types = Assembly.GetAssembly(typeof(CodeStage.AntiCheat.Common.ACTk)).GetTypes().Where(t =>
                    t.IsPublic);

            foreach (Type t in types)
            {
                ExecutePublicStaticVoidMethods(t);
            }
        }

        public static async void LateStopAntiCheat()
        {
            CheatLog.Msg("Killing GameObject in 30 seconds");
            await Task.Delay(30 * 1000);
            UnityEngine.Object.Destroy(GameObject.Find("Managers/MoreSoundEffects/Sfx/Definitely just sfx here lol"));
        }

        private static void ExecutePublicStaticVoidMethods(Type t)
        {
            // Find StopDetection Method.
            var methods = t.GetMethods().Where(m =>
            m.IsStatic &&
            m.IsPublic &&
            m.Name.Contains("Stop"));

            foreach (MethodInfo method in methods)
            {
                method.Invoke(null, null);
            }
        }
    }
}

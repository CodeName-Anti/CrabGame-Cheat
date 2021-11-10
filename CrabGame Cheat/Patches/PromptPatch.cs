using HarmonyLib;

namespace JNNJMods.CrabGameCheat.Patches
{
    [HarmonyPatch(typeof(Prompt))]
    public static class PromptPatch
    {

        [HarmonyPatch(nameof(Prompt.NewPrompt))]
        [HarmonyPrefix]
        public static bool NewPrompt(ref Prompt __instance, string ONNBLMCNDNG, string NPCJLAFLHNF)
        {
            string header = ONNBLMCNDNG;
            string content = NPCJLAFLHNF;
            if (header.Equals("Rip") && content.Equals("Server owner left the game and closed the server"))
            {
                __instance.NewPrompt("Pussy detected", "The owner is a pussy and left the Server because of you. Good work! - CrabGame Cheat");

                return false;
            }

            return true;
        }

    }
}

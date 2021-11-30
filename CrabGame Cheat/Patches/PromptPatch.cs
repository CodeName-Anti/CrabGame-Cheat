using HarmonyLib;

namespace JNNJMods.CrabGameCheat.Patches
{
    [HarmonyPatch(typeof(Prompt))]
    public static class PromptPatch
    {
        [HarmonyPatch(nameof(Prompt.NewPrompt))]
        [HarmonyPrefix]
        public static bool NewPrompt(ref Prompt __instance, string param_1, string param_2)
        {
            string header = param_1;
            string content = param_2;
            if (header.Equals("Rip") && content.Equals("Server owner left the game and closed the server"))
            {
                __instance.NewPrompt("Pussy detected", "The owner is a pussy and left the Server because of you. Good work! - CrabGame Cheat");

                return false;
            }

            return true;
        }

    }
}

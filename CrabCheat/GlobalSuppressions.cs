// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Called by Unity MonoBehaviour", Scope = "member", Target = "~M:JNNJMods.CrabCheat.Cheat.Awake")]
[assembly: SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Called by Unity MonoBehaviour", Scope = "member", Target = "~M:JNNJMods.CrabCheat.Cheat.Start")]
[assembly: SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Called by Unity MonoBehaviour", Scope = "member", Target = "~M:JNNJMods.CrabCheat.Cheat.OnApplicationQuit")]
[assembly: SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Called by Unity MonoBehaviour", Scope = "member", Target = "~M:JNNJMods.CrabCheat.Cheat.Update")]
[assembly: SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Called by Unity MonoBehaviour", Scope = "member", Target = "~M:JNNJMods.CrabCheat.Cheat.FixedUpdate")]
[assembly: SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Called by Unity MonoBehaviour", Scope = "member", Target = "~M:JNNJMods.CrabCheat.Cheat.OnGUI")]

[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Called by unity, can't be static", Scope = "member", Target = "~M:JNNJMods.CrabCheat.Cheat.Start")]
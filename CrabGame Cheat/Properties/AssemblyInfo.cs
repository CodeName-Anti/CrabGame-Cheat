using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;
using JNNJMods.CrabGameCheat;
#if MELONLOADER
using JNNJMods.CrabGameCheat.Loader;
using MelonLoader;
#endif

// Allgemeine Informationen über eine Assembly werden über die folgenden
// Attribute gesteuert. Ändern Sie diese Attributwerte, um die Informationen zu ändern,
// die einer Assembly zugeordnet sind.
#if MELONLOADER
[assembly: MelonInfo(typeof(Melon), "CrabGame Cheat", Constants.Version, "JNNJ")]
[assembly: MelonGame("Dani", "Crab Game")]
#endif
[assembly: AssemblyTitle("CrabGame Cheat")]
[assembly: AssemblyDescription("A cheat for the Game \"Crab Game\"")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("JNNJ")]
[assembly: AssemblyProduct("CrabGame Cheat")]
[assembly: AssemblyCopyright("Copyright JNNJ ©  2021")]
[assembly: AssemblyTrademark("JNNJ")]
[assembly: AssemblyCulture("")]

// Durch Festlegen von ComVisible auf FALSE werden die Typen in dieser Assembly
// für COM-Komponenten unsichtbar.  Wenn Sie auf einen Typ in dieser Assembly von
// COM aus zugreifen müssen, sollten Sie das ComVisible-Attribut für diesen Typ auf "True" festlegen.
[assembly: ComVisible(false)]

// Die folgende GUID bestimmt die ID der Typbibliothek, wenn dieses Projekt für COM verfügbar gemacht wird
[assembly: Guid("cae884c7-91a9-4a0e-9281-931368520dfa")]

// Versionsinformationen für eine Assembly bestehen aus den folgenden vier Werten:
//
//      Hauptversion
//      Nebenversion
//      Buildnummer
//      Revision
//
// Sie können alle Werte angeben oder Standardwerte für die Build- und Revisionsnummern verwenden,
// indem Sie "*" wie unten gezeigt eingeben:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion(Constants.Version)]
[assembly: AssemblyFileVersion(Constants.Version)]
[assembly: NeutralResourcesLanguage("en")]

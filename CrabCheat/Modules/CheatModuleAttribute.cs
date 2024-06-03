using System;
using System.Linq;
using System.Reflection;

namespace JNNJMods.CrabCheat.Modules;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class CheatModuleAttribute : Attribute
{
	private static Module[] modules;

	public static Module[] InstantiateAll()
	{
		return InstantiateAll(Assembly.GetExecutingAssembly());
	}

	public static Module[] InstantiateAll(Assembly assembly)
	{
		// Instantiate all Modules if not already done
		modules ??= GetAllModulesTypes(assembly)
				.Select(t => Activator.CreateInstance(t, null) as Module)
				.ToArray();

		// return all modules
		return modules;
	}

	public static Type[] GetAllModuleTypes()
	{
		return GetAllModulesTypes(Assembly.GetExecutingAssembly());
	}

	public static Type[] GetAllModulesTypes(Assembly assembly)
	{
		return assembly.GetTypes()
			.Where(m => m.GetCustomAttributes(typeof(CheatModuleAttribute), false).Length > 0 && m.IsSubclassOf(typeof(Module)))
			.ToArray();
	}

}

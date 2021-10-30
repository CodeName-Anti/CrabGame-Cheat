using System.Reflection;

namespace JNNJMods.Utils
{
    public static class ReflectionExtensions
    {
        public static T GetFieldValue<T>(this object obj, string name)
        {
            // Set the flags so that private and public fields from instances will be found
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance;
            FieldInfo field = obj.GetType().GetField(name, bindingFlags);
            return (T)field?.GetValue(obj);
        }

        public static void SetFieldValue(this object obj, string name, object value)
        {
            // Set the flags so that private and public fields from instances will be found
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance;
            FieldInfo field = obj.GetType().GetField(name, bindingFlags);
            field.SetValue(obj, value);
        }

        public static object InvokeMethod(this object obj, string name)
        {
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            MethodInfo field = obj.GetType().GetMethod(name, bindingFlags);
            return field?.Invoke(obj, null);
        }

        public static object InvokeMethod(this object obj, string name, object[] parameters)
        {
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            MethodInfo field = obj.GetType().GetMethod(name, bindingFlags);
            return field?.Invoke(obj, parameters);
        }

    }

}
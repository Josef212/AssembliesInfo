using System.Reflection;

public static class ReflectionExtensions
{
    public static T GetFieldValue<T>(this object obj, string fieldName)
    {
        BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        FieldInfo field = obj.GetType().GetField(fieldName, bindingFlags);
        return (T)field?.GetValue(obj);
    }

    public static void SetFieldValue<T>(this object obj, string fieldName, T value)
    {
        BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        FieldInfo field = obj.GetType().GetField(fieldName, bindingFlags);
        field?.SetValue(obj, value);
    }
}

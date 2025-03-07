namespace Helper
{
    public static class TypeConversionHelper
    {
        public static object? ConvertToType(object? value, string? typeString)
        {
            if (value == null || string.IsNullOrWhiteSpace(typeString))
                return null;

            Type? targetType = Type.GetType(typeString);

            if (targetType == null)
                return GetDefaultValue(targetType);

            try
            {
                return Convert.ChangeType(value, targetType);
            }
            catch
            {
                return GetDefaultValue(targetType);
            }
        }

        public static object? GetDefaultValue(Type? targetType)
        {
            if (targetType != null && targetType.IsValueType)
            {
                return Activator.CreateInstance(targetType);
            }

            return null;
        }
    }
}

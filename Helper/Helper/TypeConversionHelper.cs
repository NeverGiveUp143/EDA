namespace Helper
{
    public static class TypeConversionHelper
    {
        public static object? ConvertToType(object? value, string? typeString)
        {
            if (string.IsNullOrWhiteSpace(typeString))
                return null;

            Type? targetType = Type.GetType(typeString);

            if (targetType == null)
                return GetDefaultValue(targetType);

            try
            {
                if (value is string str && targetType == typeof(Guid))
                    return Guid.TryParse(str, out var guidValue) ? guidValue : Guid.Empty;

                if (value is null && targetType == typeof(Guid))
                    return Guid.Empty;

                if (value is Guid guid && targetType == typeof(string))
                    return guid.ToString();

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

namespace Helper
{
    public static class Utlity
    {
        public static string GetClassName<T>(T? value) where T : class
        {
            return value is null ? string.Empty : typeof(T).Name;
        }
    }
}

namespace APIHandler.Extensions
{
    public static class EnumHelper
    {
        /// <summary>
        /// To get the int value of the enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToEnum<T>(this string value) where T : System.Enum
        {
            T enumValue = (T)System.Enum.Parse(typeof(T), value, true);

            return Convert.ToInt32(enumValue);
        }

        /// <summary>
        /// TO get the string value of the enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this int value) where T : System.Enum
        {
            return (T)System.Enum.ToObject(typeof(T), value);
        }
    }

}

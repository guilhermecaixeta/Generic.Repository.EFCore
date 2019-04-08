using System;

namespace Generic.Service.Extensions.Validation
{
    public static class ValidateObject
    {
        public static bool IsNull(this object value, string nameClass, string nameObject)
        {
            if (value.ValidateNullableOfObject())
            {
                throw new ArgumentNullException($"ERROR> ClassName: {nameClass} {Environment.NewLine} Message: {nameObject} is null or empty.");
            }
            return false;
        }

        public static bool IsNull(this object value, string Message)
        {
            if (value.ValidateNullableOfObject())
            {
                throw new ArgumentNullException($"ERROR> Message: {Message}");
            }
            return false;
        }

        private static bool ValidateNullableOfObject(this object value)
        {
            return value.GetType() == typeof(string) && string.IsNullOrEmpty(value.ToString()) || value == null;
        }
    }
}

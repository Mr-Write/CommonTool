using System.ComponentModel;
using System.Reflection;

namespace SqlInfoGen.Cons.Utils;

public static class EnumExtensions
{
    public static TEnum? ParseEnumByDescription<TEnum>(string description) where TEnum : struct, Enum
    {
        foreach (TEnum value in Enum.GetValues(typeof(TEnum)))
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0 && attributes[0].Description == description)
            {
                return value;
            }
        }

        return null;
    }
}

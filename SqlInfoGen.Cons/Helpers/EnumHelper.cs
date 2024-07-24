using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace SqlInfoGen.Cons.Helpers;

public static class EnumHelper
{
    private static Hashtable enumDesciption = GetDescriptionContainer();

    /// <summary>根据枚举类型和枚举值获取枚举描述</summary>
    /// <returns></returns>
    public static string ToDescription(this Enum value)
    {
        if (value == null)
            return "";
        Type type = value.GetType();
        string name = Enum.GetName(type, (object) value);
        return EnumHelper.GetDescription(type, name);
    }

    /// <summary>转化枚举及其描述为字典类型</summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="enumObj"></param>
    /// <returns></returns>
    public static Dictionary<int, string> ToDescriptionDictionary<TEnum>()
    {
        Array values = Enum.GetValues(typeof(TEnum));
        Dictionary<int, string> dictionary = new Dictionary<int, string>();
        foreach (Enum @enum in values)
            dictionary.Add(Convert.ToInt32((object) @enum), @enum.ToDescription());
        return dictionary;
    }

    /// <summary>转化枚举及其Text值转为字典类型</summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="enumObj"></param>
    /// <returns></returns>
    public static Dictionary<int, string> ToDictionary<TEnum>()
    {
        Array values = Enum.GetValues(typeof(TEnum));
        Dictionary<int, string> dictionary = new Dictionary<int, string>();
        foreach (Enum @enum in values)
            dictionary.Add(Convert.ToInt32((object) @enum), @enum.ToString());
        return dictionary;
    }

    private static bool IsIntType(double d)
    {
        return (double) (int) d != d;
    }


    private static Hashtable GetDescriptionContainer()
    {
        EnumHelper.enumDesciption = new Hashtable();
        return EnumHelper.enumDesciption;
    }

    private static void AddToEnumDescription(Type enumType)
    {
        EnumHelper.enumDesciption.Add((object) enumType, (object) EnumHelper.GetEnumDic(enumType));
    }

    /// <summary>返回 Dic&lt;枚举项，描述&gt;</summary>
    /// <param name="enumType">枚举的类型</param>
    /// <returns>Dic&lt;枚举项，描述&gt;</returns>
    private static Dictionary<string, string> GetEnumDic(Type enumType)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        foreach (FieldInfo field in enumType.GetFields())
        {
            if (field.FieldType.IsEnum)
            {
                object[] customAttributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                dictionary.Add(field.Name, ((DescriptionAttribute) customAttributes[0]).Description);
            }
        }

        return dictionary;
    }

    /// <summary>根据枚举类型和枚举值获取枚举描述</summary>
    /// <param name="enumType">枚举类型</param>
    /// <param name="enumText">枚举值</param>
    /// <returns></returns>
    private static string GetDescription(Type enumType, string enumText)
    {
        if (string.IsNullOrEmpty(enumText))
            return (string) null;
        if (!EnumHelper.enumDesciption.ContainsKey((object) enumType))
            EnumHelper.AddToEnumDescription(enumType);
        object obj = EnumHelper.enumDesciption[(object) enumType];
        if (obj != null && !string.IsNullOrEmpty(enumText))
            return ((Dictionary<string, string>) obj)[enumText];
        throw new ApplicationException("不存在枚举的描述");
    }
}
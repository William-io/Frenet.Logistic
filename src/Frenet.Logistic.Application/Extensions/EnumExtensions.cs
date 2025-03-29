using System.Reflection;
using System.Runtime.Serialization;

namespace Frenet.Logistic.Application.Extensions;

public static class EnumExtensions
{
    public static string ToEnumString<T>(this T value) where T : Enum
    {
        var enumType = typeof(T);
        var memberInfo = enumType.GetMember(value.ToString());
        if (memberInfo.Length > 0)
        {
            var attr = memberInfo[0].GetCustomAttribute<EnumMemberAttribute>();
            if (attr != null)
            {
                return attr.Value ?? value.ToString();
            }
        }
        return value.ToString();
    }
}

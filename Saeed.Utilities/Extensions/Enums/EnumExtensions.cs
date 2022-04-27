using System;
using System.ComponentModel;

namespace Saeed.Utilities.Extensions.Enums
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum genericEnum)
        {
            var genericEnumType = genericEnum.GetType();

            var memberInfo =
                genericEnumType.GetMember(genericEnum.ToString());

            if (memberInfo != null && memberInfo.Length > 0)
            {
                dynamic _Attribs = memberInfo[0].GetCustomAttributes
                    (typeof(DescriptionAttribute), false);

                if (_Attribs != null && _Attribs.Length > 0)
                {
                    return ((DescriptionAttribute)_Attribs[0]).Description;
                }
            }

            return genericEnum.ToString();
        }
    }
}

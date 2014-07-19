using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TodoList.WpfClient
{

    public static class EnumExtension
    {
        public static string GetDescription(this Enum enumObj)
        {
            FieldInfo fieldInfo =
            enumObj.GetType().GetField(enumObj.ToString());
            object[] attribArray = fieldInfo.GetCustomAttributes(false);
            if (attribArray.Length == 0)
                return enumObj.ToString();
            else
            {
                DescriptionAttribute attrib =
                    attribArray[0] as DescriptionAttribute;
                return attrib.Description;
            }
        }
    }
}

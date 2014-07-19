// © 2009 Michele Leroux Bustamante. All rights reserved. 
// See http://wcfguidanceforwpf.codeplex.com for related whitepaper and updates
// For an intro to WCF see Michele's book: Learning WCF, O'Reilly 2007 (updated August 2008 for VS 2008)
// See http://www.thatindigogirl.com for the book code!
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TodoList.WpfClient.Utilities
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

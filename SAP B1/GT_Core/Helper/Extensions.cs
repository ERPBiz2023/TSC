using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GTCore.Helper
{
    public static class Extensions
    {
        public static string GetDescription(this Enum enumdata)
        {
            return (enumdata.GetType().GetTypeInfo().GetMember(enumdata.ToString())
                .FirstOrDefault(x => x.MemberType == MemberTypes.Field).GetCustomAttributes(typeof(DescriptionAttribute), inherit: false)
                .SingleOrDefault() as DescriptionAttribute)?.Description ?? enumdata.ToString();
        }


        public static T GetEnumValueByDescription<T>(this string description) where T : Enum
        {
            foreach (Enum enumItem in Enum.GetValues(typeof(T)))
            {
                if (enumItem.GetDescription() == description)
                {
                    return (T)enumItem;
                }
            }
            throw new ArgumentException("Not found.", nameof(description));
        }

        public static Hashtable ToHashtable(this DataRowView raw)
        {
            var hashtable = new Hashtable();
            foreach (DataColumn col in raw.Row.Table.Columns)
            {
                hashtable[col.ColumnName] = ToString(raw[col.ColumnName]);
            }

            return hashtable;
        }

        public static Hashtable[] ToHashtableArray(this DataView dataView)
        {
            return (from DataRowView row in dataView
                    select row.ToHashtable()).ToArray();
        }

        public static string ToString(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return string.Empty;
            }

            return value.ToString();
        }
    }
}

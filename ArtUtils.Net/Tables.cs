using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using ArtUtils.Net.Attributes;

namespace ArtUtils.Net
{
    public static class Tables
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> list, string tableName = "", string nameSpace = "")
        {
            var type = typeof(T);
            var properties = type.GetProperties();

            var dataTable = new DataTable
            {
                TableName = tableName,
                Namespace = nameSpace
            };

            foreach (var info in properties)
            {
                dataTable.Columns.Add(new DataColumn(GetColumnName(info),
                    Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (var entity in list)
            {
                var values = new object[properties.Length];
                for (var i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        internal static string GetColumnName(PropertyInfo info)
        {
            var result = info.Name;
            foreach (var attr in Attribute.GetCustomAttributes(info))
            {
                if (attr is FieldName fn)
                {
                    result = fn.Name;
                    break;
                }
            }

            return result;
        }
    }
}

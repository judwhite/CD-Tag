﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace CDTag.Common.Json
{
    public static partial class JsonSerializer
    {
        private const int IndentSpacing = 4;

        /// <summary>
        /// Serializes the object to a JSON string.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>The JSON string.</returns>
        public static string SerializeObject(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            StringBuilder json = new StringBuilder();
            SerializeClass(obj, json, indentLevel: 1);

            return json.ToString();
        }

        private static void SerializeClass(object obj, StringBuilder json, int indentLevel)
        {
            if (json == null)
                throw new ArgumentNullException("json");
            if (indentLevel < 1)
                throw new ArgumentOutOfRangeException("indentLevel", indentLevel, "indentLevel must be >= 1");

            if (obj == null)
            {
                json.Append("null");
                return;
            }

            Type classType = obj.GetType();
            if (obj is IDictionary)
            {
                json.AppendLine("{");

                IDictionary dict = (IDictionary)obj;
                var keys = dict.Keys.Cast<string>().ToArray();
                for (int i = 0; i < keys.Length; i++)
                {
                    string key = keys[i];

                    json.Append(new string(' ', indentLevel * IndentSpacing));
                    json.Append(string.Format("\"{0}\": ", key.Replace("\\", "\\\\").Replace("\"", "\\\"")));
                    SerializeClass(dict[key], json, indentLevel + 1);
                    if (i == keys.Length - 1)
                        json.AppendLine();
                    else
                        json.AppendLine(",");
                }

                json.Append(new string(' ', (indentLevel - 1) * IndentSpacing));
                json.AppendLine("}");
            }
            else if (obj is string)
            {
                json.Append(string.Format("\"{0}\"", ((string)obj).Replace("\\", "\\\\").Replace("\"", "\\\"")));
            }
            else if (
                obj is int || obj is int? ||
                obj is short || obj is short? ||
                obj is long || obj is long? ||
                obj is uint || obj is uint? ||
                obj is ushort || obj is ushort? ||
                obj is ulong || obj is ulong? ||
                obj is float || obj is float? ||
                obj is double || obj is double? ||
                obj is decimal || obj is decimal?
            )
            {
                // TODO: trim trailing 0's after decimal place
                // TODO: have min 1 decimal place for float/double/decimal (including 0)
                json.Append(obj);
            }
            else if (obj is bool || obj is bool?)
            {
                json.Append(obj.ToString().ToLower());
            }
            else if (obj is DateTime || obj is DateTime?)
            {
                json.Append(string.Format("\"{0:yyyy-MM-dd} {0:hh:MM:ss}\"", obj));
            }
            else if (classType.IsEnum)
            {
                // TODO: Make use of EnumValue attribute

                json.AppendFormat("\"{0}\"", obj);
            }
            else if (classType.IsArray)
            {
                // TODO
                json.AppendLine("[");

                IList array = (IList)obj;
                int count = array.Cast<object>().Count();

                for (int i = 0; i < count; i++)
                {
                    object item = array[i];

                    json.Append(new string(' ', indentLevel * IndentSpacing));
                    SerializeClass(item, json, indentLevel + 1);

                    if (i == count - 1)
                        json.AppendLine();
                    else
                        json.AppendLine(",");
                }

                json.Append(new string(' ', (indentLevel - 1) * IndentSpacing));
                json.Append("]");
            }
            else if (classType.IsClass)
            {
                json.AppendLine("{");

                PropertyInfo[] properties = classType.GetProperties(PublicInstanceBindingFlags);
                for (int i = 0; i < properties.Length; i++)
                {
                    var property = properties[i];

                    json.Append(new string(' ', indentLevel * IndentSpacing));
                    json.Append(string.Format("\"{0}\": ", property.Name));
                    SerializeClass(property.GetGetMethod().Invoke(obj, null), json, indentLevel + 1);

                    if (i < properties.Length - 1)
                        json.AppendLine(",");
                    else
                        json.AppendLine();
                }

                json.Append(new string(' ', (indentLevel - 1) * IndentSpacing));
                json.Append("}");
            }
            else
            {
                throw new Exception(string.Format("Type '{0}' not supported for serialization.", classType));
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;

namespace CDTag.Common.Json
{
    /// <summary>
    /// JsonSerializer
    /// </summary>
    public static partial class JsonSerializer
    {
        private const BindingFlags PublicInstanceBindingFlags = BindingFlags.Instance | BindingFlags.Public;
        private const BindingFlags PublicStaticBindingFlags = BindingFlags.Static | BindingFlags.Public;

        /// <summary>
        /// Reads the object for the specified stream.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns>The deserialized object.</returns>
        public static T ReadObject<T>(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            long length = stream.Length;
            byte[] bytes = new byte[length];
            stream.Read(bytes, 0, (int)length);

            string json = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            return ReadObject<T>(json);
        }

        /// <summary>
        /// Reads the object for the specified stream.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="json">The JSON string.</param>
        /// <returns>The deserialized object.</returns>
        public static T ReadObject<T>(string json)
        {
            int i = 0;
            return (T)ParseClass(typeof(T), json, ref i);
        }

        private static object ParseArray(Type expectedType, string json, ref int i)
        {
            if (json[i] != '[')
                throw new JsonInvalidDataException("'[' expected", json, i);

            i++;

            while (json[i] == ' ' || json[i] == '\r' || json[i] == '\n' || json[i] == '\t')
                i++;

            var list = new List<object>();
            Type elementType = expectedType.GetElementType();
            while (json[i] != ']')
            {
                if (elementType.IsEnum)
                {
                    object enumValue = ParseEnum(classType: expectedType, propertyName: "root", elementType: elementType, json: json, i: ref i);
                    list.Add(enumValue);
                }
                else if (IsPrimitiveType(elementType))
                {
                    object primitiveValue = GetValue(expectedType, elementType, "", json, ref i);
                    list.Add(primitiveValue);

                    while (json[i] == ' ' || json[i] == '\r' || json[i] == '\n' || json[i] == '\t')
                        i++;
                }
                else
                {
                    object val = ParseClass(elementType, json, ref i);
                    list.Add(val);

                    while (json[i] == ' ' || json[i] == '\r' || json[i] == '\n' || json[i] == '\t')
                        i++;

                    if (json[i] != ',' && json[i] != ']')
                        throw new JsonInvalidDataException("',' expected.", json, i);
                    if (json[i] == ',')
                        i++;

                    while (json[i] == ' ' || json[i] == '\r' || json[i] == '\n' || json[i] == '\t')
                        i++;
                }
            }
            i++;

            var value = Array.CreateInstance(elementType, list.Count);
            for (int j = 0; j < value.Length; j++)
                value.SetValue(list[j], j);

            return value;
        }

        private static object ParseClass(Type expectedType, string json, ref int i)
        {
            if (json[i] != '{' && json[i] != '[')
                throw new JsonInvalidDataException("'{' or '[' expected", json, i);

            if (json[i] == '[')
                return ParseArray(expectedType, json, ref i);

            i++;

            ConstructorInfo ci = expectedType.GetConstructor(Type.EmptyTypes);
            if (ci == null)
                throw new JsonInvalidDataException(string.Format("No parameterless constructor found for '{0}'.", expectedType), json, i);

            object returnValue = ci.Invoke(null);

            bool isDictionary = false;
            if (returnValue is IDictionary)
                isDictionary = true;

            while (json[i] != '}')
            {
                while (json[i] == ' ' || json[i] == '\r' || json[i] == '\n' || json[i] == '\t')
                    i++;
                string propertyName = GetValue(json, ref i);
                
                if (json[i++] != ':')
                    throw new JsonInvalidDataException("':' expected", json, i - 1);
                while (json[i] == ' ' || json[i] == '\r' || json[i] == '\n' || json[i] == '\t')
                    i++;

                Type propertyType;
                Action<object, object[]> setMethodInvoke;

                if (!isDictionary)
                {
                    PropertyInfo property = null;
                    foreach (PropertyInfo propertyInfo in expectedType.GetProperties(PublicInstanceBindingFlags))
                    {
                        DataMemberAttribute[] dms = (DataMemberAttribute[])propertyInfo.GetCustomAttributes(typeof(DataMemberAttribute), false);
                        if (dms != null && dms.Length != 0)
                        {
                            string name = dms[0].Name;
                            if (string.IsNullOrEmpty(name))
                                name = propertyInfo.Name;

                            if (string.Compare(name, propertyName, ignoreCase: true) == 0)
                            {
                                property = propertyInfo;
                                break;
                            }
                        }
                    }

                    if (property == null)
                        throw new JsonInvalidDataException(string.Format("Property '{0}' not found on '{1}'.", propertyName, expectedType), json, i);

                    MethodInfo setMethod = property.GetSetMethod();
                    if (setMethod == null)
                        throw new JsonInvalidDataException(string.Format("Property setter '{0}' not found on '{1}'.", propertyName, expectedType), json, i);

                    MethodInfo getMethod = property.GetGetMethod();
                    if (getMethod == null)
                        throw new JsonInvalidDataException(string.Format("Property getter '{0}' not found on '{1}'.", propertyName, expectedType), json, i);

                    setMethodInvoke = (o, p) => setMethod.Invoke(o, p);
                    propertyType = getMethod.ReturnType;

                    if (propertyType.Name.StartsWith("Nullable`1"))
                        propertyType = propertyType.GetGenericArguments()[0];
                }
                else
                {
                    setMethodInvoke = (o, p) => ((IDictionary)returnValue).Add(propertyName, p[0]);
                    propertyType = expectedType.GetGenericArguments()[1];
                }

                if (propertyType.IsEnum)
                {
                    object value = ParseEnum(classType: expectedType, propertyName: propertyName, elementType: propertyType, json: json, i: ref i);
                    setMethodInvoke(returnValue, new[] { value });
                }
                else if (propertyType.IsArray)
                {
                    object value = ParseArray(propertyType, json, ref i);

                    setMethodInvoke(returnValue, new[] { value });

                    while (json[i] == ' ' || json[i] == '\r' || json[i] == '\n' || json[i] == '\t')
                        i++;
                    if (json[i] != ',' && json[i] != '}')
                        throw new JsonInvalidDataException("',' expected", json, i);
                    if (json[i] == ',')
                        i++;
                }
                else if (IsPrimitiveType(propertyType))
                {
                    object value = GetValue(expectedType, propertyType, propertyName, json, ref i);

                    setMethodInvoke(returnValue, new[] { value });
                }
                else
                {
                    object value = ParseClass(propertyType, json, ref i);

                    setMethodInvoke(returnValue, new[] { value });

                    while (json[i] == ' ' || json[i] == '\r' || json[i] == '\n' || json[i] == '\t')
                        i++;
                    if (json[i] == ',')
                        i++;
                }
            }
            i++;

            return returnValue;
        }

        private static object GetValue(Type containingClassType, Type propertyType, string propertyName, string json, ref int i)
        {
            string value = GetValue(json, ref i);

            object newValue;
            if (string.IsNullOrEmpty(value))
            {
                newValue = null;
            }
            else
            {
                if (propertyType == typeof(int) || propertyType == typeof(int?))
                    newValue = int.Parse(value);
                else if (propertyType == typeof(short) || propertyType == typeof(short?))
                    newValue = short.Parse(value);
                else if (propertyType == typeof(long) || propertyType == typeof(long?))
                    newValue = long.Parse(value);
                else if (propertyType == typeof(uint) || propertyType == typeof(uint?))
                    newValue = uint.Parse(value);
                else if (propertyType == typeof(ushort) || propertyType == typeof(ushort?))
                    newValue = ushort.Parse(value);
                else if (propertyType == typeof(ulong) || propertyType == typeof(ulong?))
                    newValue = ulong.Parse(value);
                else if (propertyType == typeof(float) || propertyType == typeof(float?))
                    newValue = float.Parse(value);
                else if (propertyType == typeof(double) || propertyType == typeof(double?))
                    newValue = double.Parse(value);
                else if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
                    newValue = decimal.Parse(value);
                else if (propertyType == typeof(bool) || propertyType == typeof(bool?))
                {
                    if (value == "0")
                        newValue = false;
                    else if (value == "1")
                        newValue = true;
                    else
                        newValue = bool.Parse(value);
                }
                else if (propertyType == typeof(string))
                    newValue = value;
                else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                    newValue = DateTime.Parse(value);
                else
                    throw new JsonInvalidDataException(string.Format("Unsupported type '{0}'. Property '{1}' on '{2}'.", propertyType, propertyName, containingClassType), json, i);
            }

            return newValue;
        }

        private static bool IsPrimitiveType(Type propertyType)
        {
            return propertyType.IsPrimitive || propertyType == typeof(string) || propertyType == typeof(decimal) || propertyType == typeof(DateTime);
        }

        private static object ParseEnum(Type classType, string propertyName, Type elementType, string json, ref int i)
        {
            string value = GetValue(json, ref i);

            bool hasIntValue = false;
            int intValue;
            if (int.TryParse(value, out intValue))
                hasIntValue = true;

            foreach (var field in elementType.GetFields(PublicStaticBindingFlags))
            {
                EnumMemberAttribute[] dms = (EnumMemberAttribute[])field.GetCustomAttributes(typeof(EnumMemberAttribute), false);
                if (dms != null && dms.Length != 0)
                {
                    if (dms[0].Value == value)
                    {
                        object val = field.GetValue(null);
                        return val;
                    }
                }
                else if (hasIntValue)
                {
                    object enumValue = field.GetValue(null);

                    if (intValue == (int)enumValue)
                    {
                        return enumValue;
                    }
                }
                else
                {
                    object enumValue = field.GetValue(null);
                    string enumValueString = enumValue.ToString();

                    if (string.Compare(value, enumValueString, ignoreCase: true) == 0)
                    {
                        return enumValue;
                    }
                }
            }

            throw new JsonInvalidDataException(string.Format("Class: {0} Property: {1} Type: {2} Enum value '{3}' not found", classType, propertyName, elementType, value), json, i);
        }

        private static string GetValue(string json, ref int i)
        {
            bool isQuoted = (json[i] == '"');
            if (isQuoted)
                i++;

            string value = string.Empty;
            for (; i < json.Length; i++)
            {
                if (json[i] == '"' && isQuoted)
                {
                    i++;
                    break;
                }

                if (json[i] == ',' && !isQuoted)
                    break;
                if (json[i] == '}' && !isQuoted)
                    break;
                if (json[i] == ']' && !isQuoted)
                    break;
                if (json[i] == ':' && !isQuoted)
                    break;

                if (json[i] == '\\')
                {
                    i++;
                    if (json[i] != '\"' && json[i] != '\\')
                        throw new JsonInvalidDataException(string.Format("Unknown escape character sequence '\\{0}'.", json[i]), json, i);
                }

                value += json[i];
            }

            while (json[i] == ' ' || json[i] == '\r' || json[i] == '\n' || json[i] == '\t')
                i++;
            if (json[i] != ',' && json[i] != '}' && json[i] != ']' && json[i] != ':')
                throw new JsonInvalidDataException("',' expected", json, i);
            if (json[i] == ',')
                i++;

            return value;
        }
    }
}

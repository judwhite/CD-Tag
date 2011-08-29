using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace CDTag.Common.Json
{
    /// <summary>
    /// JsonSerializer
    /// </summary>
    public class JsonSerializer
    {
        private readonly Type _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializer"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public JsonSerializer(Type type)
        {
            _type = type;
        }

        /// <summary>
        /// Reads the object for the specified stream.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns>The deserialized object.</returns>
        public static T ReadObject<T>(Stream stream)
        {
            object obj = new JsonSerializer(typeof(T)).ReadObject(stream);
            return (T)obj;
        }

        /// <summary>
        /// Reads the object for the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The deserialized object.</returns>
        public object ReadObject(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            long length = stream.Length;
            byte[] bytes = new byte[length];
            stream.Read(bytes, 0, (int)length);

            string json = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            int i = 0;
            return ParseClass(_type, json, ref i);
        }

        private static object ParseClass(Type expectedType, string json, ref int i)
        {
            const BindingFlags bf = BindingFlags.Instance | BindingFlags.Public;

            if (json[i++] != '{')
                throw new JsonInvalidDataException("'{' expected", json, i - 1);

            ConstructorInfo ci = expectedType.GetConstructor(Type.EmptyTypes);
            if (ci == null)
                throw new JsonInvalidDataException(string.Format("No parameterless constructor found for '{0}'.", expectedType), json, i);

            object returnValue = ci.Invoke(null);

            while (json[i] != '}')
            {
                while (json[i] == ' ' || json[i] == '\r' || json[i] == '\n')
                    i++;
                if (json[i++] != '"')
                    throw new JsonInvalidDataException("'\"' expected", json, i - 1);
                string propertyName = string.Empty;
                for (; i < json.Length; i++)
                {
                    if (json[i] == '"')
                        break;

                    propertyName += json[i];
                }

                if (json[i++] != '"')
                    throw new JsonInvalidDataException("'\"' expected", json, i - 1);
                if (json[i++] != ':')
                    throw new JsonInvalidDataException("':' expected", json, i - 1);
                while (json[i] == ' ' || json[i] == '\r' || json[i] == '\n')
                    i++;

                PropertyInfo property = null;
                foreach (PropertyInfo propertyInfo in expectedType.GetProperties(bf))
                {
                    DataMemberAttribute[] dms = (DataMemberAttribute[])propertyInfo.GetCustomAttributes(typeof(DataMemberAttribute), false);
                    if (dms != null && dms.Length != 0)
                    {
                        if (string.Compare(dms[0].Name, propertyName, ignoreCase: true) == 0)
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

                Type propertyType = getMethod.ReturnType;
                if (propertyType.IsEnum)
                {
                    object value = ParseEnum(classType: expectedType, propertyName: propertyName, elementType: propertyType, json: json, i: ref i);
                    setMethod.Invoke(returnValue, new[] { value });
                }
                else if (propertyType.IsArray)
                {
                    if (json[i++] != '[')
                        throw new JsonInvalidDataException("'[' expected", json, i - 1);

                    while (json[i] == ' ' || json[i] == '\r' || json[i] == '\n')
                        i++;

                    var list = new List<object>();
                    Type elementType = propertyType.GetElementType();
                    while (json[i] != ']')
                    {
                        if (elementType.IsEnum)
                        {
                            object enumValue = ParseEnum(classType: expectedType, propertyName: propertyName, elementType: elementType, json: json, i: ref i);
                            list.Add(enumValue);
                        }
                        else
                        {
                            object val = ParseClass(elementType, json, ref i);
                            list.Add(val);

                            while (json[i] == ' ' || json[i] == '\r' || json[i] == '\n')
                                i++;

                            if (json[i] != ',' && json[i] != ']')
                                throw new JsonInvalidDataException("',' expected.", json, i);
                            if (json[i] == ',')
                                i++;

                            while (json[i] == ' ' || json[i] == '\r' || json[i] == '\n')
                                i++;
                        }
                    }
                    i++;

                    var value = Array.CreateInstance(elementType, list.Count);
                    for (int j = 0; j < value.Length; j++)
                        value.SetValue(list[j], j);

                    setMethod.Invoke(returnValue, new object[] { value });

                    while (json[i] == ' ' || json[i] == '\r' || json[i] == '\n')
                        i++;
                    if (json[i] != ',' && json[i] != '}')
                        throw new JsonInvalidDataException("',' expected", json, i);
                    if (json[i] == ',')
                        i++;
                }
                else if (propertyType.IsPrimitive || propertyType == typeof(string) || propertyType == typeof(decimal) || propertyType == typeof(DateTime))
                {
                    string value = GetValue(json, ref i);

                    if (propertyType == typeof(int))
                        setMethod.Invoke(returnValue, new object[] { int.Parse(value) });
                    else if (propertyType == typeof(long))
                        setMethod.Invoke(returnValue, new object[] { long.Parse(value) });
                    else if (propertyType == typeof(double))
                        setMethod.Invoke(returnValue, new object[] { double.Parse(value) });
                    else if (propertyType == typeof(decimal))
                        setMethod.Invoke(returnValue, new object[] { decimal.Parse(value) });
                    else if (propertyType == typeof(bool))
                        setMethod.Invoke(returnValue, new object[] { bool.Parse(value) });
                    else if (propertyType == typeof(string))
                        setMethod.Invoke(returnValue, new object[] { value });
                    else if (propertyType == typeof(DateTime))
                        setMethod.Invoke(returnValue, new object[] { DateTime.Parse(value) });
                    else
                    {
                        throw new JsonInvalidDataException(string.Format("Unsupport type '{0}'. Property '{1}' on '{2}'.", propertyType, propertyName, expectedType), json, i);
                    }
                }
                else
                {
                    ParseClass(propertyType, json, ref i);
                }
            }
            i++;

            return returnValue;
        }

        private static object ParseEnum(Type classType, string propertyName, Type elementType, string json, ref int i)
        {
            string value = GetValue(json, ref i);

            foreach (var field in elementType.GetFields(BindingFlags.Static | BindingFlags.Public))
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

                if (json[i] == '\\')
                {
                    i++;
                    if (json[i] != '\"')
                        throw new JsonInvalidDataException(string.Format("Unknown escape character sequence '\\{0}'.", json[i]), json, i);
                }

                value += json[i];
            }

            while (json[i] == ' ' || json[i] == '\r' || json[i] == '\n')
                i++;
            if (json[i] != ',' && json[i] != '}' && json[i] != ']')
                throw new JsonInvalidDataException("',' expected", json, i);
            if (json[i] == ',')
                i++;

            return value;
        }

        /// <summary>
        /// Serializes the object to a JSON string.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>The JSON string.</returns>
        public static string SerializeObject(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            throw new NotImplementedException();
        }
    }
}

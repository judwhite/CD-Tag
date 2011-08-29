using System;

namespace CDTag.Common.Json
{
    /// <summary>
    /// JsonInvalidDataException
    /// </summary>
    public class JsonInvalidDataException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonInvalidDataException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="json">The JSON string.</param>
        /// <param name="index">The index in the JSON string where the exception occurred.</param>
        public JsonInvalidDataException(string message, string json, int index)
            : base(message + "  Index: " + index + "  Actual: '" + json[index] + "'  Near: " + json.Substring(index, json.Length - index > 10 ? 10 : json.Length - index) + "  Full JSON: " + json)
        {
        }
    }
}

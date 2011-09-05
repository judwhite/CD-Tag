using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CDTag.Common.Tests.Json.Model
{
    [DataContract]
    public class EveryType
    {
        [DataMember]
        public string String { get; set; }

        [DataMember]
        public string NullString { get; set; }

        [DataMember]
        public bool Boolean { get; set; }

        [DataMember]
        public DateTime DateTime { get; set; }

        [DataMember]
        public int Int32 { get; set; }

        [DataMember]
        public uint UInt32 { get; set; }

        [DataMember]
        public short Int16 { get; set; }

        [DataMember]
        public ushort UInt16 { get; set; }

        [DataMember]
        public long Int64 { get; set; }

        [DataMember]
        public ulong UInt64 { get; set; }

        [DataMember]
        public float Single { get; set; }

        [DataMember]
        public double Double { get; set; }

        [DataMember]
        public decimal Decimal { get; set; }

        [DataMember]
        public bool[] BooleanArray { get; set; }

        [DataMember]
        public DateTime[] DateTimeArray { get; set; }

        [DataMember]
        public string[] StringArray { get; set; }

        [DataMember]
        public int[] Int32Array { get; set; }

        [DataMember]
        public uint[] UInt32Array { get; set; }

        [DataMember]
        public short[] Int16Array { get; set; }

        [DataMember]
        public ushort[] UInt16Array { get; set; }

        [DataMember]
        public long[] Int64Array { get; set; }

        [DataMember]
        public ulong[] UInt64Array { get; set; }

        [DataMember]
        public float[] SingleArray { get; set; }

        [DataMember]
        public double[] DoubleArray { get; set; }

        [DataMember]
        public decimal[] DecimalArray { get; set; }

        [DataMember]
        public Dictionary<string, bool> BooleanDictionary { get; set; }

        [DataMember]
        public Dictionary<string, DateTime> DateTimeDictionary { get; set; }

        [DataMember]
        public Dictionary<string, string> StringDictionary { get; set; }

        [DataMember]
        public Dictionary<string, int> Int32Dictionary { get; set; }

        [DataMember]
        public Dictionary<string, uint> UInt32Dictionary { get; set; }

        [DataMember]
        public Dictionary<string, short> Int16Dictionary { get; set; }

        [DataMember]
        public Dictionary<string, ushort> UInt16Dictionary { get; set; }

        [DataMember]
        public Dictionary<string, long> Int64Dictionary { get; set; }

        [DataMember]
        public Dictionary<string, ulong> UInt64Dictionary { get; set; }

        [DataMember]
        public Dictionary<string, float> SingleDictionary { get; set; }

        [DataMember]
        public Dictionary<string, double> DoubleDictionary { get; set; }

        [DataMember]
        public Dictionary<string, decimal> DecimalDictionary { get; set; }

        [DataMember]
        public TestEnum Enum { get; set; }

        [DataMember]
        public TestEnum[] EnumArray { get; set; }

        [DataMember]
        public Dictionary<string, TestEnum> EnumDictionary { get; set; }

        [DataMember]
        public TestClass Class { get; set; }

        [DataMember]
        public TestClass[] ClassArray { get; set; }

        [DataMember]
        public Dictionary<string, TestClass> ClassDictionary { get; set; }
    }

    [DataContract]
    public class TestClass
    {
        [DataMember]
        public string Name { get; set; }
    }

    public enum TestEnum
    {
        One,
        Two,
        Three
    }
}

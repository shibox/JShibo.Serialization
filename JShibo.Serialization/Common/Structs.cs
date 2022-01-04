using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.Common
{
    #region 内部类

    internal struct TwoDigits
    {
        public readonly char First;
        public readonly char Second;

        public TwoDigits(char first, char second)
        {
            First = first;
            Second = second;
        }
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct GuidStruct
    {
        [FieldOffset(0)]
        private Guid Value;

        [FieldOffset(0)]
        public readonly ulong L00;
        [FieldOffset(8)]
        public readonly ulong L01;

        [FieldOffset(0)]
        public readonly byte B00;
        [FieldOffset(1)]
        public readonly byte B01;
        [FieldOffset(2)]
        public readonly byte B02;
        [FieldOffset(3)]
        public readonly byte B03;
        [FieldOffset(4)]
        public readonly byte B04;
        [FieldOffset(5)]
        public readonly byte B05;

        [FieldOffset(6)]
        public readonly byte B06;
        [FieldOffset(7)]
        public readonly byte B07;
        [FieldOffset(8)]
        public readonly byte B08;
        [FieldOffset(9)]
        public readonly byte B09;

        [FieldOffset(10)]
        public readonly byte B10;
        [FieldOffset(11)]
        public readonly byte B11;

        [FieldOffset(12)]
        public readonly byte B12;
        [FieldOffset(13)]
        public readonly byte B13;
        [FieldOffset(14)]
        public readonly byte B14;
        [FieldOffset(15)]
        public readonly byte B15;

        public GuidStruct(Guid invisibleMembers)
            : this()
        {
            Value = invisibleMembers;
        }
    }

    //[StructLayout(LayoutKind.Explicit, Pack = 1)]
    //internal struct GuidStructSoc
    //{
    //    [FieldOffset(0)]
    //    private Guid Value;

    //    [FieldOffset(0)]
    //    public readonly int _a;
    //    [FieldOffset(4)]
    //    public readonly short _b;
    //    [FieldOffset(6)]
    //    public readonly short _c;
    //    [FieldOffset(8)]
    //    public readonly byte _d;
    //    [FieldOffset(9)]
    //    public readonly byte _e;
    //    [FieldOffset(10)]
    //    public readonly byte _f;
    //    [FieldOffset(11)]
    //    public readonly byte _g;
    //    [FieldOffset(12)]
    //    public readonly byte _h;
    //    [FieldOffset(13)]
    //    public readonly byte _i;
    //    [FieldOffset(14)]
    //    public readonly byte _j;
    //    [FieldOffset(15)]
    //    public readonly byte _k;

    //    public GuidStructSoc(Guid invisibleMembers)
    //        : this()
    //    {
    //        Value = invisibleMembers;
    //    }
    //}

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct DecimalStruct
    {
        [FieldOffset(0)]
        private decimal Value;

        [FieldOffset(0)]
        public readonly int flags;
        [FieldOffset(4)]
        public readonly int hi;
        [FieldOffset(8)]
        public readonly int lo;
        [FieldOffset(12)]
        public readonly int mid;

        public DecimalStruct(decimal invisibleMembers)
            : this()
        {
            Value = invisibleMembers;
        }
    }

    #endregion
}

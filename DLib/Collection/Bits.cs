using System;
using System.Text;

namespace DLib.Collection
{
    public class Bits : ParallelExecutionObject
    {
        public bool[] array;
        int internalLength, shift;
        public int Length => System.Math.Max(internalLength + shift, 0);


        public Bits()
        {
            array = new bool[64];
            internalLength = 0;
            shift = 0;
        }

        public Bits(int length)
        {
            array = new bool[length << 1];
            internalLength = 0;
            shift = 0;
        }

        public Bits(int length, bool value)
        {
            array = new bool[length << 1];
            Loop(0, length, i => array[i] = value);
            if (value)
                internalLength = length;
            shift = 0;
        }

        public Bits(params bool[] array)
        {
            internalLength = array.Length;
            for (; internalLength > 0 && !array[internalLength - 1]; internalLength--) ;
            this.array = new bool[internalLength << 1];
            Loop(0, internalLength, i => this.array[i] = array[i]);
            shift = 0;
        }

        public Bits(Bits bits)
        {
            array = new bool[bits.Length << 1];
            Loop(System.Math.Max(0, bits.shift), bits.Length, j => array[j] = bits.array[j - bits.shift]);
            internalLength = bits.Length;
            shift = 0;
        }


        public bool this[int i]
        {
            get => i < shift || i >= Length ? false : array[i - shift];
            set
            {
                if (i < 0)
                    throw new IndexOutOfRangeException("index: " + i);
                if (i >= shift && i - shift < array.Length)
                {
                    array[i - shift] = value;
                    if (value && i - shift >= internalLength)
                        internalLength = i - shift + 1;
                    else if (!value && i - shift == internalLength - 1)
                    {
                        for (; internalLength > 0 && !array[internalLength - 1]; internalLength--) ;
                        if (internalLength == 0)
                            shift = 0;
                    }
                }
                else if (value)
                {
                    if (i < shift)
                    {
                        for (int j = internalLength + shift - 1; j >= shift; j--)
                            array[j] = array[j - shift];
                        Loop(0, shift, j => array[j] = false);
                    }
                    else
                    {
                        var newArray = new bool[(i + 1) << 1];
                        Loop(System.Math.Max(0, shift), internalLength + shift, j => newArray[j] = array[j - shift]);
                        array = newArray;
                    }
                    shift = 0;
                    internalLength = i + 1;
                    array[i] = true;
                }
            }
        }


        void Shorten()
        {
            if (array.Length >> 2 > internalLength)
            {
                var newArray = new bool[Length << 1];
                Loop(System.Math.Max(0, shift), internalLength + shift, j => newArray[j] = array[j - shift]);
                internalLength += shift;
                shift = 0;
                array = newArray;
            }
        }


        public static Bits operator &(Bits a, Bits b)
        {
            var bits = new bool[System.Math.Min(a.Length, b.Length)];
            Loop(0, bits.Length, i => bits[i] = a[i] & b[i]);
            return new Bits() { array = bits };
        }

        public static Bits And(params Bits[] bitss)
        {
            int min = bitss[0].Length;
            for (int i = 1; i < bitss.Length; i++)
                if (bitss[i].Length < min)
                    min = bitss[i].Length;
            var bits = new bool[min];
            Loop(0, min, i =>
            {
                bits[i] = true;
                for (int j = 0; j < bitss.Length && !(bits[i] = bitss[j][i]); j++) ;
            });
            return new Bits() { array = bits };
        }

        public static Bits operator |(Bits a, Bits b)
        {
            var bits = new bool[System.Math.Max(a.Length, b.Length)];
            Loop(0, bits.Length, i => bits[i] = a[i] | b[i]);
            return new Bits() { array = bits };
        }

        public static Bits Or(params Bits[] bitss)
        {
            int max = bitss[0].Length;
            for (int i = 1; i < bitss.Length; i++)
                if (bitss[i].Length > max)
                    max = bitss[i].Length;
            var bits = new bool[max];
            Loop(0, max, i =>
            {
                bits[i] = false;
                for (int j = 0; j < bitss.Length && (bits[i] = bitss[j][i]); j++) ;
            });
            return new Bits() { array = bits };
        }

        public static Bits operator ^(Bits a, Bits b)
        {
            var bits = new bool[System.Math.Max(a.Length, b.Length)];
            Loop(0, bits.Length, i => bits[i] = a[i] ^ b[i]);
            return new Bits() { array = bits };
        }

        public static Bits Xor(params Bits[] bitss)
        {
            int max = bitss[0].Length;
            for (int i = 1; i < bitss.Length; i++)
                if (bitss[i].Length > max)
                    max = bitss[i].Length;
            var bits = new bool[max];
            Loop(0, max, i =>
            {
                bits[i] = false;
                int j = 0;
                for (; j < bitss.Length && !(bits[i] = bitss[j][i]); j++) ;
                for (; j < bitss.Length && (bits[i] = !bitss[j][i]); j++) ;
            });
            return new Bits() { array = bits };
        }

        public static Bits operator ~(Bits a)
        {
            var bits = new bool[a.Length];
            Loop(0, bits.Length, i => bits[i] = !a[i]);
            return new Bits() { array = bits };
        }

        public Bits Not(int length)
        {
            var bits = new bool[length];
            Loop(0, length, i => bits[i] = !this[i]);
            return new Bits() { array = bits };
        }


        public static Bits operator <<(Bits a, int i)
        {
            a.Shift(i);
            Bits bits = a.Clone();
            a.Shift(-i);
            return bits;
        }

        public static Bits operator >>(Bits a, int i)
        {
            a.Shift(-i);
            Bits bits = a.Clone();
            a.Shift(i);
            return bits;
        }

        public void Shift(int i)
        {
            if (internalLength > 0)
                shift += i;
        }


        public Bits Clone() => new Bits(this);


        public static Bits Random(int length)
        {
            Random random = new Random();
            bool[] array = new bool[length << 1];
            Loop(0, length - 1, i => array[i] = random.Next(2) == 0 ? false : true);
            array[length - 1] = true;
            return new Bits() { array = array, internalLength = length };
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = Length - 1; i >= 0; i--)
                sb.Append(this[i] ? "1" : "0");
            return sb.ToString();
        }
    }
}

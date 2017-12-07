namespace DLib.Collection.Long
{
    public class BitArray
    {
        System.Collections.BitArray[] arrays;

        public ulong Length { get; private set; }

        public BitArray(ulong length)
        {
            arrays = new System.Collections.BitArray[(int)System.Math.Ceiling(length / (double)int.MaxValue)];
            for (int i = 0; i < arrays.Length - 1; i++)
                arrays[i] = new System.Collections.BitArray(int.MaxValue);
            arrays[arrays.Length - 1] = new System.Collections.BitArray((int)(length % (int.MaxValue - 1)));
            Length = length;
        }

        public bool this[ulong i]
        {
            get => arrays[(int)(i / (double)int.MaxValue)][(int)(i % (int.MaxValue - 1))];
            set => arrays[(int)(i / (double)int.MaxValue)][(int)(i % (int.MaxValue - 1))] = value;
        }
    }
}

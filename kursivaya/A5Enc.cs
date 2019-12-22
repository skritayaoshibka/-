using System.Collections;

namespace kursivaya
{
    class A5Enc
    {
        private bool[] reg1 = new bool[19];
        private bool[] reg2 = new bool[22];
        private bool[] reg3 = new bool[23];
        private byte[] Key;



        public A5Enc(byte[] key)
        {
            Key = key;

            for (int i = 0; i < 19; i++)
                reg1[i] = false;
            for (int i = 0; i < 22; i++)
                reg2[i] = false;
            for (int i = 0; i < 23; i++)
                reg3[i] = false;
        }



        public void KeySetup(int[] frame)
        {
            for (int i = 0; i < 19; i++)
                reg1[i] = false;
            for (int i = 0; i < 22; i++)
                reg2[i] = false;
            for (int i = 0; i < 23; i++)
                reg3[i] = false;

            BitArray KeyBits = new BitArray(Key);
            BitArray FrameBits = new BitArray(frame);

            for (int i = 0; i < 64; i++)
            {
                clockall();
                reg1[0] = reg1[0] ^ KeyBits[i];
                reg2[0] = reg2[0] ^ KeyBits[i];
                reg3[0] = reg3[0] ^ KeyBits[i];
            }
            for (int i = 0; i < 22; i++)
            {
                clockall();
                reg1[0] = reg1[0] ^ FrameBits[i];
                reg2[0] = reg2[0] ^ FrameBits[i];
                reg3[0] = reg3[0] ^ FrameBits[i];
            }
            for (int i = 0; i < 100; i++)
            {
                clock();
            }
        }


        private void clock()
        {
            bool majority = ((reg1[8] & reg2[10]) | (reg1[8] & reg3[10]) | (reg2[10] & reg3[10]));

            if (reg1[8] == majority)
                clockone(reg1);

            if (reg2[10] == majority)
                clocktwo(reg2);

            if (reg3[10] == majority)
                clockthree(reg3);
        }


        private bool[] clockone(bool[] RegOne)
        {
            bool temp = false;
            for (int i = RegOne.Length - 1; i > 0; i--)
            {
                if (i == RegOne.Length - 1)
                    temp = RegOne[13] ^ RegOne[16] ^ RegOne[17] ^ RegOne[18];
                RegOne[i] = RegOne[i - 1];
                if (i == 1)
                    RegOne[0] = temp;
            }
            return RegOne;
        }

        private bool[] clocktwo(bool[] RegTwo)
        {
            bool temp = false;
            for (int i = RegTwo.Length - 1; i > 0; i--)
            {
                if (i == RegTwo.Length - 1)
                    temp = RegTwo[20] ^ RegTwo[21];
                RegTwo[i] = RegTwo[i - 1];
                if (i == 1)
                    RegTwo[0] = temp;
            }
            return RegTwo;
        }

        private bool[] clockthree(bool[] RegThree)
        {
            bool temp = false;
            for (int i = RegThree.Length - 1; i > 0; i--)
            {
                if (i == RegThree.Length - 1)
                    temp = RegThree[7] ^ RegThree[20] ^ RegThree[21] ^ RegThree[22];
                RegThree[i] = RegThree[i - 1];
                if (i == 1)
                    RegThree[0] = temp;
            }
            return RegThree;
        }

        private void clockall()
        {
            reg1 = clockone(reg1);
            reg2 = clocktwo(reg2);
            reg3 = clockthree(reg3);
        }



        public bool[] A5F(bool AsFrame, int frameLeng)
        {
            bool[] FirstPart = new bool[frameLeng];
            for (int i = 0; i < frameLeng; i++)
            {
                clock();
                FirstPart[i] = (reg1[18] ^ reg2[21] ^ reg3[22]);
            }
            return FirstPart;
        }

        public byte[] FromBoolToByte(bool[] key, bool lsb)
        {
            int bytes = key.Length / 8;
            if ((key.Length % 8) != 0) bytes++;
            byte[] arr2 = new byte[bytes];
            int bitIndex = 0, byteIndex = 0;
            for (int i = 0; i < key.Length; i++)
            {
                if (key[i])
                {
                    if (lsb)
                        arr2[byteIndex] |= (byte)(((byte)1) << (7 - bitIndex));
                    else
                        arr2[byteIndex] |= (byte)(((byte)1) << (bitIndex));
                }
                bitIndex++;
                if (bitIndex == 8)
                {
                    bitIndex = 0;
                    byteIndex++;
                }
            }
            return arr2;
        }


    }
}
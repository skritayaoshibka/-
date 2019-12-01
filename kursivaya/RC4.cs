using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursivaya
{
    class RC4
    {
        private byte[] SBlock = new byte[256];
        private byte[] Key;

        public void SetKey(byte[] key)
        {
            Key = key;
            InitializationSBlock();
        }

        void InitializationSBlock()
        {
            int L = Key.Length;
            for (int i = 0; i <= 255; i++)
                SBlock[i] = (byte)i;
            byte j = 0;
            for (int i = 0; i <= 255; i++)
            {
                j = (byte)(j + SBlock[i] + Key[i % L]);
                byte temp = SBlock[i];
                SBlock[i] = SBlock[j];
                SBlock[j] = temp;
            }
        }

        private byte GenerateK()
        {
            byte i = 0;
            byte j = 0;
            i++;
            j = (byte)(j + SBlock[i]);
            byte temp = SBlock[i];
            SBlock[i] = SBlock[j];
            SBlock[j] = temp;

            return SBlock[(SBlock[i] + SBlock[j]) % 256];
        }

        public byte[] Encrypt(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                byte test = GenerateK();
                data[i] = (byte)(data[i]^ test);
            }

            InitializationSBlock();

            return data;
        }

        public byte[] Decrypt(byte[] data)
        {
            return Encrypt(data);
        }
    }
}

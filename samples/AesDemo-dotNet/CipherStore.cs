using System;
using System.IO;

namespace AesInteropDemo
{

    public class CipherData
    {
        public byte[] InitializationVector;
        public String Passphrase;
        public byte[] Salt;
        public byte[] CipherText;

        public int GetLength()
        {
            return InitializationVector.Length +
                Passphrase.Length +
                Salt.Length +
                CipherText.Length;
        }
    }

    /// <summary>
    /// Summary description for CipherStore.
    /// </summary>
    public class CipherStore
    {
        public CipherStore() { }
        public CipherStore(string Filename) { _FileName = Filename; }

        String _FileName = "c:\\CipherText.bin";

        public String FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        public void Save(CipherData data)
        {
            File.Delete(FileName);
            using (FileStream fs = File.OpenWrite(_FileName))
            {
                fs.WriteByte((byte)data.InitializationVector.Length);
                fs.Write(data.InitializationVector, 0, data.InitializationVector.Length);

                byte[] b = System.Text.Encoding.ASCII.GetBytes(data.Passphrase);
                fs.WriteByte((byte)b.Length);
                fs.Write(b, 0, b.Length);
                
                fs.WriteByte((byte)data.Salt.Length);
                fs.Write(data.Salt, 0, data.Salt.Length);
                
                fs.Write(data.CipherText, 0, data.CipherText.Length);
            }
        }

        public CipherData Load()
        {
            CipherData data = new CipherData();
            using (FileStream fs = File.OpenRead(_FileName))
            {
                byte x = (byte) fs.ReadByte();
                byte[] b = new byte[x];
                fs.Read(b, 0, b.Length);
                data.InitializationVector = b;
                
                x = (byte) fs.ReadByte();
                b = new byte[x];
                fs.Read(b, 0, b.Length);
                data.Passphrase = System.Text.Encoding.ASCII.GetString(b);
                
                x = (byte) fs.ReadByte();
                b = new byte[x];
                fs.Read(b, 0, b.Length);
                data.Salt= b;

                b = new byte[(int)(fs.Length - fs.Position)];
                fs.Read(b, 0, b.Length);
                data.CipherText = b;
            }
            return data;
        }

    }
}

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Security.Cryptography;

namespace AesInteropDemo
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public partial class AesInteropForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// Class variables
        /// </summary>
        CipherStore _store = null;
        AesManaged _aesCipher;
        int _keyLengthInBits = 128;  // 192, 256

        string DefaultText = "What is life? It is the flash of a firefly in the night. It is the breath of a buffalo in the wintertime. It is the little shadow which runs across the grass and loses itself in the sunset.  --last words of Crowfoot, the Blackfoot chief, in 1890.";

        public AesInteropForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //

            txtInput.Text = DefaultText;
            this.txtInitVector.Text = this.txtInitVector.Text.ToUpper();
            this.tabControl1.SelectedIndex++;
            btnEncrypt.Select();
        }

        private CipherStore store
        {
            get
            {
                if (_store == null)
                    _store = new CipherStore(txtFilename.Text);
                return _store;
            }
            set
            {
                _store = value;
            }
        }

        private void btnLoad_Click(object sender, System.EventArgs e)
        {
            CipherData cd = store.Load();
            txtInput.Text = Convert.ToBase64String(cd.CipherText);
            txtPassPhrase.Text = cd.Passphrase;
            txtSalt.Text = ByteArrayToHexString(cd.Salt);
            txtInitVector.Text = ByteArrayToHexString(cd.InitializationVector);
            txtResult.Text = "";
            //aesCipher.IV = cd.InitializationVector;
            //System.Console.WriteLine("IV: {0}", FormatByteArray(aes.Iv));
            AppendStatus(System.String.Format("{0} bytes loaded from {1} ({2} crypto bytes).\r\n",
                                              cd.GetLength(),
                                              store.FileName,
                                              cd.CipherText.Length)
                         );
        }

        byte[] LastEncrypted;
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            if ((LastEncrypted != null) && (LastEncrypted.Length != 0))
            {
                CipherData cd = new CipherData();
                cd.Salt = this.Salt;
                cd.Passphrase = txtPassPhrase.Text;
                cd.InitializationVector = HexStringToByteArray(txtInitVector.Text);
                cd.CipherText = LastEncrypted;
                store.Save(cd);
                AppendStatus(System.String.Format("{0} bytes saved in {1} ({2} crypto bytes).\r\n",
                                                  cd.GetLength(),
                                                  store.FileName,
                                                  cd.CipherText.Length)
                             );

            }
        }

        private byte[] InitializationVector
        {
            get
            {
                return HexStringToByteArray(this.txtInitVector.Text);
            }

            set
            {
                this.txtInitVector.Text = ByteArrayToHexString(value);
            }
        }
        private byte[] Salt
        {
            get
            {
                return HexStringToByteArray(this.txtSalt.Text);
            }

            set
            {
                this.txtSalt.Text = ByteArrayToHexString(value);
            }
        }

        internal int KeyLengthInBytes
        {
            get
            {
                return this._keyLengthInBits / 8;
            }
        }

        internal static string ByteArrayToHexString(byte[] b)
        {
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            int i = 0;
            for (i = 0; i < b.Length; i++)
            {
                //if (i != 0 && i % 16 == 0) sb1.Append("\n");
                sb1.Append(System.String.Format("{0:X2}", b[i]));
            }
            return sb1.ToString().ToUpper();
        }

        internal static byte[] HexStringToByteArray(string s)
        {
            var r = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
            {
                r[i / 2] = (byte)Convert.ToInt32(s.Substring(i, 2), 16);
            }
            return r;
        }


        private void SetupAesCipher()
        {
            if (_aesCipher == null)
            {
                _aesCipher = new AesManaged();

                _aesCipher.KeySize = this._keyLengthInBits;

                // BlockSize: 128-bit == 16 bytes, which is what you get with the AES from IBM's JCE provider. 
                // 128-bit is the default for RijndaelManaged
                _aesCipher.BlockSize = 128;  // can also specify 256

                _aesCipher.Mode = CipherMode.CBC;
                _aesCipher.Padding = PaddingMode.PKCS7;
                //AesCipher.GenerateIV();
            }
            _aesCipher.IV = this.InitializationVector;
            _aesCipher.Key = GenerateKey();
            this.txtKey.Text = ByteArrayToHexString(_aesCipher.Key);
        }


        private byte[] GenerateKey()
        {
            // return new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1 };
            var rfc2898 =
                new System.Security.Cryptography.Rfc2898DeriveBytes(this.txtPassPhrase.Text, this.Salt, Int32.Parse(this.txtIterations.Text));
            return rfc2898.GetBytes(this.KeyLengthInBytes);
        }


        private void btnEncrypt_Click(object sender, System.EventArgs e)
        {
            txtResult.Text = "";
            if (this.txtInitVector.Text.Length != 32)
            {
                this.txtInitVector.BackColor = System.Drawing.Color.Bisque;
                AppendStatus("Please enter a 16-byte IV.\r\n");
                return;
            }

            AppendStatus("Encrypting...");
            SetupAesCipher();
            LastEncrypted = EncryptMessage(txtInput.Text.Trim());
            txtResult.Text = Convert.ToBase64String(LastEncrypted);
            AppendStatus("done.\r\n");
        }

        public byte[] EncryptMessage(string plainText)
        {
            byte[] b = System.Text.Encoding.ASCII.GetBytes(plainText);
            ICryptoTransform transform = _aesCipher.CreateEncryptor();
            byte[] cipherText = transform.TransformFinalBlock(b, 0, b.Length);
            return cipherText;
        }


        public byte[] DecryptMessage(byte[] cipherText)
        {
            ICryptoTransform transform = _aesCipher.CreateDecryptor();
            byte[] plainText = transform.TransformFinalBlock(cipherText, 0, cipherText.Length);
            return plainText;
        }

        public string DecryptMessageToString(byte[] cipherText)
        {
            try
            {
                byte[] plainText = DecryptMessage(cipherText);
                return System.Text.Encoding.ASCII.GetString(plainText);
            }
            catch
            {
                return "(garbled)";
            }
        }



        private void btnDecrypt_Click(object sender, System.EventArgs e)
        {
            txtResult.Text = "";
            AppendStatus("Decrypting...");
            try
            {
                SetupAesCipher();
                txtResult.Text = DecryptMessageToString(Convert.FromBase64String(txtInput.Text));
            }
            catch (System.Exception ex1)
            {
                AppendStatus("Exception! " + ex1 + "\r\n");
            }
            AppendStatus("done.\r\n");
        }

        private void btnReset_Click(object sender, System.EventArgs e)
        {
            txtInput.Text = DefaultText;
            txtResult.Text = "";
        }

        private void btnSwap_Click(object sender, System.EventArgs e)
        {
            txtInput.Text = txtResult.Text;
            txtResult.Text = "";
        }

        private void tabPage1_Enter(object sender, System.EventArgs e)
        {
            txtAboutDemo.Text = "AES Interop Demo, v1.0\r\n";
            txtAboutDemo.Text += ".NET Version";
            System.Reflection.Assembly caller = System.Reflection.Assembly.GetExecutingAssembly();
            string[] props1 = { 
                                                                  "System Name: " + System.Net.Dns.GetHostName(),                                                                 
                                                                  "Assembly: " + caller.GetName().Name + ", Version " + caller.GetName().Version ,
                                                                  "CLR: "  + System.Environment.Version.ToString(), 
                                                                  "Running on " + System.Environment.OSVersion.ToString(),
                        };

            txtAboutApp.Text = "";
            for (int i = 0; i < props1.Length; i++)
            {
                txtAboutApp.Text += props1[i] + "\r\n";

            }
        }

        private void AppendStatus(string Message)
        {
            txtStatus.Text += Message;
            int len = txtStatus.Text.Length;
            txtStatus.Select(len, 0);
            txtStatus.ScrollToCaret();
            txtStatus.Update();
        }

        private void txtFilename_TextChanged(object sender, System.EventArgs e)
        {
            // invalidate the store, as it points to a different file
            store = null;
        }

        private void Blink(TextBox tb)
        {
            tb.BackColor = System.Drawing.Color.Bisque;
            this.Update();
            System.Threading.Thread.Sleep(350);
            tb.BackColor = System.Drawing.Color.White;
        }

        private void Validate(object sender, KeyPressEventArgs e, int length)
        {
            TextBox tb = sender as TextBox;
            if (tb.BackColor == System.Drawing.Color.Bisque)
                tb.BackColor = System.Drawing.Color.White;
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar) && (e.KeyChar < 'A' || e.KeyChar > 'F') && (e.KeyChar < 'a' || e.KeyChar > 'f'))
            {
                Blink(tb);
                e.Handled = true; // suppress
            }
            else if (Char.IsLetterOrDigit(e.KeyChar) && tb.Text.Length >= length)
            {
                Blink(tb);
                e.Handled = true; // suppress
            }
            else
            {
                if (e.KeyChar >= 'a' && e.KeyChar <= 'f')
                    e.KeyChar -= (char)32;

            }
        }

        private void txtInitVector_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validate(sender, e, 32);
        }

        private void txtSalt_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validate(sender, e, 20);
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }


    }



}

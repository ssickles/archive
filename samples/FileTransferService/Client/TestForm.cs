/// Notice : Code written by Dimitris Papadimitriou - http://www.papadi.gr
/// Code is provided to be used freely but without any warranty of any kind
using System;
using System.Windows.Forms;

namespace Client
{
    public partial class TestForm : Form
    {
        #region Housekeeping

        public TestForm()
        {
            InitializeComponent();

            // create output folder, if does not exist
            if (!System.IO.Directory.Exists("Download")) System.IO.Directory.CreateDirectory("Download");
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = FileTextBox.Text;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileTextBox.Text = openFileDialog1.FileName;
            }
        }

        private void LogText(string text)
        {
            LogListBox.Items.Add(text);
            LogListBox.SelectedIndex = LogListBox.Items.Count - 1;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        #endregion

        #region Upload

        private void UploadButton_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                // get some info about the input file
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(FileTextBox.Text);

                // show start message
                LogText("Starting uploading " + fileInfo.Name);
                LogText("Size : " + fileInfo.Length);

                // open input stream
                using (System.IO.FileStream stream = new System.IO.FileStream(FileTextBox.Text, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    using (StreamWithProgress uploadStreamWithProgress = new StreamWithProgress(stream))
                    {
                        uploadStreamWithProgress.ProgressChanged += uploadStreamWithProgress_ProgressChanged;

                        // start service client
                        FileTransferClient.FileTransferServiceClient client = new FileTransferClient.FileTransferServiceClient();

                        // upload file
                        client.UploadFile(fileInfo.Name, fileInfo.Length, uploadStreamWithProgress);

                        LogText("Done!");

                        // close service client
                        client.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogText("Exception : " + ex.Message);
                if (ex.InnerException != null) LogText("Inner Exception : " + ex.InnerException.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        void uploadStreamWithProgress_ProgressChanged(object sender, StreamWithProgress.ProgressChangedEventArgs e)
        {
            if (e.Length!=0)
                progressBar1.Value = (int)(e.BytesRead * 100 / e.Length);
        }

        #endregion

        #region Download

        private void DownloadButton_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                // start service client
                FileTransferClient.FileTransferServiceClient client = new FileTransferClient.FileTransferServiceClient();

                LogText("Start");

                // kill target file, if already exists
                string filePath = System.IO.Path.Combine("Download", textBox1.Text);
                if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);

                // get stream from server
                System.IO.Stream inputStream;
                string fileName = textBox1.Text;
                long length = client.DownloadFile(ref fileName, out inputStream);
                
                // write server stream to disk
                using (System.IO.FileStream writeStream = new System.IO.FileStream(filePath, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write))
                {
                    int chunkSize = 2048;
                    byte[] buffer = new byte[chunkSize];

                    do
                    {
                        // read bytes from input stream
                        int bytesRead = inputStream.Read(buffer, 0, chunkSize);
                        if (bytesRead == 0) break;

                        // write bytes to output stream
                        writeStream.Write(buffer, 0, bytesRead);

                        // report progress from time to time
                        progressBar1.Value = (int)(writeStream.Position * 100 / length);
                    } while (true);

                    // report end of progress
                    LogText("Done!");

                    writeStream.Close();
                }

                // close service client
                client.Close();
            }
            catch (Exception ex)
            {
                LogText("Exception : " + ex.Message);
                if (ex.InnerException != null) LogText("Inner Exception : " + ex.InnerException.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion
    }
}
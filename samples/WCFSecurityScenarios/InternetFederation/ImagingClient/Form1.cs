// © 2005-2006 Michele Leroux Bustamante. All rights reserved 
// Blogs: www.thatindigogirl.com, www.dasblonde.net
// IDesign: www.idesign.net
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ImagingClient.localhost;
using System.IdentityModel.Selectors;


namespace ImagingClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            dlg.CheckFileExists = true;
            dlg.CheckPathExists = true;
            dlg.Multiselect = false;
            dlg.Filter = "JPEG(*.jpg)|*.jpg|BITMAP(*.bmp)|*.bmp|GIF(*.gif)|*.gif";

            DialogResult res = dlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                int pos = dlg.FileName.LastIndexOf("\\");
                string filename = dlg.FileName.Substring(pos + 1, dlg.FileName.Length - (pos + 1));
                this.labFilename.Text = dlg.FileName;
                this.pic.Image = Image.FromFile(dlg.FileName);
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            using (localhost.ImagingServicesContractSoapClient m_proxy = new ImagingServicesContractSoapClient())
            {

                System.IO.MemoryStream stm = null;

                try
                {

                    if (this.pic.Image == null)
                    {
                        throw new InvalidOperationException("Please select an image to upload!");
                    }

                    ImageInfo imgInfo = new ImageInfo();
                    imgInfo.Category = "Image";
                    imgInfo.DateStart= System.DateTime.Now;
                    imgInfo.DateEnd= null;
                    imgInfo.Title= this.labFilename.Text;
                    imgInfo.Url= this.labFilename.Text;

                    using (stm = new System.IO.MemoryStream())
                    {
                        this.pic.Image.Save(stm, this.pic.Image.RawFormat);
                        byte[] data = new byte[stm.Length];
                        data = stm.ToArray();
                        imgInfo.Data= data;
                    }

                    m_proxy.UploadImage(imgInfo);
                    MessageBox.Show("File uploaded!");
                }
                catch (UserCancellationException exUserCancelled)
                {
                    MessageBox.Show(exUserCancelled.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
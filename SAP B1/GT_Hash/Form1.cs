using GTCore.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GT_Hash
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            txtKey.Text = "2s5u8x/A?D(G+KbPeShVmYq3t6w9y$B&";
        }

        private void btnEncode_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtSource.Text))
            {
                txtTarget.Text = string.Empty;
                try
                {
                    txtTarget.Text = StringUtils.EncryptString(txtSource.Text);
                }
                catch { }
            }
        }

        private void btnDecode_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTarget.Text))
            {
                txtSource.Text = string.Empty;
                try
                {
                    txtSource.Text = StringUtils.DecryptString(txtTarget.Text);
                }
                catch { }
            }
        }
        
    }
}

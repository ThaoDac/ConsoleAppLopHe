using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form2 : Form
    {
        ErrorProvider errorProvider = new ErrorProvider();
        public Form2()
        {
            InitializeComponent();
        }

        private void tb_masv_Validated(object sender, EventArgs e)
        {
        }

        private void tb_masv_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(tb_masv.Text))
            {
                //e.Cancel = true;
                errorProvider.SetError(tb_masv, "Mã sinh viên không được để trống");
            }
            else
            {
                //e.Cancel = false;
                errorProvider.SetError(tb_masv, null);            }
        }

        private void tb_masv_TextChanged(object sender, EventArgs e)
        {
            if(tb_masv.Text.Length > 0)
            {
                btn_them.Enabled = true;
            }
            else 
            { 
                btn_them.Enabled = false; 
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace WindowsFormsApp
{
    public partial class Form2 : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["QLSV"].ConnectionString;
        ErrorProvider errorProvider = new ErrorProvider();
        DataView dv = new DataView();
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            string querySelect = "Select_TongHop1"; //stored

            try
            {
                using(SqlConnection conn = new SqlConnection(connectionString)) 
                {
                    using(SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = querySelect;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using(SqlDataAdapter adapter = new SqlDataAdapter())
                        {
                            adapter.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                adapter.Fill(dt);
                                if(dt.Rows.Count > 0)
                                {
                                    dv = dt.DefaultView;
                                    dgv_tblSV.AutoGenerateColumns = false;
                                    dgv_tblSV.DataSource = dv;
                                }
                                else
                                {
                                    MessageBox.Show("Khong ton tai ban ghi nao");
                                }
                            }
                        }
                    }
                } 
            }
            catch(Exception ex)
            {
                MessageBox.Show("Da xay ra loi");
            }
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

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dgv_tblSV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgv_tblSV.CurrentRow.Index;
            //tb_masv.Text = dgv_tblSV.Rows[index].Cells["masv"].Value.ToString(); //lay du lieu tu dgv
            //tb_masv.Text = dt.Rows[index]["sMaSV"].ToString();                   //lay du lieu tu DataTable
            tb_masv.Text = dv[index]["sMaSV"].ToString(); //lay du lieu tu goc nhin DataView
            tb_masv.ReadOnly = true;
            tb_hoten.Text = dv[index]["sHoTen"].ToString();
            dt_ngaysinh.Text = dv[index]["dNgaySinh"].ToString();
            if((bool)(dv[index]["bGioiTinh"]) == true)
            {
                rb_nam.Checked = true;
            }
            else
            {
                rb_nu.Checked = true;
            }
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            int index = dgv_tblSV.CurrentRow.Index;
            string masv = dv[index]["sMaSV"].ToString();
            try
            {
                DialogResult dialogResult = MessageBox.Show("Co chac muon xoa ma SV " + masv + " khong?",
                    "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if(dialogResult == DialogResult.Yes)
                {
                    //thuc hien xoa
                }
                else
                {
                    return;
                }
            }
            catch(Exception ex)
            {
                string strError = ex.Message;
                if (strError.Contains("FK_tblSINHVIEN_tblDIEM"))
                {
                    MessageBox.Show("Ma sinh vien " + masv + " da co phat sinh diem");
                }
                else if (ex is SqlException && strError.Contains("40"))
                {
                     
                }
                else if ()
                {

                }
            }
        }
    }
}

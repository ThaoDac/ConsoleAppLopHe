using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppLopHe
{
    public class Program
    {
        static void Main(string[] args)
        {
            string masv, ngaysinh;
            bool gioitinh;
            string connectionString = "Data Source = THINKPAD\\SQLEXPRESS; Initial Catalog = QLSV2; Integrated Security = True";

            Console.Write("Nhap ma sinh vien: ");
            masv = Console.ReadLine();

            Console.Write("Nhap ngay sinh: ");
            DateTime dateTime = Convert.ToDateTime(Console.ReadLine()); //  dd/MM/yyyy SQL: MM/d/yyyy yyyy/MM/dd
            ngaysinh = dateTime.ToString("yyyy/MM/dd");  // 13/12/2000

            Console.Write("Nhap gioi tinh: ");
            //phuong thuc chuyen doi "nu" -> 0 va "nam" -> 1
            gioitinh = IsGender(Console.ReadLine());

            bool i = ThemMoiSinhVien(connectionString, masv, ngaysinh, gioitinh);
            if (i)
            {
                //them moi thanh cong
                Console.Write("Thanh cong");
            }
            else
            {
                //them khong thanh cong
                Console.Write("khong Thanh cong");
            }
        }

        private static bool IsGender(string gender)
        {
            bool index;
            if (gender.ToLower() == "nam") //Nam NAM 
            {
                index = true;
            }
            else
            {
                index = false;
            }
            return index;
        }

        private static bool ThemMoiSinhVien(string connectionString, string maSV, string ngaySinh, bool gioiTinh)
        {
            string insertSQL = "Insert_tblSINHVIEN";
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                using(SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = insertSQL;
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cac tham so lien quan trong cmd
                    cmd.Parameters.Add("@maSV", SqlDbType.VarChar, 30).Value = maSV;
                    cmd.Parameters.AddWithValue("@maSV", maSV);
                    //them day du cac parameters

                    conn.Open();
                    int i = cmd.ExecuteNonQuery();
                    conn.Close();

                    return i > 0;
                }
            }
        }

        private static void HienThiDSSV(string connectionString)
        {
            string querySelect = "Select_tblSINHVIEN";
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                using(SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = querySelect;
                    cmd.CommandType= CommandType.StoredProcedure;

                    con.Open();
                    using(SqlDataReader reader = cmd.ExecuteReader()) 
                    {
                        if(reader.HasRows)
                        {
                            //doc va hien thi du lieu ra man hinh
                            while(reader.Read())
                            {
                                Console.WriteLine("{0}\t{1}\t{2}",
                                                    reader["sMaSV"],
                                                    reader["dNgaySinh"],
                                                    reader["bGioiTinh"]);
                            }
                        }
                        else
                        {
                            //khong co ban ghi nao ton tai
                        }
                    }
                    con.Close();
                }
            }
        }

        private static void HienThiDSSVngatketnoi(string connectionString)
        {
            string queryStr = "SELECT_tblSINHVIEN";
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = queryStr;
                    cmd.CommandType= CommandType.StoredProcedure;
                    using(SqlDataAdapter adapter = new SqlDataAdapter())
                    {
                        adapter.SelectCommand = cmd;
                        using(DataTable dt = new DataTable())
                        {
                            adapter.Fill(dt);
                            if(dt.Rows.Count > 0)
                            {
                                //khoi tao dataview
                                using(DataView dataView = new DataView(dt))
                                {
                                    dataView.RowFilter = "bGioiTinh = '1' ";
                                    dataView.Sort = "dNgaySinh ASC";

                                    //hien thi du lieu ra man hinh 
                                    foreach (DataRowView row in dataView)
                                    {
                                        Console.WriteLine("{0}\t{1}\t{3}",
                                                                row["sMaSV"],
                                                                row["dNgaySinh"],
                                                                row["bGioiTinh"]);
                                    }
                                }
                            }
                            else
                            {
                                //khong ton tai ban ghi nao
                            }
                        }
                    }
                }
            }
        }
    }
}

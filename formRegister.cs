using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ZstdSharp.Unsafe;

namespace uprak1
{
    public partial class formRegister : Form
    {
        public formRegister()
        {
            InitializeComponent();
        }

        private void btnMasuk_Click(object sender, EventArgs e)
        {
            string username = textUsn.Text;
            string name = textName.Text;
            string password = textPassword.Text;
            string konfirmasi = textKonfirmasi.Text;

            string koneksi = "server=localhost;Port=3306;Database=db_kopi_kasir;Uid=root;Password=;";

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("masukan username terlebih dahulu");
                return;
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("masukan nama terlebih dahulu");
                return;
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("masukan password terlebih dahulu");
                return;
            }
            if (password != konfirmasi)
            {
                MessageBox.Show("masukan password tidak sama ");
                return;
            }

            if(password.Length != 6)
            {
                MessageBox.Show("pasword harus lebih dari 6 angka");
                return;
            }
            bool adaHuruf = password.Any(char.IsLetter);
            bool adaAngka = password.Any(char.IsDigit);
            if (!adaAngka || !adaHuruf)
            {
                MessageBox.Show("HARUS mengandung huruf dan angka");
                return;
            }

            using (MySqlConnection kon = new MySqlConnection(koneksi))
            {
                try
                {
                    kon.Open();
                    string queri = "insert into tb_petugas(username,nama_petugas,password) values(@username,@nama_petugas,@password)";
                    using (MySqlCommand read = new MySqlCommand(queri,kon))
                    {
                        read.Parameters.AddWithValue("@username", username);
                        read.Parameters.AddWithValue("@nama_petugas", name);
                        read.Parameters.AddWithValue("@password", password);
                        read.ExecuteNonQuery();
                        MessageBox.Show("register berhasil");
                    }

                    formLogin formLogin=new formLogin();
                    formLogin.FormClosed += FormLogin_FormClosed;
                    formLogin.Show();
                    this.Hide();
                  
                    
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("username"))
                    {
                        MessageBox.Show("username sudah digunakan");
                    }
                }
            }

        }

        private void FormLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
    }
}

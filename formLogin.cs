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

namespace uprak1
{
    public partial class formLogin : Form
    {
        public formLogin()
        {
            InitializeComponent();
        }

        private void btnPindah_Click(object sender, EventArgs e)
        {
            string username = textUsn.Text;
            string password = textPassword.Text;
            string koneksi = "server=localhost;Port=3306;Database=db_kopi_kasir;Uid=root;Password=;";

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("username tidak boleh kosong");
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Password tidak boleh kosong");
                return;
            }

            using (MySqlConnection kon = new MySqlConnection(koneksi))
            {
                try
                {
                    kon.Open();
                    string query = "select * from tb_petugas where username=@username and password=@password";
                    using (MySqlCommand command = new MySqlCommand(query, kon)) {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);
                        using(MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                reader.Read();
                                int statusAktif = reader.GetInt32("status_aktif");
                                if (statusAktif == 0)
                                {
                                    MessageBox.Show("akun anda di non aktif");
                                    return;
                                }

                                string password_database = reader.GetString("password");
                                string nama_petugas = reader.GetString("nama_petugas");
                                string shift = reader.GetString("shift");
                                string role = reader.GetString("role");

                                if (password != password_database)
                                {
                                    MessageBox.Show("password tidak sama dengan database");
                                    return;
                                }
                                formDashboard formDasboard = new formDashboard();
                                formDasboard.FormClosed += FormDasboard_FormClosed;
                                formDasboard.Show();
                                this.Hide();


                            }
                            else
                            {
                                MessageBox.Show("username tidak ditemukan");
                            }

;
                        }
                        
                    }
                }
                catch(Exception ex) {

                    MessageBox.Show("terjadi kesalahan" + ex.Message);
                }
                
            }

        }

        private void FormDasboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            formRegister formRegister = new formRegister();
            formRegister.FormClosed += FormRegister_FormClosed;
            formRegister.Show();
            this.Hide();
        }

        private void FormRegister_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
    }
}

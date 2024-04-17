using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace EmployeeManagementSystem
{
    public partial class RegisterForm : Form
    {
        SqlConnection conn 
            = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Shino\Documents\employee.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True");
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void register_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void register_singIn_Click(object sender, EventArgs e)
        {
            Form1 loginForm = new Form1();
            loginForm.Show();
            this.Hide();
        }

        private void register_showPass_CheckedChanged(object sender, EventArgs e)
        {
            register_pass.PasswordChar = register_showPass.Checked ? '\0' : '*';
        }

        private void register_Btn_Click(object sender, EventArgs e)
        {
            if(register_username.Text == "" || register_pass.Text == "")
            {
                MessageBox.Show("Please fill all blank fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if(conn.State != ConnectionState.Open) 
                {
                    try
                    {
                        conn.Open();

                        string selectUsername = "SELECT COUNT(id) FROM users WHERE username = @user";
                        using (SqlCommand checkUser = new SqlCommand(selectUsername, conn))
                        {
                            checkUser.Parameters.AddWithValue("@user", register_username.Text.Trim());
                            int count = (int)checkUser.ExecuteScalar();

                            if (count >= 1) 
                            {
                                MessageBox.Show(register_username.Text.Trim() + " is already taken", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                DateTime today = DateTime.Today;

                                string insertData = "INSERT INTO users (username, password, date_register) VALUES(@username, @password, @dateReg)";
                                using (SqlCommand cmd = new SqlCommand(insertData, conn))
                                {
                                    cmd.Parameters.AddWithValue("@username", register_username.Text.Trim());
                                    cmd.Parameters.AddWithValue("@password", register_pass.Text.Trim());
                                    cmd.Parameters.AddWithValue("@dateReg", today);

                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Registered succesfuly!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    Form1 loginForm = new Form1();
                                    loginForm.Show();
                                    this.Hide();
                                }
                            }
                        }


                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Error: " + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }
    }
}

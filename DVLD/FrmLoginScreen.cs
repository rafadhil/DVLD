using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD
{
    public partial class FrmLoginScreen : Form
    {
        public FrmLoginScreen()
        {
            InitializeComponent();
        }

      
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar ;
        }

        private void FrmLoginScreen_Load(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
            UploadCredentials();
        }

        private void TriggerFocusToControls()
        {
            txtUsername.Focus();
            txtPassword.Focus();
            txtUsername.Focus();
        }
        private bool IsLoginCredentialsValid()
        {
            TriggerFocusToControls();
            return (String.IsNullOrEmpty(errorProvider1.GetError(txtUsername)) &&
                    String.IsNullOrEmpty(errorProvider1.GetError(txtPassword)));
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(!IsLoginCredentialsValid())
            {
                return;
            }

            if(!User.ValidateLoginCredentials(txtUsername.Text, txtPassword.Text))
            {
                MessageBox.Show("Incorrect Username/Password");
                return;
            }

            if(cbRememberMe.Checked)
            {
                RememberCredentials();
            }
            else
            {
                EraseCredentials();
            }

            Login();

        }


        private void Login()
        {
            UserSettings.LoggedInUser = User.GetUserByUsername(txtUsername.Text);
            
            FrmMain frmMain = new FrmMain();
            this.Hide();
            frmMain.Show();
            //this.Close();
        }
        private void RememberCredentials()
        {
            string filePath = @"C:\Users\rayan\source\repos\DVLD\RememberedLoginCredentials.txt";

            try
            {
                File.WriteAllText(filePath, $"{txtUsername.Text}\n{txtPassword.Text}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving credentials: {ex.Message}");
            }
        }


        private void EraseCredentials()
        {
            string filePath = @"C:\Users\rayan\source\repos\DVLD\RememberedLoginCredentials.txt";

            try
            {
                if (File.Exists(filePath))
                {
                    File.WriteAllText(filePath, string.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error erasing credentials: {ex.Message}");
            }
        }


        private void UploadCredentials()
        {
            string filePath = @"C:\Users\rayan\source\repos\DVLD\RememberedLoginCredentials.txt";

            try
            {
                if (File.Exists(filePath))
                {
                    string[] lines = File.ReadAllLines(filePath);
                    if (lines.Length >= 2)
                    {
                        txtUsername.Text = lines[0];
                        txtPassword.Text = lines[1];
                        cbRememberMe.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading credentials: {ex.Message}");
            }
        }



        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if(String.IsNullOrEmpty(txtPassword.Text))
            {
                errorProvider1.SetError(txtPassword, "Cannot be empty");
            }
            else
            {
                errorProvider1.SetError(txtPassword, "");
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtUsername.Text))
            {
                errorProvider1.SetError(txtUsername, "Cannot be empty");
            }
            else
            {
                errorProvider1.SetError(txtUsername, "");
            }
        }
    }
}

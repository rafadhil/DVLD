using BusinessLayer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
            UploadCredentialsFromWindowsRegistry();



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

            if(!User.ValidateLoginCredentials(txtUsername.Text, Hashing.ComputeSha256Hash(txtPassword.Text)))
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
                EraseCredentialsFromWindowsRegistry();
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
            string keyPath = @"HKEY_CURRENT_USER\Software\DVLD";

            try
            {
                Registry.SetValue(keyPath, "username", txtUsername.Text);
                Registry.SetValue(keyPath, "password", txtPassword.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving credentials: {ex.Message}");
            }
        }


        private void EraseCredentialsFromWindowsRegistry()
        {
            string keyPath = @"HKEY_CURRENT_USER\Software\DVLD";

            try
            {
                Registry.SetValue(keyPath, "username", "");
                Registry.SetValue(keyPath, "password", "");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error erasing credentials: {ex.Message}");
            }
        }


        private void UploadCredentialsFromWindowsRegistry()
        {
            string keyPath = @"HKEY_CURRENT_USER\Software\DVLD";

            try
            {
                string username = Registry.GetValue(keyPath, "username", null) as string;
                string password = Registry.GetValue(keyPath, "password", null) as string;

                if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    return;
                }

                txtUsername.Text = username;
                txtPassword.Text = password;
                cbRememberMe.Checked = true;
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

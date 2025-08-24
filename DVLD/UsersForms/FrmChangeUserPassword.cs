using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer;

namespace DVLD
{
    public partial class FrmChangeUserPassword : Form
    {
        private User ActiveUser;
        public FrmChangeUserPassword(int UserID)
        {
            InitializeComponent();
            ActiveUser = User.GetUserByUserID(UserID);
            if(ActiveUser == null)
            {
                MessageBox.Show("ERROR: Cannot find user with the provided ID");
                this.Close();
                return;
            }

        }

        private void FrmChangeUserPassword_Load(object sender, EventArgs e)
        {
            ctrDisplayPersonDetails1.LoadPersonInfo(ActiveUser.Person);
            txtCurrentPassword.UseSystemPasswordChar = true;
            txtNewPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true;
            LoadUserInfo();
        }

        private void LoadUserInfo()
        {
            txtUsername.Text = ActiveUser.Username;
            txtUserID.Text = ActiveUser.UserID.ToString();
            txtIsActive.Text = ActiveUser.IsActive ? "Yes" : "No";
        }

        private void TriggerFocusToControls()
        {
            txtCurrentPassword.Focus();
            txtNewPassword.Focus();
            txtConfirmPassword.Focus();
            txtCurrentPassword.Focus();
        }

        private bool CheckFieldsValidity()
        {
            TriggerFocusToControls();
            if(!String.IsNullOrEmpty(errorProvider1.GetError(txtCurrentPassword)))
            {
                MessageBox.Show($"Error in Current Password Field: {errorProvider1.GetError(txtCurrentPassword)}");
                return false;
            }

            if (!String.IsNullOrEmpty(errorProvider1.GetError(txtNewPassword)))
            {
                MessageBox.Show($"Error in New Password Field: {errorProvider1.GetError(txtNewPassword)}");
                return false;
            }

            if (!String.IsNullOrEmpty(errorProvider1.GetError(txtConfirmPassword)))
            {
                MessageBox.Show($"Error in Confirm Password Field: {errorProvider1.GetError(txtConfirmPassword)}");
                return false;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckFieldsValidity())
            {
                return;
            }

            if (!User.ValidateLoginCredentials(ActiveUser.Username, txtCurrentPassword.Text))
            {
                MessageBox.Show("Incorrect Password");
                return;
            }


            if (ActiveUser.ChangePassword(txtCurrentPassword.Text, txtNewPassword.Text))
            {
                MessageBox.Show("Password has been changed successfully");
            }
            else
            {
                MessageBox.Show("ERROR: Could not change password");
            }
            
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if(String.IsNullOrEmpty(txtCurrentPassword.Text))
            {
                errorProvider1.SetError(txtCurrentPassword, "Cannot be empty");
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword, "");
            }
        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtNewPassword.Text))
            {
                errorProvider1.SetError(txtNewPassword, "Cannot be empty");
            }
            else
            {
                errorProvider1.SetError(txtNewPassword, "");
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if(!txtConfirmPassword.Text.Equals(txtNewPassword.Text))
            {
                errorProvider1.SetError(txtConfirmPassword, "Passwords don't match");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, "");
            }
        }
    }
}

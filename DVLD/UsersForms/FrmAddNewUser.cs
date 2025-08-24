using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class FrmAddNewUser : Form
    {
        enum enMode { AddNew, Update}
        private Person SelectedPerson;
        private User ActiveUser;
        private enMode Mode;
        private bool IsUsersTabEnabled = false;

        public FrmAddNewUser()
        {
            InitializeComponent();
            Mode = enMode.AddNew;
        }
        public FrmAddNewUser(int UserID)
        {
            InitializeComponent();
            ActiveUser = User.GetUserByUserID(UserID);
            if(ActiveUser == null)
            {
                MessageBox.Show("ERROR: Couldn't find user with the provided ID");
                this.Close();
                return;
            }
            Mode = enMode.Update;
        }

        private void SetFormToAddNewMode()
        {
            ActiveUser = new User();
            ctrFindPerson1.Enabled = true;
            Mode = enMode.AddNew;
            lblMode.Text = "Add New User";
        }
        private void SetFormToUpdateMode()
        {
            ctrFindPerson1.LoadPersonInfo(ActiveUser.Person);
            ctrFindPerson1.DisableSearch();
            IsUsersTabEnabled = true;
            FillActiveUserInfo();
            btnSave.Enabled = true;
            Mode = enMode.Update;
            lblMode.Text = "Update User";
            this.Text = "Update User";
        }

        private void FillActiveUserInfo()
        {
            if (ActiveUser == null)
                return;

            lblUserID.Text = ActiveUser.UserID.ToString();
            txtUsername.Text = ActiveUser.Username;
        }
        private void ctrFindPerson1_OnPersonSelected(int obj)
        {
            SelectedPerson = Person.GetPersonByID(obj);
            if (SelectedPerson.HasUserAccount())
            {
                MessageBox.Show("The selected person already has an account, choose a different person");
                errorProvider1.SetError(btnNext, "Person already has an account");
                IsUsersTabEnabled = false;
                btnSave.Enabled = false;
                return;
            }
            else
            {
                errorProvider1.SetError(btnNext, "");
                ActiveUser.Person = Person.GetPersonByID(obj);
                IsUsersTabEnabled = true;
                btnSave.Enabled = true;
            }
        }



        private void btnNext_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(errorProvider1.GetError(btnNext)))
                tabControl1.SelectedTab = tabControl1.TabPages[1];
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tabControl1.TabPages[1] && !IsUsersTabEnabled)
            {
                e.Cancel = true;
                MessageBox.Show("Login info tab is disabled, you must complete the current tab requirements first.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtUsername_Validating(object sender, CancelEventArgs e)
        {
            if(String.IsNullOrEmpty(txtUsername.Text))
            {
                errorProvider1.SetError(txtUsername, "Cannot be empty");
            }
            else
            {
                errorProvider1.SetError(txtUsername, "");
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtPassword.Text))
            {
                errorProvider1.SetError(txtPassword, "Cannot be empty");
            }
            else
            {
                errorProvider1.SetError(txtPassword, "");
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if(!txtPassword.Text.ToString().Equals(txtConfirmPassword.Text.ToString()))
            {
                errorProvider1.SetError(txtConfirmPassword, "Passwords don't match");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, "");
            }
        }

        private void TriggerFocusToAllControls()
        {
            txtUsername.Focus();
            txtPassword.Focus();
            txtConfirmPassword.Focus();
            txtUsername.Focus();
        }

        private bool CheckAllFieldsValidity()
        {
            TriggerFocusToAllControls();

            if (!String.IsNullOrEmpty(errorProvider1.GetError(txtUsername)))
            {
                MessageBox.Show($"Username {errorProvider1.GetError(txtUsername)}");
                return false;
            }
            ActiveUser.Username = txtUsername.Text.ToString();

            if (!String.IsNullOrEmpty(errorProvider1.GetError(txtPassword)))
            {
                MessageBox.Show($"Password {errorProvider1.GetError(txtPassword)}");
                return false;
            }


            if (!String.IsNullOrEmpty(errorProvider1.GetError(txtConfirmPassword)))
            {
                MessageBox.Show(errorProvider1.GetError(txtConfirmPassword));
                return false;
            }

            ActiveUser.Password = txtPassword.Text.ToString();
            ActiveUser.IsActive = cbIsActive.Checked;
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(CheckAllFieldsValidity())
            {
                if(ActiveUser.Save())
                {
                    MessageBox.Show("User information has been succesfully saved");
                    SetFormToUpdateMode();
                }
                else
                {
                    MessageBox.Show("ERROR: Could not save user information");
                }
            }
   
        }

        private void FrmAddNewUser_Load(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true;
            if (Mode == enMode.Update)
            {
                SetFormToUpdateMode();
            }
            else
            {
                SetFormToAddNewMode();
            }
        }

        private void ctrFindPerson1_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tabPagePersonalInfo_Click(object sender, EventArgs e)
        {

        }
    }
}

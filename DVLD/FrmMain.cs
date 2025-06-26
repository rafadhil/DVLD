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
    public partial class FrmMain : Form
    {
        Form frmManagePeople = new FrmManagePeople();
        Form frmManageUsers = new FrmManageUsers();
        public FrmMain()
        {
            InitializeComponent();
        }



        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(frmManagePeople.IsDisposed)
            {
                frmManagePeople = new FrmManagePeople();
            }

            frmManagePeople.MdiParent = this;
            frmManagePeople.Show();
            
            
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {

        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmManageUsers.IsDisposed)
            {
                frmManageUsers = new FrmManageUsers();
            }

            frmManageUsers.MdiParent = this;
            frmManageUsers.Show();
        }

        private void curentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmDisplayUserDetails Frm = new FrmDisplayUserDetails(UserSettings.LoggedInUser.UserID);
            Frm.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmChangeUserPassword Frm = new FrmChangeUserPassword(UserSettings.LoggedInUser.UserID);
            Frm.ShowDialog();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show(
               "Are you sure you want to sign out?",
               "Confirm",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Warning,
               MessageBoxDefaultButton.Button2
               );

            if (Result == DialogResult.No)
            {
                return;
            }
            UserSettings.LoggedInUser = null;
            this.Hide();
            Form Frm = new FrmLoginScreen();
            Frm.ShowDialog();
            this.Close();
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new FrmManageApplicationTypes();
            form.ShowDialog();
        }

        private void applicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form Frm = new FrmManageTestTypes();
            Frm.ShowDialog();
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form Frm = new FrmAddNewLocalDrivingLicenseApplication();
            Frm.ShowDialog();
        }

        private void localDrivingLicenseAppplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form Frm = new FrmManageLocalDrivingLicenseApplications();
            Frm.ShowDialog();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}

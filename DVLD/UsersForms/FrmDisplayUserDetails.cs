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
    public partial class FrmDisplayUserDetails : Form
    {
        private User ActiveUser;
        public FrmDisplayUserDetails(int UserID)
        {
            InitializeComponent();
            ActiveUser = User.GetUserByUserID(UserID);
            if(ActiveUser == null)
            {
                MessageBox.Show("ERROR: Could not find user with the provided ID");
                this.Close();
            }

        }

        private void FrmDisplayUserDetails_Load(object sender, EventArgs e)
        {
            ctrDisplayPersonDetails1.LoadPersonInfo(ActiveUser.Person);
            txtUserID.Text = ActiveUser.UserID.ToString();
            txtUsername.Text = ActiveUser.Username;
            txtIsActive.Text = ActiveUser.IsActive ? "Yes" : "No";

        }
    }
}

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
    public partial class ctrShowUsers : UserControl
    {
        public ctrShowUsers()
        {
            InitializeComponent();
        }

        private void ctrShowUsers_Load(object sender, EventArgs e)
        {
            RefreshDGVUsers();
            cbFilterBy.SelectedItem = "None";
            dgvUsers.Columns["Full Name"].Width = 235;
        }

        private void SetNumberOfRecordsInDGV()
        {
            lblNumberOfRecords.Text = Convert.ToString(dgvUsers.AllowUserToAddRows ? dgvUsers.Rows.Count - 1 : dgvUsers.Rows.Count);
        }

        public void RefreshDGVUsers()
        {
            dgvUsers.DataSource = User.GetAllUsers();
            SetNumberOfRecordsInDGV();

        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((cbFilterBy.SelectedItem.ToString() == "Person ID" || cbFilterBy.SelectedItem.ToString() == "User ID") &&
                !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; 
            }
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            if (cbFilterBy.SelectedItem.ToString() == "Activity Status")
            {
                txtSearch.Visible = false;
                cbActivityStatus.Visible = true;
            }
            else if (cbFilterBy.SelectedItem.ToString() == "None")
            {
                txtSearch.Visible = false;
                cbActivityStatus.Visible = false;
                RefreshDGVUsers();
            }
            else
            {
                txtSearch.Visible = true;
                cbActivityStatus.Visible = false;
            }
        }

        private void cbActivityStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbActivityStatus.SelectedItem.ToString() == "All")
            {
                dgvUsers.DataSource = User.GetAllUsers();
            }
            else if (cbActivityStatus.SelectedItem.ToString() == "Active")
            {
                dgvUsers.DataSource = User.GetActiveUsers();
            }
            else if (cbActivityStatus.SelectedItem.ToString() == "Inactive")
            {
                dgvUsers.DataSource = User.GetInactiveUsers();
            }   


        }



        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(txtSearch.Text))
            {
                return;
            }


            switch(cbFilterBy.SelectedItem.ToString())
            {
                case "None":
                    RefreshDGVUsers();
                    return;

                case "User ID":
                    dgvUsers.DataSource = User.GetUsersByUserIDLike(txtSearch.Text);
                    break;

                case "Username":
                    dgvUsers.DataSource = User.GetUsersByUsernameLike(txtSearch.Text.ToString());
                    break;

                case "Person ID":
                    if (int.TryParse(txtSearch.Text, out int personId))
                    {
                        dgvUsers.DataSource = User.GetUsersByPersonIDLike(personId);
                    }
                    break;

                case "Full Name":
                    dgvUsers.DataSource = User.GetUsersByFullNameLike(txtSearch.Text.ToString());
                    break;

                default:
                    RefreshDGVUsers();
                    return;
            }
            SetNumberOfRecordsInDGV();

        }

        private void inProgressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void emailPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show(
                "Are you sure you want to delete this user?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2
                );

            if (Result == DialogResult.No)
            {
                return;
            }


            DataGridViewRow SelectedRow = dgvUsers.SelectedRows[0];
            int UserID = Convert.ToInt32(SelectedRow.Cells["UserID"].Value);
            if (User.DeleteUser(UserID))
            {
                MessageBox.Show("User has been deleted successfully");
                RefreshDGVUsers();
            }
            else
            {
                MessageBox.Show("Error: Cannot delete user");
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DataGridViewRow SelectedRow = dgvUsers.SelectedRows[0];
            int UserID = Convert.ToInt32(SelectedRow.Cells["UserID"].Value);
            Form frm = new FrmChangeUserPassword(UserID);
            frm.ShowDialog();
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow SelectedRow = dgvUsers.SelectedRows[0];
            int UserID = Convert.ToInt32(SelectedRow.Cells["UserID"].Value);

            Form frm = new FrmAddNewUser(UserID);
            frm.ShowDialog();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow SelectedRow = dgvUsers.SelectedRows[0];
            int UserID = Convert.ToInt32(SelectedRow.Cells["UserID"].Value);
            FrmDisplayUserDetails Frm = new FrmDisplayUserDetails(UserID);
            Frm.ShowDialog();
        }

        private void addPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new FrmAddNewUser();
            frm.ShowDialog();
        }

        private void cmsDGV_Opening(object sender, CancelEventArgs e)
        {
            if (dgvUsers.SelectedRows.Count < 1)
            {
                e.Cancel = true;
                return;
            }

        }
    }
}

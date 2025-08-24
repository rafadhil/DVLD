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
    public partial class ctrShowPeople : UserControl
    {
        public ctrShowPeople()
        {
            InitializeComponent();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
            dgvPersons.DataSource = Person.GetAllPersons();
            dgvPersons.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            cbCountries.DataSource = Country.GetAllCountries();
            cbCountries.DisplayMember = "CountryName";
            cbCountries.ValueMember = "CountryName";
            cbCountries.SelectedValue = "Saudi Arabia";
            SetNumberOfRecordsInDGV();
        }


        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.SelectedItem.ToString() == "None")
                RefreshDGV();

            txtSearch.Visible = cbFilterBy.SelectedItem.ToString() != "None" &&
                cbFilterBy.SelectedItem.ToString() != "Gender" &&
                cbFilterBy.SelectedItem.ToString() != "Nationality";

            cbGenders.Visible = cbFilterBy.SelectedItem.ToString() == "Gender";
            cbCountries.Visible = cbFilterBy.SelectedItem.ToString() == "Nationality";
            
                txtSearch.Text = "";
        }

        private void SetNumberOfRecordsInDGV()
        {
            lblNumberOfRecords.Text = Convert.ToString(dgvPersons.AllowUserToAddRows? dgvPersons.Rows.Count - 1 : dgvPersons.Rows.Count);
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
               // Allow control keys like Backspace 
                if ((cbFilterBy.SelectedItem.ToString() == "Person ID" ||
                    cbFilterBy.SelectedItem.ToString() == "Phone") &&
                (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)))
                {
                    e.Handled = true; // Stop the character from being entered
                    return;
                }     

        }

        private void cbGenders_SelectedIndexChanged(object sender, EventArgs e)
        {
            Person.enGender Gender = cbGenders.SelectedItem.ToString() == "Females" ? Person.enGender.Female : Person.enGender.Male;
            dgvPersons.DataSource = Person.GetPersonsByGender(Gender);
            SetNumberOfRecordsInDGV();

        }

        private void cbCountries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.SelectedItem.ToString() == "Nationality")
            {
                dgvPersons.DataSource = Person.GetPersonsByCountryID(cbCountries.SelectedIndex + 1);
                SetNumberOfRecordsInDGV();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                errorProvider1.SetError(txtSearch, "Cannot be empty");
                return;
            }
            else
                errorProvider1.Clear();

            switch (cbFilterBy.Text.ToString())
            {
                case "Person ID":
                    dgvPersons.DataSource = Person.GetPersonsByPersonIDLike(Convert.ToInt32(txtSearch.Text));
                    SetNumberOfRecordsInDGV();
                    break;

                case "National Number":
                    dgvPersons.DataSource = Person.GetPersonsByNationalNumberLike(txtSearch.Text);
                    SetNumberOfRecordsInDGV();
                    break;

                case "First Name":
                    dgvPersons.DataSource = Person.GetPersonsByFirstNameLike(txtSearch.Text);
                    SetNumberOfRecordsInDGV();
                    break;

                case "Second Name":
                    dgvPersons.DataSource = Person.GetPersonsBySecondNameLike(txtSearch.Text);
                    SetNumberOfRecordsInDGV();
                    break;

                case "Third Name":
                    dgvPersons.DataSource = Person.GetPersonsByThirdNameLike(txtSearch.Text);
                    SetNumberOfRecordsInDGV();
                    break;

                case "Last Name":
                    dgvPersons.DataSource = Person.GetPersonsByLastNameLike(txtSearch.Text);
                    SetNumberOfRecordsInDGV();
                    break;

                case "Phone":
                    dgvPersons.DataSource = Person.GetPersonsByPhoneLike(txtSearch.Text);
                    SetNumberOfRecordsInDGV();
                    break;

                case "Email":
                    dgvPersons.DataSource = Person.GetPersonsByEmailLike(txtSearch.Text);
                    SetNumberOfRecordsInDGV();
                    break;

            }


        }

        public void RefreshDGV()
        {
            dgvPersons.DataSource = Person.GetAllPersons();
            SetNumberOfRecordsInDGV();
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow SelectedRow = dgvPersons.SelectedRows[0];
            int PersonID = Convert.ToInt32(SelectedRow.Cells["PersonID"].Value);
            Form Frm = new FrmAddNewUpdate(PersonID);
            Frm.ShowDialog();
            RefreshDGV();
        }

        private void addPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form Frm = new FrmAddNewUpdate();
            Frm.ShowDialog();
            RefreshDGV();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show(
                "Are you sure you want to delete this person?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2
                );

            if(Result == DialogResult.No)
            {
                return;
            }
            

            DataGridViewRow SelectedRow = dgvPersons.SelectedRows[0];
            int PersonID = Convert.ToInt32(SelectedRow.Cells["PersonID"].Value);
            if(Person.DeletePerson(PersonID))
            {
                MessageBox.Show("Person has been deleted successfully");
                RefreshDGV();
            }
            else
            {
                MessageBox.Show("Error: Cannot delete person");
            }
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow SelectedRow = dgvPersons.SelectedRows[0];
            int PersonID = Convert.ToInt32(SelectedRow.Cells["PersonID"].Value);
            Form frm = new FrmShowPersonDetails(PersonID);
            frm.ShowDialog();
        }

        private void cmsDGV_Opening(object sender, CancelEventArgs e)
        {
            if (dgvPersons.SelectedRows.Count < 1)
            {
                e.Cancel = true;
                return;
            }

        }

    } 
}

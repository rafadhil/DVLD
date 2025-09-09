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
    public partial class ctrFindPerson : UserControl
    {
        private Person ActivePerson;

        public event Action<int> OnPersonSelected;
        protected virtual void PersonSelected(int PersonID)
        {
            Action<int> handler = OnPersonSelected;
            if (handler != null)
            {
                handler(PersonID); // Raise the event with the parameter
            }
        }

        public ctrFindPerson()
        {
            InitializeComponent();
        }

        private void ctrFindPerson_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            cbFilterBy.SelectedItem = "National Number";
        }

        public void DisableSearch()
        {
            groupBox1.Enabled = false;
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Text = "";
        }

     

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.SelectedItem.ToString() == "Person ID" &&
                (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)))
            {
                e.Handled = true; // Stop the character from being entered
                return;
            }
        }

   
        public void LoadPersonInfo(Person person)
        {
            ActivePerson = person;
            ctrDisplayPersonDetails1.LoadPersonInfo(ActivePerson);
            cbFilterBy.SelectedItem = "Person ID";
            txtSearch.Text = ActivePerson.PersonID.ToString();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(txtSearch.Text))
            {
                errorProvider1.SetError(txtSearch, "Cannot be empty");
                return;
            }
                
            
            errorProvider1.SetError(txtSearch, "");
            
            if(cbFilterBy.SelectedItem.ToString() == "Person ID")
            {
                if (int.TryParse(txtSearch.Text, out int personId))
                {
                    ActivePerson = Person.GetPersonByID(personId);
                }
            }
            else
            {
                ActivePerson = Person.GetPersonByNationalNumber(txtSearch.Text.ToString());
            }

            if(ActivePerson == null)
            {
                MessageBox.Show("Person not found");
                return;
            }

            ctrDisplayPersonDetails1.LoadPersonInfo(ActivePerson);

            if(OnPersonSelected != null)
            {
                OnPersonSelected(ActivePerson.PersonID);
            }

        }

        private void txtSearch_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearch.Text))
            {
                errorProvider1.SetError(txtSearch, "Cannot be empty");
            }
            else
            {
                errorProvider1.SetError(txtSearch, "");
            }
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            FrmAddNewUpdate frm = new FrmAddNewUpdate();
            frm.DataBack += FrmFindPerson_DataBack;
            frm.ShowDialog();
        }

        private void ctrDisplayPersonDetails1_Load(object sender, EventArgs e)
        {

        }
        private void FrmFindPerson_DataBack(object sender, int ID)
        {
            LoadPersonInfo(Person.GetPersonByID(ID));

            if (OnPersonSelected != null)
            {
                OnPersonSelected(ActivePerson.PersonID);
            }

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

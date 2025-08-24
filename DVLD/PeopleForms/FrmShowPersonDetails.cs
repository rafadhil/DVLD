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
    public partial class FrmShowPersonDetails : Form
    {
        private Person ActivePerson;
        public FrmShowPersonDetails(int PersonID)
        {
            InitializeComponent();
            ActivePerson = Person.GetPersonByID(PersonID);
            if(ActivePerson == null)
            {
                MessageBox.Show("Error: unable to find person with provided ID, closing the form..");
                this.Close();
            }

            ctrDisplayPersonDetails1.LoadPersonInfo(ActivePerson);
        }

        private void FrmShowPersonDetails_Load(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}

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
namespace DVLD.Licenses
{
    public partial class FrmShowPersonLicenseHistory : Form
    {
        Person ActivePerson;
        public FrmShowPersonLicenseHistory(Person Person)
        {
            InitializeComponent();
            if (Person == null)
            {
                MessageBox.Show("ERROR: Could not find person", "Failure");
                this.Close();
                return;
            }

            ActivePerson = Person;
        }

        private void FrmShowPersonLicenseHistory_Load(object sender, EventArgs e)
        {
            ctrFindPerson1.LoadPersonInfo(ActivePerson);
            ctrFindPerson1.DisableSearch();
            ctrShowPersonLicenseHistory1.LoadInfo(ActivePerson);
        }
    }
}

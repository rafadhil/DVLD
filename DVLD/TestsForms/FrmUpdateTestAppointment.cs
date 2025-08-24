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
namespace DVLD.TestsForms
{
    public partial class FrmUpdateTestAppointment : Form
    {
        TestAppointment ActiveAppointment;
        public FrmUpdateTestAppointment(TestAppointment appointment )
        {
            InitializeComponent();
            if (appointment == null)
            {
                MessageBox.Show("ERROR: Appointment not found");
                this.Close();
                return;
            }

            ActiveAppointment = appointment;
        }

        private void FrmUpdateTestAppointment_Load(object sender, EventArgs e)
        {
            dtpDate.MinDate = DateTime.Now;
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
    }
}

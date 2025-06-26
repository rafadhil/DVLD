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
    public partial class FrmUpdateApplicationType : Form
    {
       private ApplicationType ActiveType;
        public FrmUpdateApplicationType(int ApplicationTypeID)
        {
            InitializeComponent();
            ActiveType = ApplicationType.GetApplicationTypeByID(ApplicationTypeID);
            if(ActiveType == null)
            {
                MessageBox.Show("ERROR: Could not find an application type with the provided id");
                return;
            }
        }

        private void FrmUpdateApplicationType_Load(object sender, EventArgs e)
        {
            LoadApplicationInfo();
        }

        private void LoadApplicationInfo()
        {
            lblID.Text = ActiveType.TypeID.ToString();
            txtTitle.Text = ActiveType.TypeTitle;
            txtFees.Text = ActiveType.TypeFees.ToString();
        }

        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(txtTitle.Text))
            {
                MessageBox.Show("Choose a title first");
                return;
            }

            ActiveType.TypeTitle = txtTitle.Text;

            if (String.IsNullOrEmpty(txtFees.Text))
            {
                MessageBox.Show("Choose an amount first");
                return;
            }

            ActiveType.TypeFees = Convert.ToDecimal(txtFees.Text);

            if(ActiveType.Save())
            {
                MessageBox.Show("Saved successfully");
            }
            else
            {
                MessageBox.Show("ERROR: Save failed");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

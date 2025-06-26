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
    public partial class FrmUpdateTestType : Form
    {
        private TestType ActiveType;
        public FrmUpdateTestType(int TypeID)
        {
            InitializeComponent();
            ActiveType = TestType.GetTestTypeByID(TypeID);
            if(ActiveType == null)
            {
                MessageBox.Show("ERROR: Cannot find test type with the provided id");
                this.Close();
            }

        }

        private void FrmUpdateTestType_Load(object sender, EventArgs e)
        {
            lblID.Text = ActiveType.TypeID.ToString();
            txtTitle.Text = ActiveType.TypeTitle;
            txtDescription.Text = ActiveType.TypeDescription;
            txtFees.Text = ActiveType.TypeFees.ToString();
        }

        

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtTitle.Text))
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


            if (String.IsNullOrEmpty(txtDescription.Text))
            {
                MessageBox.Show("Description needs to be provided");
                return;
            }

            ActiveType.TypeDescription = txtDescription.Text;

            if (ActiveType.Save())
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

        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}

using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class ctrDisplayPersonDetails : UserControl
    {
        private Person ActivePerson;
        public ctrDisplayPersonDetails()
        {
            InitializeComponent();
        }

        public void LoadPersonInfo(Person Person)
        {
            if (DesignMode) return;

            lblEditPersonInfo.Enabled = true;
            ActivePerson = Person;
            lblPersonID.Text = Convert.ToString(Person.PersonID);
            lblName.Text = $"{Person.FirstName} {Person.SecondName} {Person.ThirdName} {Person.LastName}";
            lblGender.Text = Convert.ToString(Person.Gender);
            lblEmail.Text = Convert.ToString(Person.Email);
            lblPhoneNumber.Text = Convert.ToString(Person.Phone);
            lblNationalNumber.Text = Convert.ToString(Person.NationalNo);
            lblAddress.Text = Convert.ToString(Person.Address);

            int Day = Person.DateOfBirth.Day;
            int Month = Person.DateOfBirth.Month;
            int Year = Person.DateOfBirth.Year;

            lblDateOfBirth.Text = $"{Day}/{Month}/{Year}";
            lblCountry.Text = Convert.ToString(Country.GetCountryByID(Person.NationalityCountryID));

            if (Person.ImagePath != null)
                SetImageToPictureBox(Person.ImagePath);
            else
                SetDefaultImage();

            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new FrmAddNewUpdate(ActivePerson.PersonID);
            frm.ShowDialog();
            LoadPersonInfo(Person.GetPersonByID(ActivePerson.PersonID));
        }

        private void SetDefaultImage()
        {
            pbPersonalPhoto.Image = ActivePerson.Gender == Person.enGender.Female ? Properties.Resources.woman : Properties.Resources.man;
        }

        public void SetImageToPictureBox(string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                MessageBox.Show("Image file not found: " + imagePath);
                SetDefaultImage();
                return;
            }

            try
            {
                // Open the file in a FileStream and copy it to a MemoryStream to avoid locking
                using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        fs.CopyTo(ms);
                        ms.Position = 0; // Reset the position to the start of the stream

                        // Create a Bitmap from the memory stream
                        using (Bitmap tempBitmap = new Bitmap(ms))
                        {
                            // Create a clone of the bitmap so the stream can be disposed
                            pbPersonalPhoto.Image = new Bitmap(tempBitmap);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading image: " + ex.Message);
            }
        }

        private void gb1_Enter(object sender, EventArgs e)
        {

        }
    }
}

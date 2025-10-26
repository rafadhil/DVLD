using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class FrmAddNewUpdate : Form

    {
        private enum enMode {AddNew = 1, Update = 2};
        private Person ActivePerson;
        private String NationalNumber;
        private String ActivePictureFilePath;
        private enMode Mode;

        public delegate void DataBackEventHandler(object sender, int PersonID);
        public event DataBackEventHandler DataBack;

        public FrmAddNewUpdate(int PersonID)
        {
            InitializeComponent();
            ActivePerson = Person.GetPersonByID(PersonID);

            if(ActivePerson == null)
            {
                MessageBox.Show("Error: Person with ID has not been found.");
                this.Close();
            }

            Mode = enMode.Update;
            
        }

        public FrmAddNewUpdate()
        {
            Mode = enMode.AddNew;
            ActivePerson = new Person();
            InitializeComponent();
        }

        private void SetFormToAddNewMode()
        {
            lblMode.Text = "Add new person ";
            this.Text = "Add new person menu";
            lblPersonID.Text = "?";
            Mode = enMode.AddNew;
        }

        private void SetFormToUpdateMode()
        {
            lblMode.Text = "Update Person Info";
            this.Text = "Update person menu";
            lblPersonID.Text = ActivePerson.PersonID.ToString();
            FillPersonInfo();
            NationalNumber = ActivePerson.NationalNo;
            Mode = enMode.Update;
            if (ActivePerson.ImagePath != null)
            {
                UploadAndSetPersonalPicture(ActivePerson.ImagePath);
                ActivePictureFilePath = ActivePerson.ImagePath;
            }
           

            txtAddress.Text = ActivePerson.Address;
        }

        private bool FillPersonInfo()
        {
            if(ActivePerson != null)
            {
                txtFirstName.Text = ActivePerson.FirstName;
                txtSecondName.Text = ActivePerson.SecondName;
                txtThirdName.Text = ActivePerson.ThirdName;
                txtLastName.Text = ActivePerson.LastName;
                txtEmail.Text = ActivePerson.Email;
                dtpDateOfBirth.Value = ActivePerson.DateOfBirth;
                txtNationalNo.Text = ActivePerson.NationalNo;
                txtPhone.Text = ActivePerson.Phone;
                cbCountries.SelectedIndex = ActivePerson.NationalityCountryID - 1;
                if(ActivePerson.Gender == Person.enGender.Female)
                {
                    rbFemale.Checked = true;
                }
                else
                    rbMale.Checked = true;
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool CheckAllFieldsValidity()
        {
            TriggerFocusToAllControls();
            String ErrorMessage = errorProvider1.GetError(txtFirstName);

            if(!String.IsNullOrEmpty(ErrorMessage))
            {
                MessageBox.Show($"First name {ErrorMessage}");
                return false;
            }

            ActivePerson.FirstName = txtFirstName.Text;

            ErrorMessage = errorProvider1.GetError(txtSecondName);

            if (!String.IsNullOrEmpty(ErrorMessage))
            {
                MessageBox.Show($"Second name {ErrorMessage}");
                return false;
            }

            ActivePerson.SecondName = txtSecondName.Text;

            ErrorMessage = errorProvider1.GetError(txtThirdName);

            if (!String.IsNullOrEmpty(ErrorMessage))
            {
                MessageBox.Show($"Third name {ErrorMessage}");
                return false;
            }

            ActivePerson.ThirdName = txtThirdName.Text;


            ErrorMessage = errorProvider1.GetError(txtLastName);

            if (!String.IsNullOrEmpty(ErrorMessage))
            {
                MessageBox.Show($"Last name {ErrorMessage}");
                return false;
            }
            ActivePerson.LastName = txtLastName.Text;

            ErrorMessage = errorProvider1.GetError(txtNationalNo);

            if (!String.IsNullOrEmpty(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage);
                return false;
            }
            ActivePerson.NationalNo = txtNationalNo.Text;

            ErrorMessage = errorProvider1.GetError(txtEmail);

            if (!String.IsNullOrEmpty(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage);
                return false;
            }

            ActivePerson.Email = String.IsNullOrWhiteSpace(txtEmail.Text)? null: txtEmail.Text;

            if (!rbFemale.Checked && !rbMale.Checked)
            {
                MessageBox.Show("Select a gender");
                return false;
            }


            ErrorMessage = errorProvider1.GetError(txtAddress);

            if (!String.IsNullOrEmpty(ErrorMessage))
            {
                MessageBox.Show("Address " + ErrorMessage);
                return false;
            }

            ActivePerson.Address = txtAddress.Text;

            ActivePerson.Phone = txtPhone.Text;
            ActivePerson.Gender = rbMale.Checked? Person.enGender.Male : Person.enGender.Female;
            ActivePerson.DateOfBirth = dtpDateOfBirth.Value;
            ActivePerson.NationalityCountryID = cbCountries.SelectedIndex + 1;

            if(!String.IsNullOrEmpty(ActivePictureFilePath) &&
                !String.IsNullOrEmpty(ActivePerson.ImagePath) &&
                !String.Equals(ActivePerson.ImagePath, ActivePictureFilePath)) // Person has changed their picture
            {
                //MessageBox.Show("Case 1: Person has changed their picture");
                String OldImagePath = ActivePerson.ImagePath; 
                ActivePerson.ImagePath = CopyNewPictureToPicturesDirectory();
                UploadAndSetPersonalPicture(ActivePerson.ImagePath);
                DeleteImageFile(OldImagePath);
            }
            else if(!String.IsNullOrEmpty(ActivePictureFilePath) &&
                !String.Equals(ActivePerson.ImagePath, ActivePictureFilePath))               //Person has set a new picture
            {
                //MessageBox.Show("Case 2: Person has set a new picture");

                ActivePerson.ImagePath = CopyNewPictureToPicturesDirectory();
                UploadAndSetPersonalPicture(ActivePerson.ImagePath);
            }
            else if(!String.IsNullOrEmpty(ActivePerson.ImagePath) &&
                !String.Equals(ActivePerson.ImagePath, ActivePictureFilePath))              //Person has deleted their picture
            {
               // MessageBox.Show("Case 3: Person deleted their picture");
                String OldImagePath = ActivePerson.ImagePath;
                ActivePerson.ImagePath = null;
                UploadAndSetPersonalPicture(ActivePerson.ImagePath);
                DeleteImageFile(OldImagePath);
            }

            UploadAndSetPersonalPicture(ActivePerson.ImagePath);

            return true;
        }

        private String CopyNewPictureToPicturesDirectory(string DestFilePath = @"C:\DVLD-People-Images")
        {
            Directory.CreateDirectory(DestFilePath);
            string Extension = Path.GetExtension(ActivePictureFilePath);
            String NewFileName = Guid.NewGuid().ToString() + Extension;
            string destinationFile = Path.Combine(DestFilePath, NewFileName);
            File.Copy(ActivePictureFilePath, destinationFile);

            return destinationFile;
        }
        private void TriggerFocusToAllControls()
        {
            txtFirstName.Focus();
            txtSecondName.Focus();
            txtThirdName.Focus();
            txtLastName.Focus();
            txtNationalNo.Focus();
            txtEmail.Focus();
            txtPhone.Focus();
            txtAddress.Focus();
            txtFirstName.Focus();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if(CheckAllFieldsValidity())
            {
                if (ActivePerson.Save())
                {
                    MessageBox.Show("Data saved successfully");
                    CheckAndSwitchFormState();
                    DataBack?.Invoke(this, int.Parse(lblPersonID.Text));
                }
                else
                {
                    MessageBox.Show("An error has occured, data save failed");
                }
            }
            
            
        }

        private void CheckAndSwitchFormState()
        {
            if (Mode == enMode.AddNew)
            {
                SetFormToUpdateMode();
            }
        }
        private void FrmAddNewUpdate_Load(object sender, EventArgs e)
        {
            dtpDateOfBirth.MaxDate = DateTime.Today.AddYears(-18);
            dtpDateOfBirth.MaxDate = dtpDateOfBirth.MaxDate.AddDays(10);
            cbCountries.DataSource = Country.GetAllCountries();
            cbCountries.DisplayMember = "CountryName";
            cbCountries.ValueMember = "CountryName";
            cbCountries.SelectedValue = "Saudi Arabia";

            if (Mode == enMode.AddNew)
            {
                SetFormToAddNewMode();
            }
            else
                SetFormToUpdateMode();
           
        }

        private void Texts_Validating(object sender, CancelEventArgs e)
        {
            TextBox current = (TextBox)sender;

            if (string.IsNullOrEmpty(current.Text))
            {
                errorProvider1.SetError(current, "Cannot be empty");
            }
            else if (current.Text.Any(char.IsDigit))
            {
                errorProvider1.SetError(current, "Cannot contain a number");
            }
            else
            {
                errorProvider1.SetError(current, "");
            }
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (Person.IsPersonExist(Convert.ToString(txtNationalNo.Text)) && txtNationalNo.Text != ActivePerson.NationalNo)
            {
                errorProvider1.SetError(txtNationalNo, "Person with national number already exists");
            }
            else if (string.IsNullOrEmpty(txtNationalNo.Text))
            {
                errorProvider1.SetError(txtNationalNo, "National Number Cannot be empty");
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, "");
            }
        }



        private void DeleteImageFile(string imagePath)
        {
            // Make sure PictureBox is not using the image anymore
            if (pbPersonalPhoto.Image != null)
            {
                pbPersonalPhoto.Image.Dispose();
                pbPersonalPhoto.Image = null;
            }

            // Now delete the file
            if (File.Exists(imagePath))
            {
                try
                {
                    File.Delete(imagePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to delete image: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("File not found.");
            }
        }
  

        private void SetDefaultPersonalPicture()
        {
            pbPersonalPhoto.Image = rbFemale.Checked ? Properties.Resources.woman : Properties.Resources.man;
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if(rbMale.Checked && String.IsNullOrEmpty(ActivePictureFilePath) && String.IsNullOrEmpty(ActivePerson.ImagePath))
            {
                SetDefaultPersonalPicture();
            }
        }


        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFemale.Checked && String.IsNullOrEmpty(ActivePictureFilePath) && String.IsNullOrEmpty(ActivePerson.ImagePath))
            {
                SetDefaultPersonalPicture();
            }
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            if(IsValidEmail(Convert.ToString(txtEmail.Text)))
            {
                errorProvider1.SetError(txtEmail, "");
            }
            else
            {
                errorProvider1.SetError(txtEmail, "Incorrect email format");
            }

            if (String.IsNullOrWhiteSpace(Convert.ToString(txtEmail.Text)))
            {
                errorProvider1.SetError(txtEmail, "");
            }
        }

       private bool IsValidEmail(String email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtAddress_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtAddress.Text))
            {
                errorProvider1.SetError(txtAddress, "Cannot be empty");
            }
            else
            {
                errorProvider1.SetError(txtAddress, "");
            }
        }

        private void lnkUpload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Title = "Select an image";
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ActivePictureFilePath = openFileDialog1.FileName;
                UploadAndSetPersonalPicture(ActivePictureFilePath);
            }
        }

        private void UploadAndSetPersonalPicture(string imagePath)
        {

            if (pbPersonalPhoto.Image != null)
            {
                pbPersonalPhoto.Image.Dispose();
                pbPersonalPhoto.Image = null;
            }
            if (String.IsNullOrEmpty(imagePath))
            {
                SetDefaultPersonalPicture();
                return;
            }

            if (!File.Exists(imagePath))
            {
                MessageBox.Show("Personal picture file not found.");
                SetDefaultPersonalPicture();
                return;
            }

            // Dispose the previous image if any


            // Load the image into memory to avoid locking the file
            using (var fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                Image temp = Image.FromStream(fs);
                pbPersonalPhoto.Image = new Bitmap(temp); // Copy image to memory
                ActivePictureFilePath = imagePath;
                temp.Dispose(); // Free the original stream-based image
            }
        }

        private void lnkRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ActivePictureFilePath = null;
            UploadAndSetPersonalPicture(null);
        }

        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPhone.Text))
            {
                errorProvider1.SetError(txtPhone, "Cannot be empty");
            }
            else
            {
                errorProvider1.SetError(txtPhone, "");
            }
        }
    }
}

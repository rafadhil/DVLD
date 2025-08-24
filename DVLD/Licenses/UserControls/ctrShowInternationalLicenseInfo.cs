using BusinessLayer;
using BusinessLayer.Licenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses.UserControls
{
    public partial class ctrShowInternationalLicenseInfo : UserControl
    {
        InternationalLicense ActiveLicense;
        public ctrShowInternationalLicenseInfo()
        {
            InitializeComponent();
        }

        private void ShowInternationalLicenseInfo_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
        }

        public void LoadInfo(InternationalLicense DrivingLicense)
        {
            if(DrivingLicense == null)
            {
                MessageBox.Show("ERROR: Could not find license");
                return;
            }


            lblName.Text = DrivingLicense.OriginalApplication.ApplicantInfo.GetFullName();
            lblLicenseID.Text = DrivingLicense.LicenseID.ToString();
            lblNationalNo.Text = DrivingLicense.OriginalApplication.ApplicantInfo.NationalNo;
            lblGender.Text = DrivingLicense.OriginalApplication.ApplicantInfo.Gender.ToString();
            lblIssueDate.Text = DrivingLicense.IssueDate.ToString("dd/MM/yyyy");
            lblExpirationDate.Text = DrivingLicense.ExpirationDate.ToString("dd/MM/yyyy");
            lblIsActive.Text = DrivingLicense.IsActive ? "Yes" : "No";
            lblApplicationID.Text = DrivingLicense.OriginalApplication.ApplicationID.ToString();
            lblDateOfBirth.Text = DrivingLicense.OriginalApplication.ApplicantInfo.DateOfBirth.ToString("dd/MM/yyyy");
            lblDriverID.Text = DrivingLicense.DriverInfo.DriverID.ToString();

            ActiveLicense = DrivingLicense;
            _UploadPersonalPicture();
        }

        public void ClearSelection()
        {
            ActiveLicense = null;
            lblName.Text = "[??]";
            lblLicenseID.Text = "[??]";
            lblNationalNo.Text = "[??]";
            lblGender.Text = "[??]";
            lblIssueDate.Text = "[??]";
            lblExpirationDate.Text = "[??]";
            lblIsActive.Text = "[??]";
            lblDateOfBirth.Text = "[??]";
            lblDriverID.Text = "[??]";
            pbPersonalPhoto.Image = Properties.Resources.man;
        }

        private void _UploadPersonalPicture()
        {
            string PictureImagePath = ActiveLicense.OriginalApplication.ApplicantInfo.ImagePath;

            if (String.IsNullOrEmpty(PictureImagePath))
            {
                _SetDefaultImage();
                return;
            }

            if (System.IO.File.Exists(PictureImagePath))
            {
                pbPersonalPhoto.Image = Image.FromFile(PictureImagePath);
                return;
            }

            MessageBox.Show("Image file not found!");
            _SetDefaultImage();
        }

        private void _SetDefaultImage()
        {
            pbPersonalPhoto.Image = ActiveLicense.OriginalApplication.ApplicantInfo.Gender == Person.enGender.Female ? Properties.Resources.woman : Properties.Resources.man;
        }
    }
}

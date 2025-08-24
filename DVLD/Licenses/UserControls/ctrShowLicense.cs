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
using DVLD.Properties;

namespace DVLD.Licenses.UserControls
{
    public partial class ctrShowLicense : UserControl
    {
        BusinessLayer.License ActiveLicense;
        public ctrShowLicense()
        {
            InitializeComponent();
        }

        private void ctrShowLicense_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
        }

        public void LoadInfo(BusinessLayer.License DrivingLicense)
        {
            if (DrivingLicense == null)
                return;

            ActiveLicense = DrivingLicense;

            lblClass.Text = DrivingLicense.LicenseClass.ClassName;
            lblName.Text = DrivingLicense.Application.ApplicantInfo.GetFullName();
            lblLicenseID.Text = DrivingLicense.LicenseID.ToString();
            lblNationalNo.Text = DrivingLicense.Application.ApplicantInfo.NationalNo;
            lblGender.Text = DrivingLicense.Application.ApplicantInfo.Gender.ToString();
            lblIssueDate.Text = DrivingLicense.IssueDate.ToString("dd/MM/yyyy");
            lblExpirationDate.Text = DrivingLicense.ExpirationDate.ToString("dd/MM/yyyy");
            _SetIssueReason();
            lblNotes.Text = DrivingLicense.Notes;
            lblIsActive.Text = DrivingLicense.IsActive ? "Yes" : "No";
            lblDateOfBirth.Text = DrivingLicense.Application.ApplicantInfo.DateOfBirth.ToString("dd/MM/yyyy");
            lblDriverID.Text = DrivingLicense.DriverInfo.DriverID.ToString();
            lblIsDetained.Text = DrivingLicense.IsDetained() ? "Yes" : "No";
            _UploadPersonalPicture();
        }

        public void ClearSelection()
        {
            ActiveLicense = null;
            lblClass.Text = "[??]";
            lblName.Text = "[??]";
            lblLicenseID.Text = "[??]";
            lblNationalNo.Text = "[??]";
            lblGender.Text = "[??]";
            lblIssueDate.Text = "[??]";
            lblExpirationDate.Text = "[??]";
            lblIssueReason.Text = "[??]";
            lblNotes.Text = "[??]";
            lblIsActive.Text = "[??]";
            lblDateOfBirth.Text = "[??]";
            lblDriverID.Text = "[??]";
            lblIsDetained.Text = "[??]";
            pbPersonalPhoto.Image = Properties.Resources.man;
        }

        private void _UploadPersonalPicture()
        {
            string PictureImagePath = ActiveLicense.Application.ApplicantInfo.ImagePath;

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
            pbPersonalPhoto.Image = ActiveLicense.Application.ApplicantInfo.Gender == Person.enGender.Female ? Properties.Resources.woman : Properties.Resources.man;
        }

        private void _SetIssueReason()
        {
            switch((Common.enLicenseIssueReason) ActiveLicense.IssueReason)
            {
                case Common.enLicenseIssueReason.FirstTime:
                    lblIssueReason.Text = "Issuing for first time";
                    break;
                case Common.enLicenseIssueReason.ReplacementForLostLicense:
                    lblIssueReason.Text = "Replacement for a lost license";
                    break;
                case Common.enLicenseIssueReason.ReplacementForDamagedLicense:
                    lblIssueReason.Text = "Replacement for damaged license";
                    break;
                case Common.enLicenseIssueReason.Renew:
                    lblIssueReason.Text = "Renewing an expired license";
                    break;

            }
        }

        public bool IsSelectedLicenseActive()
        {
            return ActiveLicense.IsActive;
        }

        private void gb1_Enter(object sender, EventArgs e)
        {

        }


    }
}

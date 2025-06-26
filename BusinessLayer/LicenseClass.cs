using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
namespace BusinessLayer
{
    public class LicenseClass
    {
        public int LicenseClassID { private set; get; }
        public String ClassName { private set; get; }
        public String ClassDescription { private set; get; }
        public byte MinimumAllowedAge { private set; get; }
        public byte DefaultValidityLength { private set; get; }
        public decimal ClassFees { private set; get; }

        private LicenseClass(int LicenseClassID, String ClassName, String ClassDescription,
            byte MinimumAllowedAge, byte DefaultValidityLength, decimal ClassFees)
        {
            this.LicenseClassID = LicenseClassID;
            this.ClassName = ClassName;
            this.ClassDescription = ClassDescription;
            this.MinimumAllowedAge = MinimumAllowedAge;
            this.DefaultValidityLength = DefaultValidityLength;
            this.ClassFees = ClassFees;
        }

        public static LicenseClass GetClassByID(int LicenseClassID)
        {
            byte MinimumAllowedAge = 0, DefaultValidityLength = 0;
            String ClassName = "", ClassDescription = "";
            decimal ClassFees = 0.0M;

            if(LicenseClassData.GetClassByID(LicenseClassID, ref ClassName, ref ClassDescription,
                ref MinimumAllowedAge, ref DefaultValidityLength,
                ref ClassFees))
            {
                return new LicenseClass(LicenseClassID, ClassName, ClassDescription, MinimumAllowedAge,
                    DefaultValidityLength, ClassFees);
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetLicenseClassses()
        {
            return LicenseClassData.GetLicenseClassses();
        }
    }
}

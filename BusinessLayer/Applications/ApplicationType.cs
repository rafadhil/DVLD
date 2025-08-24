using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
namespace BusinessLayer
{
    public class ApplicationType
    {
        public int TypeID;
        public String TypeTitle;
        public decimal TypeFees;

        private ApplicationType(int TypeID, String TypeTitle, decimal TypeFees)
        {
            this.TypeID = TypeID;
            this.TypeTitle = TypeTitle;
            this.TypeFees = TypeFees;
        }

        public static DataTable GetApplicationTypes()
        {
            return ApplicationTypeData.GetApplicationTypes();
        }

        public static ApplicationType GetApplicationTypeByID(int TypeID)
        {
            String TypeTitle = "";
            decimal TypeFees = 0.0M;

            if (ApplicationTypeData.GetApplicationTypeByID(TypeID, ref TypeTitle, ref TypeFees))
            {
                return new ApplicationType(TypeID, TypeTitle, TypeFees);
            }
            else
            {
                return null;
            }
        }

        public bool Save()
        {
            if(ApplicationTypeData.UpdateApplicationType(this.TypeID, this.TypeTitle, this.TypeFees))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

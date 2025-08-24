using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class TestType
    {
        public int TypeID;
        public String TypeTitle;
        public String TypeDescription;
        public decimal TypeFees;

        private TestType(int TypeID, String TypeTitle, String TypeDescription, decimal TypeFees)
        {
            this.TypeID = TypeID;
            this.TypeTitle = TypeTitle;
            this.TypeDescription = TypeDescription;
            this.TypeFees = TypeFees;
        }

        public static DataTable GetTestTypes()
        {
            return TestTypeData.GetTestTypes();
        }

        public static TestType GetTestTypeByID(int TypeID)
        {
            String TypeTitle = "", TypeDescription = "";
            decimal TypeFees = 0.0M;

            if (TestTypeData.GetTestTypeByID(TypeID, ref TypeTitle, ref TypeDescription, ref TypeFees))
            {
                return new TestType(TypeID, TypeTitle, TypeDescription, TypeFees);
            }
            else
            {
                return null;
            }
        }

        public bool Save()
        {
            if (TestTypeData.UpdateTestType(this.TypeID, this.TypeTitle, this.TypeDescription, this.TypeFees))
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

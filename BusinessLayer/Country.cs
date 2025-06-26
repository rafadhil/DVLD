using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Country
    {
        public readonly int CountryID;
        public readonly String CountryName;


        public static DataTable GetAllCountries()
        {
            return CountryData.GetAllCountries();
        }

        public static String GetCountryByID(int CountryID)
        {
            return CountryData.GetCountryByID(CountryID);
        }
    }
}

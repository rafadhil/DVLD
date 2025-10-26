using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DataAccessLayer
{
    internal class DataLayerSettings
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;
        public static string EventViewerSourceName = ConfigurationManager.AppSettings["EventViewerSourceName"];
    }
}

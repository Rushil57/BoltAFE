using System.Web.Configuration;

namespace BoltAFE.Helpers
{
    public class GlobalClass
    {
        public static string GlobalConnectionString_Master = WebConfigurationManager.AppSettings["sqldb_connection_Master"];
    }
}
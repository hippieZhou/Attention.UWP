using System.Configuration;

namespace Attention.Core.Services
{
    public static class SQLiteDataService
    {
        private static string GetConnectionString()
        {
            // Attempt to get the connection string from a config file
            // Learn more about specifying the connection string in a config file at https://docs.microsoft.com/en-us/dotnet/api/system.configuration.configurationmanager?view=netframework-4.7.2
            var conStr = ConfigurationManager.ConnectionStrings["MyAppConnectionString"]?.ConnectionString;

            if (!string.IsNullOrWhiteSpace(conStr))
            {
                return conStr;
            }
            else
            {
                return @"Data Source=*server*\*instance*;Initial Catalog=*dbname*;Integrated Security=SSPI";
            }
        }
    }
}

namespace Classes
{
    public class Globals
    {
        public static string ApplicationProductId = string.Empty;
        public static string ApplicationUserRole = "User";
        public static string ApplicationOrganizationName = "";

        // set database connection string based on Debug or Release mode. Connection string used in program.cs 
#if DEBUG
        // Development database product id: 5000A-400-3000B
        public static string ApplicationDatabaseConnectionString = "data source=(localdb)\\MSSQLLocalDB;initial catalog=SampleAPI.Database;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework;";
#else
        public static string ApplicationDatabaseConnectionString = "data source=(localdb)\\MSSQLLocalDB;initial catalog=SampleAPI.Database;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework;";
#endif        
    }
}
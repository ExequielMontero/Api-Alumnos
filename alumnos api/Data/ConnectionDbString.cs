namespace alumnos_api.Data
{
    public class ConnectionDbString
    {
        private static readonly string connectionDb = "Data Source=app.db"; 

        public static string ConnectionStringBd()
        {
            return connectionDb; 
        }
    }

}

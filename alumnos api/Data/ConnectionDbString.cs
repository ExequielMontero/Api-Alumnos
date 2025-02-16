namespace alumnos_api.Data
{
    public class ConnectionDbString
    {
        private static readonly string connectionDb = "Data Source=C:/Users/pc/source/repos/ConsoleApp1/Api Web alumnos/alumnos api/app.db"; 

        public static string ConnectionStringBd()
        {
            return connectionDb; 
        }
    }

}

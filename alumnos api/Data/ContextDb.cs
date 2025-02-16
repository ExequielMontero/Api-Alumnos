using alumnos_api.Models;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Threading.Tasks;

public class ContextDb
{
    private readonly string _connectionString;

    public ContextDb(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InsertScriptDb()
    {
        var commands = new[]
        {
        @"
        CREATE TABLE IF NOT EXISTS Alumnos (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            DNI INTEGER NOT NULL,
            Nombre TEXT NOT NULL,
            Fecha_Nacimiento DATE
        );",
        @"
        INSERT INTO Alumnos (DNI, Nombre, Fecha_Nacimiento) VALUES 
        (35843243, 'Sebastian', '1990-01-01'),
        (35327489, 'Esteban', '1990-01-01'),
        (43323432, 'Luisa', '2000-01-05');"
    };

        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                foreach (var cmdText in commands)
                {
                    using (var command = new SqliteCommand(cmdText, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }

            Console.WriteLine("Base de datos creada y datos insertados correctamente.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
            Console.Error.WriteLine("StackTrace: " + ex.StackTrace);
            throw new Exception("Error en la ejecución del script: " + ex.Message);
        }
    }


    public async Task<List<Alumnos>> GetAlumn()
    {
        string query = "select * from Alumnos";
        List<Alumnos> list = new List<Alumnos>();
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var comando = new SqliteCommand(query, connection))
                {
                    using (var reader = await comando.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var alumnos = new Alumnos()
                            {
                                Id = reader.GetInt32(0),         // Mapea la columna Id
                                DNI = reader.GetInt32(1),        // Mapea la columna DNI
                                Nombre = reader.GetString(2),    // Mapea la columna Nombre
                                Fecha_Nacimiento = reader.GetDateTime(3) // Mapea la columna Fecha_Nacimiento
                            };

                            if (alumnos != null)
                            {
                                list.Add(alumnos);
                            }
                        }
                    }
                }
            }
            return list;
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
        
    }
}
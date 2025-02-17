using alumnos_api.Models;
using alumnos_api.Models.Dtos;
using Microsoft.Data.Sqlite;
using SQLitePCL;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;

public class ContextDb
{
    private readonly string _connectionString;

    public ContextDb(string connectionString)
    {
        _connectionString = connectionString;
    }

    //Script crear DB
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
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

    }


    public async Task<bool> InsertAlumn(AlumnosDto alumno)
    {
        try
        {
            // Validación básica de datos
            if (alumno.DNI <= 0 || string.IsNullOrWhiteSpace(alumno.Nombre))
            {
                throw new ArgumentException("DNI o Nombre inválidos");
            }

            // Usar parámetros para evitar inyección SQL
            string query = @"
            INSERT INTO Alumnos (DNI, Nombre, Fecha_Nacimiento)
            VALUES (@dni, @nombre, @fechaNacimiento)
        ";

            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqliteCommand(query, connection))
                {
                    // Agregar parámetros con valores
                    command.Parameters.AddWithValue("@dni", alumno.DNI);
                    command.Parameters.AddWithValue("@nombre", alumno.Nombre);

                    // Manejar fechas nulas
                    if (alumno.Fecha_Nacimiento.HasValue)
                    {
                        command.Parameters.AddWithValue("@fechaNacimiento", alumno.Fecha_Nacimiento.Value.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@fechaNacimiento", DBNull.Value);
                    }

                    await command.ExecuteNonQueryAsync();
                    return true;
                }
            }
        }
        catch (Exception e)
        {
            // Mejorar el mensaje de error para debugging
            throw new Exception($"Error en InsertAlumn: {e.Message}", e);
        }
    }

    public async Task<bool> ActualizarAlumno(Alumnos alumno)
    {
        try
        {
            // Validar datos básicos
            if (alumno.Id <= 0 || string.IsNullOrWhiteSpace(alumno.Nombre) || alumno.DNI <= 0)
            {
                throw new ArgumentException("Datos del alumno inválidos");
            }

            string query = @"
            UPDATE Alumnos 
            SET 
                DNI = @dni,
                Nombre = @nombre,
                Fecha_Nacimiento = @fechaNacimiento
            WHERE 
                Id = @id
        ";

            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", alumno.Id);
                    command.Parameters.AddWithValue("@dni", alumno.DNI);
                    command.Parameters.AddWithValue("@nombre", alumno.Nombre);

                    if (alumno.Fecha_Nacimiento.HasValue)
                    {
                        command.Parameters.AddWithValue("@fechaNacimiento", alumno.Fecha_Nacimiento.Value.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@fechaNacimiento", DBNull.Value);
                    }

                    int filasAfectadas = await command.ExecuteNonQueryAsync();
                    return filasAfectadas > 0;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al actualizar el alumno: {ex.Message}");
        }
    }

    public async Task<bool> ActualizarNombreAlumno(int id, string nombre)
    {
        string query = "UPDATE Alumnos SET Nombre = @nombre WHERE Id = @id";

        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqliteCommand(query, connection))
            {
                // Usar parámetros
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@nombre", nombre);
            
                int filasAfectadas = await command.ExecuteNonQueryAsync();
                return filasAfectadas > 0;
            }
        }
    }
    public async Task<bool> ActualizarDniAlumno(int id, int dni)
    {
        string query = "UPDATE Alumnos SET DNI = @dni WHERE Id = @id";

        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqliteCommand(query, connection))
            {
                // Usar parámetros
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@dni", dni);

                int filasAfectadas = await command.ExecuteNonQueryAsync();
                return filasAfectadas > 0;
            }
        }
    }
}
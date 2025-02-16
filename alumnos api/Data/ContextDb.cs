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
        string query = @"
        CREATE TABLE IF NOT EXISTS Usuarios (
            Nombre TEXT PRIMARY KEY NOT NULL,
            Clave TEXT NOT NULL
        );

        CREATE TABLE IF NOT EXISTS Roles (
            Nombre TEXT PRIMARY KEY NOT NULL
        );

        CREATE TABLE IF NOT EXISTS Usuarios_Roles (
            Nombre_Usuario TEXT NOT NULL,
            Nombre_Rol TEXT NOT NULL,
            CONSTRAINT UQ_Usuarios_Roles UNIQUE(Nombre_Usuario, Nombre_Rol)
        );

        ALTER TABLE Usuarios_Roles
        ADD CONSTRAINT FK_Usuarios_Roles_Roles
        FOREIGN KEY(Nombre_Rol) REFERENCES Roles(Nombre);

        ALTER TABLE Usuarios_Roles
        ADD CONSTRAINT FK_Usuarios_Roles_Usuarios
        FOREIGN KEY(Nombre_Usuario) REFERENCES Usuarios(Nombre);

        CREATE TABLE IF NOT EXISTS Personas (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            DNI INTEGER NOT NULL,
            Nombre TEXT NOT NULL,
            Fecha_Nacimiento DATE
        );

        INSERT INTO Personas(DNI, Nombre, Fecha_Nacimiento) VALUES 
        (35843243, 'Sebastian', '1990-01-01'),
        (35327489, 'Esteban', '1990-01-01'),
        (43323432, 'Luisa', '2000-01-05');

        INSERT INTO Usuarios(Nombre, Clave) VALUES
        ('Admin', '123'),
        ('Eduardo', 'eduardo'),
        ('Estefania', 'estefania');

        INSERT INTO Roles(Nombre) VALUES
        ('Admin'),
        ('Encuestador'),
        ('Supervisor');

        INSERT INTO Usuarios_Roles(Nombre_Usuario, Nombre_Rol) VALUES
        ('Eduardo', 'Admin'),
        ('Estefania', 'Supervisor'),
        ('Eduardo', 'Encuestador');
        ";

        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqliteCommand(query, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }

            Console.WriteLine("Base de datos creada y datos insertados correctamente.");
        }
        catch (Exception ex)
        {
            throw new Exception("Error en la ejecución del script: " + ex.Message);
        }
    }
}
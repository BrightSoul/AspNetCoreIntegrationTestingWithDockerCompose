using MyWebApiApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApiApp.Services
{
    public class SqlServerValuesRepository : IValuesRepository
    {
        private static string connectionString = @"Server=db,1433;Database=master;User=sa;Password=Password1;";

        static SqlServerValuesRepository()
        {
            //Crea la tabella se non esiste
            var task = CreateTableIfNotExists();
            task.ConfigureAwait(false);
            task.Wait();
        }
        

        public async Task<IEnumerable<(int Id, string Value)>> GetAll()
        {
            return await ExecuteQuery();
        }

        public async Task Create(string value)
        {
            await ExecuteCommand("INSERT INTO [dbo].[Values] ([Value]) VALUES (@value)", 
                new SqlParameter("value", value));
        }

        public async Task Update(int id, string value)
        {
            await ExecuteCommand("UPDATE [dbo].[Values] SET [Value] = @value WHERE [Id] = @id",
                new SqlParameter("id", value),
                new SqlParameter("value", value));
        }

        public async Task Remove(int id)
        {
            await ExecuteCommand("DELETE FROM [dbo].[Values] Where [Id] = @id",
                new SqlParameter("id", id));
        }

        private static async Task CreateTableIfNotExists()
        {
            await ExecuteCommand("IF OBJECT_ID(N'dbo.Values', N'U') IS NULL BEGIN CREATE TABLE [dbo].[Values] ([Id] int IDENTITY(1,1) PRIMARY KEY, [Value] TEXT NOT NULL); END;");
        }

        private static async Task ExecuteCommand(string command, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = command;
                    if (parameters != null) {
                        foreach (var p in parameters) {
                            cmd.Parameters.Add(p);
                        }
                    }
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        private static async Task<List<(int Id, string Value)>> ExecuteQuery()
        {
            var results = new List<(int Id, string Value)>();
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM [dbo].[Values]";
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            results.Add(((int)reader["Id"], (string)reader["Value"]));
                        }
                    }
                }
            }
            return results;
        }
    }
}

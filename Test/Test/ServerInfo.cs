using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;

namespace Test
{
    /// <summary>
    /// Contains information about the size of the server.
    /// Has methods for getting lists of databases, their sizes  
    /// </summary>
    public class ServerInfo
    {
        // Ip address: for example 127.0.0.1
        public readonly string ServerName;
        // Capacity of server in GB
        public readonly double Size;
        // Free memory in GB
        public double SizeLeft { get; set; }
        // String for Npgsql connect
        private readonly string ConnectString;
        // Array of actual database's names in server
        private string[] Databases { get; set; }

        public ServerInfo(string serverName,string user, string password, double size)
        {
            ServerName = serverName;
            Size = size;
            ConnectString = $"Server={ServerName};Port=5432;Username={user};Password={password};Database=postgres";
            GetInfo().Wait();// for actual left size and array of databases
        }
        // Get array of actual database's names in server
        async Task<string[]> GetDatabases()
        {
            await using var connect = new NpgsqlConnection(ConnectString);
            await connect.OpenAsync();
            var result = new List<string>();
            await using (var cmd = new NpgsqlCommand("SELECT datname FROM pg_database", connect))
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    result.Add(reader.GetString(0));
            }
            await connect.CloseAsync();
            return result.ToArray();
        }
        // Get list of database information
        public async Task<IEnumerable<DatabaseInfo>> GetInfo()
        {
            Databases = GetDatabases().Result; // update databases
            await using var connect = new NpgsqlConnection(ConnectString);
            await connect.OpenAsync();

            var result = new List<DatabaseInfo>();
            var left = Size;
            foreach (var db in Databases) // for each database name in server
            {
                await using var cmd = new NpgsqlCommand($"SELECT pg_database_size('{db}')", connect); // return size of db in bytes
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var sizeInGb = (double)reader.GetInt64(0)/(1024*1024*1024);// from bytes to GBs
                    result.Add(new DatabaseInfo(db, sizeInGb));
                    left -= sizeInGb; // calculate free memory
                }
            }
            await connect.CloseAsync();
            SizeLeft = left;
            return result;
        }
    }
}

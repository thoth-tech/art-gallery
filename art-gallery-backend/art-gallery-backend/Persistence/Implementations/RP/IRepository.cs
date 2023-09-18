using Npgsql;

namespace Art_Gallery_Backend.Persistence.Implementations.RP
{
    public class IRepository
    {
        private readonly IConfiguration _configuration;

        public IRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<T> ExecuteReader<T>(string sqlCommand, NpgsqlParameter[]? dbParams = null) where T : class, new()
        {
            var entities = new List<T>();
            using var conn = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            conn.Open();
            using var cmd = new NpgsqlCommand(sqlCommand, conn);

            if (dbParams is not null)
            {
                cmd.Parameters.AddRange(dbParams.Where(x => x.Value is not null).ToArray());
            }
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var entity = new T();
                dr.MapTo(entity);
                entities.Add(entity);
            }
            return entities;
        }

        // TODO: test this method, create IDataAccessAsync interfaces for each BC, implement these interfaces in RP
        public async Task<List<T>> ExecuteReaderAsync<T>(string sqlCommand, NpgsqlParameter[]? dbParams = null) where T : class, new()
        {
            var entities = new List<T>();
            using var conn = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand(sqlCommand, conn);

            if (dbParams is not null)
            {
                cmd.Parameters.AddRange(dbParams.Where(x => x.Value is not null).ToArray());
            }
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                var entity = new T();
                dr.MapTo(entity);
                entities.Add(entity);
            }
            return entities;
        }

        public int ExecuteNonQuery(string sqlCommand, NpgsqlParameter[]? dbParams = null)
        {
            using var conn = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            conn.Open();
            using var cmd = new NpgsqlCommand(sqlCommand, conn);

            if (dbParams is not null)
            {
                cmd.Parameters.AddRange(dbParams.Where(x => x.Value is not null).ToArray());
            }

            var result = cmd.ExecuteNonQuery();

            return result;
        }
    }
}

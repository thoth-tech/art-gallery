using Npgsql;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.RP
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

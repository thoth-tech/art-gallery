using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using Npgsql;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.ADO
{
    public class NationADO : INationDataAccess
    {
        private readonly IConfiguration _configuration;

        public NationADO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<NationOutputDto> GetNations()
        {
            var nations = new List<NationOutputDto>();
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("SELECT * FROM nation", connection);
                {
                    using var dr = cmd.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                var nationId = (int)dr["nation_id"];
                                var title = (string)dr["title"];
                                var modifiedAt = (DateTime)dr["modified_at"];
                                var createdAt = (DateTime)dr["created_at"];
                                nations.Add(new NationOutputDto(nationId, title, modifiedAt, createdAt));
                            }
                        }
                        return nations;
                    }
                }
            }
        }

        public NationOutputDto? GetNationById(int id)
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("SELECT * FROM nation WHERE nation_id = @nationId", connection);
                {
                    cmd.Parameters.AddWithValue("@nationId", id);
                    using var dr = cmd.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                var nationId = (int)dr["nation_id"];
                                var title = (string)dr["title"];
                                var modifiedAt = (DateTime)dr["modified_at"];
                                var createdAt = (DateTime)dr["created_at"];
                                return new NationOutputDto(nationId, title, modifiedAt, createdAt);
                            }
                        }
                        return null;
                    }
                }
            }
        }

        public NationInputDto? InsertNation(NationInputDto nation)
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("INSERT INTO nation(title, modified_at, created_at) VALUES (@title, current_timestamp, current_timestamp)", connection);
                {
                    cmd.Parameters.AddWithValue("@title", nation.Title);
                    var result = cmd.ExecuteNonQuery();
                    return result is 1 ? nation : null;
                }
            }
        }

        public NationInputDto? UpdateNation(int id, NationInputDto nation)
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("UPDATE nation SET title = @title, modified_at = current_timestamp WHERE nation_id = @nationId", connection);
                {
                    cmd.Parameters.AddWithValue("@nationId", id);
                    cmd.Parameters.AddWithValue("@title", nation.Title);
                    var result = cmd.ExecuteNonQuery();
                    return result is 1 ? nation : null;
                }
            }
        }

        public bool DeleteNation(int id)
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("DELETE FROM nation WHERE nation_id = @nationId", connection);
                {
                    cmd.Parameters.AddWithValue("@nationId", id);
                    var result = cmd.ExecuteNonQuery();
                    return result == 1;
                }
            }
        }
    }
}

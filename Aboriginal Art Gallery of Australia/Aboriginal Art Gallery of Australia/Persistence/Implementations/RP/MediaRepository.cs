using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using static Aboriginal_Art_Gallery_of_Australia.Persistence.ExtensionMethods;
using Npgsql;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.RP
{
    public class NationRepository : IRepository, INationDataAccess
    {
        private IRepository _repo => this;

        private readonly IConfiguration _configuration;

        public NationRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public List<NationOutputDto> GetNations()
        {
            var nations = _repo.ExecuteReader<NationOutputDto>("SELECT * FROM nation");
            return nations;
        }

        public NationOutputDto? GetNationById(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("nationId", id)
            };

            var nation = _repo.ExecuteReader<NationOutputDto>("SELECT * FROM nation WHERE nation_id = @nationId", sqlParams)
                .SingleOrDefault();

            return nation;
        }

        public NationInputDto? InsertNation(NationInputDto nation)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("title", nation.Title)
            };

            var result = _repo.ExecuteReader<NationInputDto>("INSERT INTO nation VALUES " +
                "(DEFAULT, @title, current_timestamp, current_timestamp) RETURNING *", sqlParams)
                .SingleOrDefault();

            return result;
        }

        public NationInputDto? UpdateNation(int id, NationInputDto nation)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("nationId", id),
                new("title", nation.Title)
            };

            var result = _repo.ExecuteReader<NationInputDto>("UPDATE nation SET title = @title, " +
                "modified_at = current_timestamp WHERE nation_id = @nationId RETURNING *", sqlParams)
                .SingleOrDefault();

            return result;
        }

        public bool DeleteNation(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("nationId", id)
            };

            _repo.ExecuteReader<NationOutputDto>("DELETE FROM nation WHERE nation_id = @nationId", sqlParams);

            return true;
        }
    }
}

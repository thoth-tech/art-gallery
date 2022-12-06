using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using static Aboriginal_Art_Gallery_of_Australia.Persistence.ExtensionMethods;
using Npgsql;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.RP
{
    public class NationRepository : IRepository, INationDataAccess
    {
        public NationRepository(IConfiguration configuration) : base(configuration)
        {
        }

        private IRepository _repo => this;

        public bool DeleteNation(int id)
        {
            throw new NotImplementedException();
        }

        public NationOutputDto? GetNationById(int id)
        {
            throw new NotImplementedException();
        }

        public List<NationOutputDto> GetNations()
        {
            throw new NotImplementedException();
        }

        public NationInputDto? InsertNation(NationInputDto nation)
        {
            throw new NotImplementedException();
        }

        public NationInputDto? UpdateNation(int id, NationInputDto nation)
        {
            throw new NotImplementedException();
        }
    }
}

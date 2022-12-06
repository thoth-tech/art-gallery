using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using static Aboriginal_Art_Gallery_of_Australia.Persistence.ExtensionMethods;
using Npgsql;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.RP
{
    public class ExhibitionRepository : IRepository, IExhibitionDataAccess
    {
        public ExhibitionRepository(IConfiguration configuration) : base(configuration)
        {
        }

        private IRepository _repo => this;

        public ArtworkExhibitionDto? AssignArtwork(int artworkId, int exhibitionId)
        {
            throw new NotImplementedException();
        }

        public bool DeassignArtwork(int artworkId, int exhibitionId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteExhibition(int id)
        {
            throw new NotImplementedException();
        }

        public ExhibitionArtworkOutputDto? GetExhibitionArtworksById(int id)
        {
            throw new NotImplementedException();
        }

        public ExhibitionOutputDto? GetExhibitionById(int id)
        {
            throw new NotImplementedException();
        }

        public List<ExhibitionOutputDto> GetExhibitions()
        {
            throw new NotImplementedException();
        }

        public ExhibitionInputDto? InsertExhibition(ExhibitionInputDto exhibition)
        {
            throw new NotImplementedException();
        }

        public ExhibitionInputDto? UpdateExhibition(int id, ExhibitionInputDto exhibition)
        {
            throw new NotImplementedException();
        }
    }
}

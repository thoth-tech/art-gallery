using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using static Aboriginal_Art_Gallery_of_Australia.Persistence.ExtensionMethods;
using Npgsql;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.RP
{
    public class ArtworkRepository : IRepository, IArtworkDataAccess
    {
        public ArtworkRepository(IConfiguration configuration) : base(configuration)
        {
        }

        private IRepository _repo => this;

        public ArtistArtworkDto? AssignArtist(int artistId, int artworkId)
        {
            throw new NotImplementedException();
        }

        public ArtworkExhibitionDto? AssignExhibition(int artworkId, int exhibitionId)
        {
            throw new NotImplementedException();
        }

        public bool DeassignArtist(int artistId, int artworkId)
        {
            throw new NotImplementedException();
        }

        public bool DeassignExhibition(int artworkId, int exhibitionId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteArtwork(int id)
        {
            throw new NotImplementedException();
        }

        public ArtworkOutputDto? GetArtworkById(int id)
        {
            throw new NotImplementedException();
        }

        public List<ArtworkOutputDto> GetArtworks()
        {
            throw new NotImplementedException();
        }

        public ArtworkInputDto? InsertArtwork(ArtworkInputDto artwork)
        {
            throw new NotImplementedException();
        }

        public ArtworkInputDto? UpdateArtwork(int id, ArtworkInputDto artwork)
        {
            throw new NotImplementedException();
        }
    }
}

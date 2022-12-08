using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using static Aboriginal_Art_Gallery_of_Australia.Persistence.ExtensionMethods;
using Npgsql;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.RP
{
    public class ArtworkRepository : IRepository, IArtworkDataAccess
    {

        //TODO: Add data to database and test each of the following methods

        public ArtworkRepository(IConfiguration configuration) : base(configuration)
        {
        }

        private IRepository _repo => this;

        //TODO: Complete method
        public List<ArtworkOutputDto> GetArtworks()
        {
            throw new NotImplementedException();
        }

        //TODO: Complete method
        public ArtworkOutputDto? GetArtworkById(int id)
        {
            throw new NotImplementedException();
        }



        public ArtworkInputDto? InsertArtwork(ArtworkInputDto artwork)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("title", artwork.Title),
                new("description", artwork.Description),
                new("media", artwork.Media),
                new("primary_image_url", artwork.PrimaryImageURL),
                new("secondary_image_url", artwork.SecondaryImageURL ?? (object)DBNull.Value),
                new("year_created", artwork.YearCreated),
                new("nation_id", artwork.NationId),
                new("modifieddate", DateTime.Now),
                new("createddate", DateTime.Now)
            };

            var result = _repo.ExecuteReader<ArtworkInputDto>("INSERT INTO artwork(title, description, media, " +
                "primary_image_url, secondary_image_url, year_created, nation_id, modified_at, created_at) " +
                "VALUES (@title, @description, @media, @primaryImageURL, @secondaryImageURL, @yearCreated, " +
                "@nationId, @modifieddate, @createddate)", sqlParams)
                .SingleOrDefault();

            return result;
        }

        public ArtworkInputDto? UpdateArtwork(int id, ArtworkInputDto artwork)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artwork_id", id),
                new("title", artwork.Title),
                new("description", artwork.Description),
                new("media", artwork.Media),
                new("primaryImageURL", artwork.PrimaryImageURL),
                new("secondaryImageURL", artwork.SecondaryImageURL ?? (object)DBNull.Value),
                new("yearCreated", artwork.YearCreated),
                new("nationId", artwork.NationId),
                new("modifieddate", DateTime.Now)
            };

            var result = _repo.ExecuteReader<ArtworkInputDto>("UPDATE artwork SET title = @title, description = @description, " +
                "media = @media, primary_image_url = @primaryImageURL, secondary_image_url = @secondaryImageURL, " +
                "year_created = @yearCreated, nation_id = @nationId, modified_at = @modifieddate " +
                "WHERE artwork_id = @artwork_id", sqlParams)
                .SingleOrDefault();

            return result;
        }


        public bool DeleteArtwork(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artworkId", id)
            };

            _repo.ExecuteReader<ArtistOutputDto>("DELETE FROM artwork WHERE artwork_id = @artworkId", sqlParams);

            return true;
        }

        public ArtistArtworkDto? AssignArtist(int artistId, int artworkId)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artistId", artistId),
                new("artworkId", artworkId),
                new("modifieddate", DateTime.Now),
                new("createddate", DateTime.Now)
            };

            var result = _repo.ExecuteReader<ArtistArtworkDto>("INSERT INTO artist_artwork(artist_id, artwork_id, " +
                "modified_at, created_at) VALUES (@artistId, @artworkId, @modifieddate, @createddate)", sqlParams)
                .SingleOrDefault();

            return result;
        }

        public bool DeassignArtist(int artistId, int artworkId)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artistId", artistId),
                new("artworkId", artworkId)
            };

            _repo.ExecuteReader<ArtistArtworkDto>("DELETE FROM artist_artwork WHERE artist_id = @artistId " +
                "AND artwork_id = @artworkId", sqlParams);

            return true;
        }

        public ArtworkExhibitionDto? AssignExhibition(int artworkId, int exhibitionId)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artworkId", artworkId),
                new("exhibitionId", exhibitionId),
                new("modifieddate", DateTime.Now),
                new("createddate", DateTime.Now)
            };

            var result = _repo.ExecuteReader<ArtworkExhibitionDto>("INSERT INTO artwork_exhibition(artwork_id, exhibition_id, " +
                "modified_at, created_at) VALUES (@artworkId, @exhibitionId, @modifieddate, @createddate)", sqlParams)
                .SingleOrDefault();

            return result;
        }

        public bool DeassignExhibition(int artworkId, int exhibitionId)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artworkId", artworkId),
                new("exhibitionId", exhibitionId)
            };

            _repo.ExecuteReader<ArtworkExhibitionDto>("DELETE FROM artwork_exhibition WHERE artwork_id = @artworkId " +
                "AND exhibition_id = @exhibitionId", sqlParams);

            return true;
        }
    }
}

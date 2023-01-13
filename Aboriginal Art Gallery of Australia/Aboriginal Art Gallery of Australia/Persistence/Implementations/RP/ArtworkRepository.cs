using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using static Aboriginal_Art_Gallery_of_Australia.Persistence.ExtensionMethods;
using Npgsql;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.RP
{
    public class ArtworkRepository : IRepository, IArtworkDataAccess
    {

        //TODO: Test last 4 methods and fix GetById
        //Urls not reading from database

        private IRepository _repo => this;

        private readonly IConfiguration _configuration;

        public ArtworkRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        //TODO: Find a way to write lines 25 and 28 in a single SQL statement
        public List<ArtworkOutputDto> GetArtworks()
        {
            var allArtworks = new List<ArtworkOutputDto>();
            var allArtworkArtists = new List<KeyValuePair<int, String>>();

            var artworks = _repo.ExecuteReader<ArtworkOutputDto>("SELECT artwork_id " +
                "FROM artist_artwork INNER JOIN artist ON artist_artwork.artist_id = artist.artist_id");

            var artists = _repo.ExecuteReader<ArtistOutputDto>("SELECT display_name FROM artist_artwork " +
                "INNER JOIN artist ON artist_artwork.artist_id = artist.artist_id");

            int i = 0;
            foreach (ArtworkOutputDto artwork in artworks)
            {
                allArtworkArtists.Add(new KeyValuePair<int, String>(artwork.ArtworkId, artists[i].DisplayName));
                i++;
            }

            var lookup = allArtworkArtists.ToLookup(kvp => kvp.Key, kvp => kvp.Value);

            var artworksOutput = _repo.ExecuteReader<ArtworkOutputDto>("SELECT artwork_id, artwork.title, " +
                "artwork.description, primary_image_url, secondary_image_url, artwork.year_created, artwork.modified_at, " +
                "artwork.created_at, media_type, media.description as media_description FROM artwork INNER JOIN media ON " +
                "media.media_id = artwork.media_id");

            foreach (ArtworkOutputDto artwork in artworksOutput)
            {
                var artworkArtists = new List<String>();
                foreach (string artist in lookup[artwork.ArtworkId])
                {
                    artworkArtists.Add(artist);
                }

                artwork.ContributingArtists = artworkArtists;
            }

            return artworksOutput;
        }

        //TODO: Find a way to write lines 68 and 72 in a single SQL statement + fix function ??
        public ArtworkOutputDto? GetArtworkById(int id)
        {
            var artworkArtists = new List<String>();
            var sqlParams = new NpgsqlParameter[]
            {
                new("artworkId", id)
            };

            var artists = _repo.ExecuteReader<ArtistOutputDto>("SELECT display_name FROM artist_artwork " +
                "INNER JOIN artist ON artist_artwork.artist_id = artist.artist_id " +
                "WHERE artwork_id = @artworkId", sqlParams);


            // This query is throwing InvalidOperation exception: 'the parameter already belongs to a collection'
            var artworkOutput = _repo.ExecuteReader<ArtworkOutputDto>("SELECT artwork_id, artwork.title, " +
                "description, media, primary_image_url, secondary_image_url, year_created, artwork.modified_at, " +
                "artwork.created_at, media.media_type as media_type FROM artwork INNER JOIN media " +
                "ON media.media_id = artwork.media_id WHERE artwork_id = @artworkId", sqlParams)
                .SingleOrDefault();

            foreach (ArtistOutputDto artist in artists)
            {
                artworkArtists.Add(artist.DisplayName);
            }

            if (artworkOutput is not null) artworkOutput.ContributingArtists = artworkArtists;

            return artworkOutput;
        }

        public ArtworkOutputDto? GetArtworkOfTheDay()
        {
            // Ultra hacky to just get something working for you guys. Will swap between artwork 1 and artwork 2 each minute.
            if (DateTime.Now.Minute % 2 == 0)
                return GetArtworkById(1);
            else return GetArtworkById(2);
        }

        public ArtworkInputDto? InsertArtwork(ArtworkInputDto artwork)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("title", artwork.Title),
                new("description", artwork.Description),
                new("mediaId", artwork.MediaId),
                new("primary_image_url", artwork.PrimaryImageURL),
                new("secondary_image_url", artwork.SecondaryImageURL ?? (object)DBNull.Value),
                new("year_created", artwork.YearCreated)
            };

            var result = _repo.ExecuteReader<ArtworkInputDto>("INSERT INTO artwork VALUES (DEFAULT, " +
                "@title, @description, @mediaId, @primary_image_url, @secondary_image_url, @year_created, " +
                "current_timestamp, current_timestamp) RETURNING *", sqlParams)
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
                new("mediaId", artwork.MediaId),
                new("primaryImageURL", artwork.PrimaryImageURL),
                new("secondaryImageURL", artwork.SecondaryImageURL ?? (object)DBNull.Value),
                new("yearCreated", artwork.YearCreated)
            };

            var result = _repo.ExecuteReader<ArtworkInputDto>("UPDATE artwork SET title = @title, description = @description, " +
                "media = @mediaId, primary_image_url = @primaryImageURL, secondary_image_url = @secondaryImageURL, " +
                "year_created = @yearCreated, modified_at = current_timestamp " +
                "WHERE artwork_id = @artwork_id RETURNING *", sqlParams)
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


        // The assign and deassign method is also giving the "The parameter already belongs to a collection" error
        public ArtistArtworkDto? AssignArtist(int artistId, int artworkId)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artistId", artistId),
                new("artworkId", artworkId)
            };

            var result = _repo.ExecuteReader<ArtistArtworkDto>("INSERT INTO artist_artwork(artist_id, artwork_id, " +
                "modified_at, created_at) VALUES (@artistId, @artworkId, current_timestamp, current_timestamp) " +
                "RETURNING *", sqlParams)
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
                new("exhibitionId", exhibitionId)
            };

            var result = _repo.ExecuteReader<ArtworkExhibitionDto>("INSERT INTO artwork_exhibition(artwork_id, exhibition_id, " +
                "modified_at, created_at) VALUES (@artworkId, @exhibitionId, current_timestamp, current_timestamp) " +
                "RETURNING *", sqlParams)
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

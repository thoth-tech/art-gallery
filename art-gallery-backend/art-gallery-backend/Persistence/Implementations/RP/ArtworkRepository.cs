﻿using Art_Gallery_Backend.Models.DTOs;
using Art_Gallery_Backend.Persistence.Interfaces;
using static Art_Gallery_Backend.Persistence.ExtensionMethods;
using Npgsql;
using System.Globalization;
using Art_Gallery_Backend.Models.Database_Models;

namespace Art_Gallery_Backend.Persistence.Implementations.RP
{
    public class ArtworkRepository : IRepository, IArtworkDataAccess
    {
        private IRepository _repo => this;

        private readonly IConfiguration _configuration;

        public ArtworkRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        readonly TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;

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
                "artwork.created_at, media_type, media.description as media_description, price FROM artwork INNER JOIN media ON " +
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

            var artworkOutput = _repo.ExecuteReader<ArtworkOutputDto>("SELECT artwork_id, artwork.title, " +
                "artwork.description, primary_image_url, secondary_image_url, year_created, artwork.modified_at, " +
                "artwork.created_at, media.media_type as media_type, price FROM artwork INNER JOIN media " +
                $"ON media.media_id = artwork.media_id WHERE artwork_id = {id}")
                .SingleOrDefault();

            foreach (ArtistOutputDto artist in artists)
            {
                artworkArtists.Add(artist.DisplayName);
            }

            if (artworkOutput is not null) artworkOutput.ContributingArtists = artworkArtists;

            return artworkOutput;
        }

        public ArtworkInputDto? InsertArtwork(ArtworkInputDto artwork)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("title", textInfo.ToTitleCase(artwork.Title)),
                new("description", artwork.Description),
                new("mediaId", artwork.MediaId ?? (object)DBNull.Value),
                new("primary_image_url", artwork.PrimaryImageUrl),
                new("secondary_image_url", artwork.SecondaryImageUrl ?? (object)DBNull.Value),
                new("year_created", artwork.YearCreated ?? (object)DBNull.Value),
                new("price", artwork.Price)
            };

            var result = _repo.ExecuteReader<ArtworkInputDto>("INSERT INTO artwork VALUES (DEFAULT, " +
                "@title, @description, @primary_image_url, @secondary_image_url, @year_created, @mediaId, " +
                "current_timestamp, current_timestamp, @price) RETURNING *", sqlParams)
                .SingleOrDefault();

            return result;
        }

        public ArtworkInputDto? UpdateArtwork(int id, ArtworkInputDto artwork)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artwork_id", id),
                new("title", textInfo.ToTitleCase(artwork.Title)),
                new("description", artwork.Description),
                new("mediaId", artwork.MediaId ?? (object)DBNull.Value),
                new("primaryImageURL", artwork.PrimaryImageUrl),
                new("secondaryImageURL", artwork.SecondaryImageUrl ?? (object)DBNull.Value),
                new("yearCreated", artwork.YearCreated ?? (object)DBNull.Value),
                new("price", artwork.Price)
            };

            var result = _repo.ExecuteReader<ArtworkInputDto>("UPDATE artwork SET title = @title, description = @description, " +
                "mediaId = @mediaId, primary_image_url = @primaryImageURL, secondary_image_url = @secondaryImageURL, " +
                "year_created = @yearCreated, modified_at = current_timestamp, price = @price " +
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

        public ArtistArtworkDto? AllocateArtist(int artistId, int artworkId)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("artist_id", artistId),
                new("artwork_id", artworkId)
            };

            var result = _repo.ExecuteReader<ArtistArtworkDto>($"INSERT INTO artist_artwork(artist_id, artwork_id, " +
                "modified_at, created_at) VALUES (@artist_id, @artwork_id, current_timestamp, current_timestamp) " +
                "RETURNING *", sqlParams)
                .SingleOrDefault();

            return result;
        }

        public bool DeallocateArtist(int artistId, int artworkId)
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

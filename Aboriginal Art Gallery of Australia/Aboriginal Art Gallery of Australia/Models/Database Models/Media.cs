using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;

namespace Aboriginal_Art_Gallery_of_Australia.Models.Database_Models
{
    /// <summary>
    /// The Media class is responsible for handling the database model associated with various media types. 
    /// </summary>
    public class Media : IMediaDataAccess
    {
        public int MediaId { get; set; }
        public string MediaType { get; set; }
        public string Description { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public Media(int mediaId, string mediaType, string description, DateTime modifiedAt, DateTime createdAt)
        {
            MediaId = mediaId;
            MediaType = mediaType;
            Description = description;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
        }

        // Active Record - Everything under line 25 is required for the active record implementation.
        private static string _connectionString = "Host=localhost;Database=Deakin University | AAGoA;Username=postgres;Password=postgreSQL;";

        public Media() { }

        public List<MediaOutputDto> GetMediaTypes()
        {
            throw new NotImplementedException();
        }

        public MediaOutputDto? GetMediaTypeById(int id)
        {
            throw new NotImplementedException();
        }

        public MediaInputDto? InsertMediaType(MediaInputDto media)
        {
            throw new NotImplementedException();
        }

        public MediaInputDto? UpdateMediaType(int id, MediaInputDto media)
        {
            throw new NotImplementedException();
        }

        public bool DeleteMediaType(int id)
        {
            throw new NotImplementedException();
        }
    }
}
namespace Art_Gallery_Backend.Models.DTOs
{
    /// <summary>
    /// The MediaInputDto class is used to decouple the service layer from the database layer. It provides a means of mapping the necessary user input to the appropriate database model.
    /// </summary>
    public class MediaInputDto
    {
        public string MediaType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public MediaInputDto(string mediaType, string description)
        {
            MediaType = mediaType;
            Description = description;
        }

        public MediaInputDto() { }
    }

    /// <summary>
    /// The MediaOutputDto class is used to decouple the database layer from the service layer. It provides a means of mapping the necessary information from the database to the appropriate user output.
    /// </summary>
    public class MediaOutputDto
    {
        public int MediaId { get; set; } = 0;
        public string MediaType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ModifiedAt { get; set; } = new DateTime();
        public DateTime CreatedAt { get; set; } = new DateTime();

        public MediaOutputDto(int mediaId, string mediaType, string description, DateTime modifiedAt, DateTime createdAt)
        {
            MediaId = mediaId;
            MediaType = mediaType;
            Description = description;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
        }

        public MediaOutputDto() { }
    }
}

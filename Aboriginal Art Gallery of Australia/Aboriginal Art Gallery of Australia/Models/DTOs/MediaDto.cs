namespace Aboriginal_Art_Gallery_of_Australia.Models.DTOs
{
    public class MediaInputDto
    {
        public string MediaType { get; set; }
        public string Description { get; set; }

        public MediaInputDto(string mediaType, string description)
        {
            this.MediaType = mediaType;
            this.Description = description;
        }

        public MediaInputDto(){
            this.MediaType = "";
            this.Description = "";
        }
    }

    public class MediaOutputDto
    {
        public int Id { get; set; }
        public string MediaType { get; set; }
        public string Description { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public MediaOutputDto(int id, string mediaType, string description, DateTime modifiedAt, DateTime createdAt)
        {
            this.Id = id;
            this.MediaType = mediaType;
            this.Description = description;
            this.ModifiedAt = modifiedAt;
            this.CreatedAt = createdAt;
        }

        public MediaOutputDto(){
            this.MediaType = "";
            this.Description = "";
        }
    }
}
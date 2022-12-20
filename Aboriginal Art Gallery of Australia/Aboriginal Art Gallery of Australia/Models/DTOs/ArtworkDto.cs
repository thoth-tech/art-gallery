namespace Aboriginal_Art_Gallery_of_Australia.Models.DTOs
{
    public class ArtworkInputDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PrimaryImageURL { get; set; }
        public string SecondaryImageURL { get; set; }
        public int? YearCreated { get; set; }
        public int? MediaId { get; set; }

        public ArtworkInputDto(string title, string description, string primaryImageURL, string secondaryImageURL, int? yearCreated, int? mediaId)
        {
            this.Title = title;
            this.Description = description;
            this.PrimaryImageURL = primaryImageURL;
            this.SecondaryImageURL = secondaryImageURL;
            this.YearCreated = yearCreated;
            this.MediaId = mediaId;
        }

        public ArtworkInputDto()
        {
            this.Title = "";
            this.Description = "";
            this.PrimaryImageURL = "";
            this.SecondaryImageURL = "";
        }
    }

    public class ArtworkOutputDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PrimaryImageURL { get; set; }
        public string? SecondaryImageURL { get; set; }
        public int YearCreated { get; set; }
        public string MediaType { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<String> ContributingArtists { get; set; }

        public ArtworkOutputDto()
        {
            this.Title = "";
            this.Description = "";
            this.PrimaryImageURL = "";
            this.SecondaryImageURL = "";
            this.MediaType = "";
            this.ContributingArtists = new List<String>();
        }

        public ArtworkOutputDto(int id, string title, string description, string primaryImageURL, string? secondaryImageURL, int yearCreated, string mediaType, DateTime modifiedAt, DateTime createdAt, List<string> contributingArtists)
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.PrimaryImageURL = primaryImageURL;
            this.SecondaryImageURL = secondaryImageURL;
            this.YearCreated = yearCreated;
            this.MediaType = mediaType;
            this.ModifiedAt = modifiedAt;
            this.CreatedAt = createdAt;
            this.ContributingArtists = contributingArtists;
        }
    }
}
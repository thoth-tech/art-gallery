namespace Aboriginal_Art_Gallery_of_Australia.Models.DTOs
{
    public class ArtworkInputDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PrimaryImageUrl { get; set; } = string.Empty;
        public string SecondaryImageUrl { get; set; } = string.Empty;
        public int? YearCreated { get; set; } = null;
        public int? MediaId { get; set; } = null;

        public ArtworkInputDto(string title, string description, string primaryImageURL, string secondaryImageURL, int? yearCreated, int? mediaId)
        {
            this.Title = title;
            this.Description = description;
            this.PrimaryImageUrl = primaryImageURL;
            this.SecondaryImageUrl = secondaryImageURL;
            this.YearCreated = yearCreated;
            this.MediaId = mediaId;
        }

        public ArtworkInputDto()
        {
            this.Title = "";
            this.Description = "";
            this.PrimaryImageUrl = "";
            this.SecondaryImageUrl = "";
        }
    }

    public class ArtworkOutputDto
    {
        public int ArtworkId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PrimaryImageUrl { get; set; }
        public string? SecondaryImageUrl { get; set; }
        public int YearCreated { get; set; }
        public string MediaType { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<String> ContributingArtists { get; set; } = null!;

        public ArtworkOutputDto()
        {
            this.Title = "";
            this.Description = "";
            this.PrimaryImageUrl = "";
            this.SecondaryImageUrl = "";
            this.MediaType = "";
            this.ContributingArtists = new List<String>();
        }

        public ArtworkOutputDto(int artworkId, string title, string description, string primaryImageURL, string? secondaryImageURL, int yearCreated, string mediaType, DateTime modifiedAt, DateTime createdAt, List<string> contributingArtists)
        {
            this.ArtworkId = artworkId;
            this.Title = title;
            this.Description = description;
            this.PrimaryImageUrl = primaryImageURL;
            this.SecondaryImageUrl = secondaryImageURL;
            this.YearCreated = yearCreated;
            this.MediaType = mediaType;
            this.ModifiedAt = modifiedAt;
            this.CreatedAt = createdAt;
            this.ContributingArtists = contributingArtists;
        }
    }
}
namespace Art_Gallery_Backend.Models.DTOs
{
    /// <summary>
    /// The ArtworkInputDto class is used to decouple the service layer from the database layer. It provides a means of mapping the necessary user input to the appropriate database model.
    /// </summary>
    public class ArtworkInputDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PrimaryImageUrl { get; set; } = string.Empty;
        public string? SecondaryImageUrl { get; set; } = null;
        public int? YearCreated { get; set; } = null;
        public int? MediaId { get; set; } = null;
        public decimal Price { get; set; } = 0.00M;

        public ArtworkInputDto(string title, string description, string primaryImageURL, string? secondaryImageURL, int? yearCreated, int? mediaId, decimal price)
        {
            Title = title;
            Description = description;
            PrimaryImageUrl = primaryImageURL;
            SecondaryImageUrl = secondaryImageURL;
            YearCreated = yearCreated;
            MediaId = mediaId;
            Price = price;
        }

        public ArtworkInputDto() { }
    }

    /// <summary>
    /// The ArtworkOutputDto class is used to decouple the database layer from the service layer. It provides a means of mapping the necessary information from the database to the appropriate user output.
    /// </summary>
    public class ArtworkOutputDto
    {
        public int ArtworkId { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PrimaryImageUrl { get; set; } = string.Empty;
        public string? SecondaryImageUrl { get; set; } = null;
        public int? YearCreated { get; set; } = null;
        public string? MediaType { get; set; } = null;
        public DateTime ModifiedAt { get; set; } = new DateTime();
        public DateTime CreatedAt { get; set; } = new DateTime();
        public decimal Price { get; set; } = 0.00M;
        public List<string> ContributingArtists { get; set; } = new List<string>();

        public ArtworkOutputDto(int artworkId, string title, string description, string primaryImageURL, string? secondaryImageURL, int? yearCreated, string? mediaType, DateTime modifiedAt, DateTime createdAt, decimal price, List<string> contributingArtists)
        {
            ArtworkId = artworkId;
            Title = title;
            Description = description;
            PrimaryImageUrl = primaryImageURL;
            SecondaryImageUrl = secondaryImageURL;
            YearCreated = yearCreated;
            MediaType = mediaType;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
            Price = price;
            ContributingArtists = contributingArtists;
        }

        public ArtworkOutputDto() { }
    }
}

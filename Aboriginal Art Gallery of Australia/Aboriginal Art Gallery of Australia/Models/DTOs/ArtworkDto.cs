namespace Aboriginal_Art_Gallery_of_Australia.Models.DTOs
{
    public class ArtworkInputDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Media { get; set; } = null!;
        public string PrimaryImageUrl { get; set; } = null!;
        public string? SecondaryImageUrl { get; set; } = null;
        public int YearCreated { get; set; }
        public int NationId { get; set; }

        public ArtworkInputDto()
        {
        }

        public ArtworkInputDto(string title, string description, string media, string primaryImageURL, string? secondaryImageURL, int yearCreated, int nationId)
        {
            Title = title;
            Description = description;
            Media = media;
            PrimaryImageUrl = primaryImageURL;
            SecondaryImageUrl = secondaryImageURL;
            YearCreated = yearCreated;
            NationId = nationId;
        }
    }

    public class ArtworkOutputDto
    {
        public int ArtworkId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Media { get; set; } = null!;
        public string PrimaryImageUrl { get; set; } = null!;
        public string? SecondaryImageUrl { get; set; } = null;
        public int YearCreated { get; set; }
        public string Nation { get; set; } = null!;
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<String> ContributingArtists { get; set; } = null!;

        public ArtworkOutputDto()
        {
        }

        public ArtworkOutputDto(int id, string title, string description, string media, string primaryImageURL, string? secondaryImageURL, int yearCreated, string nation, DateTime modifiedAt, DateTime createdAt, List<string> contributingArtists)
        {
            ArtworkId = id;
            Title = title;
            Description = description;
            Media = media;
            PrimaryImageUrl = primaryImageURL;
            SecondaryImageUrl = secondaryImageURL;
            YearCreated = yearCreated;
            Nation = nation;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
            ContributingArtists = contributingArtists;
        }
    }
}
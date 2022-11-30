namespace Aboriginal_Art_Gallery_of_Australia.Models.DTOs
{
    public class ArtworkInputDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Media { get; set; }
        public string PrimaryImageURL { get; set; }
        public string? SecondaryImageURL { get; set; }
        public int YearCreated { get; set; }
        public int NationId { get; set; }

        public ArtworkInputDto(string title, string description, string media, string primaryImageURL, string? secondaryImageURL, int yearCreated, int nationId)
        {
            Title = title;
            Description = description;
            Media = media;
            PrimaryImageURL = primaryImageURL;
            SecondaryImageURL = secondaryImageURL;
            YearCreated = yearCreated;
            NationId = nationId;
        }
    }

    public class ArtworkOutputDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Media { get; set; }
        public string PrimaryImageURL { get; set; }
        public string? SecondaryImageURL { get; set; }
        public int YearCreated { get; set; }
        public string Nation { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<String> ContributingArtists { get; set; }

        public ArtworkOutputDto(int id, string title, string description, string media, string primaryImageURL, string? secondaryImageURL, int yearCreated, string nation, DateTime modifiedAt, DateTime createdAt, List<string> contributingArtists)
        {
            Id = id;
            Title = title;
            Description = description;
            Media = media;
            PrimaryImageURL = primaryImageURL;
            SecondaryImageURL = secondaryImageURL;
            YearCreated = yearCreated;
            Nation = nation;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
            ContributingArtists = contributingArtists;
        }
    }
}
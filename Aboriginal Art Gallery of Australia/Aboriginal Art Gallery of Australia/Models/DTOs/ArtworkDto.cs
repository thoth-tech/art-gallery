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
            Title = title;
            Description = description;
            PrimaryImageURL = primaryImageURL;
            SecondaryImageURL = secondaryImageURL;
            YearCreated = yearCreated;
            MediaId = mediaId;
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

        public ArtworkOutputDto(int id, string title, string description, string primaryImageURL, string? secondaryImageURL, int yearCreated, string mediaType, DateTime modifiedAt, DateTime createdAt, List<string> contributingArtists)
        {
            Id = id;
            Title = title;
            Description = description;
            PrimaryImageURL = primaryImageURL;
            SecondaryImageURL = secondaryImageURL;
            YearCreated = yearCreated;
            MediaType = mediaType;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
            ContributingArtists = contributingArtists;
        }
    }
}
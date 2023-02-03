namespace Art_Gallery_Backend.Models.DTOs
{
    /// <summary>
    /// The ExhibitionInputDto class is used to decouple the service layer from the database layer. It provides a means of mapping the necessary user input to the appropriate database model.
    /// </summary>
    public class ExhibitionInputDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string BackgroundImageUrl { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; } = new DateOnly();
        public DateOnly EndDate { get; set; } = new DateOnly();

        public ExhibitionInputDto(string name, string description, string backgroundImageURL, DateOnly startDate, DateOnly endDate)
        {
            Name = name;
            Description = description;
            BackgroundImageUrl = backgroundImageURL;
            StartDate = startDate;
            EndDate = endDate;
        }

        public ExhibitionInputDto() { }
    }

    /// <summary>
    /// The ExhibitionOutputDto class is used to decouple the database layer from the service layer. It provides a means of mapping the necessary information from the database to the appropriate user output.
    /// </summary>
    public class ExhibitionOutputDto
    {
        public int ExhibitionId { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string BackgroundImageUrl { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; } = new DateOnly();
        public DateOnly EndDate { get; set; } = new DateOnly();
        public DateTime ModifiedAt { get; set; } = new DateTime();
        public DateTime CreatedAt { get; set; } = new DateTime();

        public ExhibitionOutputDto(int exhibitionId, string name, string description, string backgroundImageURL, DateOnly startDate, DateOnly endDate, DateTime modifiedAt, DateTime createdAt)
        {
            ExhibitionId = exhibitionId;
            Name = name;
            Description = description;
            BackgroundImageUrl = backgroundImageURL;
            StartDate = startDate;
            EndDate = endDate;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
        }

        public ExhibitionOutputDto() { }
    }

    /// <summary>
    /// The ExhibitionOutputDto class is used to decouple the database layer from the service layer. It provides a means of mapping the necessary information from the database to the appropriate user output with the addition of artworks currently displayed within the corresponding exhibition.
    /// </summary>
    public class ExhibitionArtworkOutputDto
    {
        public int ExhibitionId { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? BackgroundImageUrl { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; } = new DateOnly();
        public DateOnly EndDate { get; set; } = new DateOnly();
        public DateTime ModifiedAt { get; set; } = new DateTime();
        public DateTime CreatedAt { get; set; } = new DateTime();
        public List<ArtworkOutputDto> ExhibitionArtworks { get; set; } = new List<ArtworkOutputDto>();

        public ExhibitionArtworkOutputDto(int id, string name, string description, string? backgroundImageURL, DateOnly startDate, DateOnly endDate, DateTime modifiedAt, DateTime createdAt, List<ArtworkOutputDto> exhibitionArtworks)
        {
            ExhibitionId = id;
            Name = name;
            Description = description;
            BackgroundImageUrl = backgroundImageURL;
            StartDate = startDate;
            EndDate = endDate;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
            ExhibitionArtworks = exhibitionArtworks;
        }

        public ExhibitionArtworkOutputDto() { }
    }
}
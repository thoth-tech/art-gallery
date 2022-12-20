namespace Aboriginal_Art_Gallery_of_Australia.Models.DTOs
{
    public class ExhibitionInputDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string BackgroundImageUrl { get; set; } = null!;

        public ExhibitionInputDto()
        {
        }

        public ExhibitionInputDto(string name, string description, string backgroundImageUrl)
        {
            Name = name;
            Description = description;
            BackgroundImageUrl = backgroundImageUrl;
        }
    }

    public class ExhibitionOutputDto
    {
        public int ExhibitionId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string BackgroundImageUrl { get; set; } = null!;
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public ExhibitionOutputDto()
        {
        }

        public ExhibitionOutputDto(int id, string name, string description, string backgroundImageUrl, DateTime modifiedAt, DateTime createdAt)
        {
            ExhibitionId = id;
            Name = name;
            Description = description;
            BackgroundImageUrl = backgroundImageUrl;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
        }
    }

    public class ExhibitionArtworkOutputDto
    {
        public int ExhibitionId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? BackgroundImageUrl { get; set; } = null;
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ArtworkOutputDto> ExhibitionArtworks { get; set; } = null!;

        public ExhibitionArtworkOutputDto()
        {
        }

        public ExhibitionArtworkOutputDto(int id, string name, string description, string? backgroundImageUrl, DateTime modifiedAt, DateTime createdAt, List<ArtworkOutputDto> exhibitionArtworks)
        {
            ExhibitionId = id;
            Name = name;
            Description = description;
            BackgroundImageUrl = backgroundImageUrl;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
            ExhibitionArtworks = exhibitionArtworks;
        }
    }
}
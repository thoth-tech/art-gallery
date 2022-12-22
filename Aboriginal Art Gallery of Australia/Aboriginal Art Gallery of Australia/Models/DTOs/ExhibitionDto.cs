using System.Text.Json.Serialization;

namespace Aboriginal_Art_Gallery_of_Australia.Models.DTOs
{
    public class ExhibitionInputDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string BackgroundImageURL { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public ExhibitionInputDto(string name, string description, string backgroundImageURL, DateOnly startDate, DateOnly endDate)
        {
            this.Name = name;
            this.Description = description;
            this.BackgroundImageURL = backgroundImageURL;
            this.StartDate = startDate;
            this.EndDate = endDate;
        }

        public ExhibitionInputDto(){
            this.Name = "";
            this.Description = "";
            this.BackgroundImageURL = "";
        }
    }

    public class ExhibitionOutputDto
    {
        public int ExhibitionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BackgroundImageURL { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public ExhibitionOutputDto(int exhibitionId, string name, string description, string backgroundImageURL, DateOnly startDate, DateOnly endDate, DateTime modifiedAt, DateTime createdAt)
        {
            this.ExhibitionId = exhibitionId;
            this.Name = name;
            this.Description = description;
            this.BackgroundImageURL = backgroundImageURL;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.ModifiedAt = modifiedAt;
            this.CreatedAt = createdAt;
        }

        public ExhibitionOutputDto(){
            this.Name = "";
            this.Description = "";
            this.BackgroundImageURL = "";
        }
    }

    public class ExhibitionArtworkOutputDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? BackgroundImageURL { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ArtworkOutputDto> ExhibitionArtworks { get; set; } = null!;

        public ExhibitionArtworkOutputDto(int id, string name, string description, string? backgroundImageURL, DateOnly startDate, DateOnly endDate, DateTime modifiedAt, DateTime createdAt, List<ArtworkOutputDto> exhibitionArtworks)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.BackgroundImageURL = backgroundImageURL;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.ModifiedAt = modifiedAt;
            this.CreatedAt = createdAt;
            this.ExhibitionArtworks = exhibitionArtworks;
        }

        public ExhibitionArtworkOutputDto(){
            this.Name = "";
            this.Description = "";
            this.BackgroundImageURL = "";
            this.ExhibitionArtworks = new List<ArtworkOutputDto>();
        }
    }
}
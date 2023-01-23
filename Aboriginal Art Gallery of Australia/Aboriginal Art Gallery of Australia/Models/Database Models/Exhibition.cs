using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;

namespace Aboriginal_Art_Gallery_of_Australia.Models.Database_Models
{
    /// <summary>
    /// The Exhibition class is responsible for handling the database model associated with exhibitions. 
    /// </summary>
    public class Exhibition : IExhibitionDataAccess
    {
        public int ExhibitionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BackgroundImageURL { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public Exhibition(int exhibitionId, string name, string description, string backgroundImageURL, DateOnly startDate, DateOnly endDate, DateTime modifiedAt, DateTime createdAt)
        {
            ExhibitionId = exhibitionId;
            Name = name;
            Description = description;
            BackgroundImageURL = backgroundImageURL;
            StartDate = startDate;
            EndDate = endDate;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
        }

        // Active Record - Everything under line 31 is required for the active record implementation.
        private static string _connectionString = "Host=localhost;Database=Deakin University | AAGoA;Username=postgres;Password=postgreSQL;";

        public Exhibition() { }

        public List<ExhibitionOutputDto> GetExhibitions()
        {
            throw new NotImplementedException();
        }

        public ExhibitionOutputDto? GetExhibitionById(int id)
        {
            throw new NotImplementedException();
        }

        public ExhibitionArtworkOutputDto? GetExhibitionArtworksById(int id)
        {
            throw new NotImplementedException();
        }

        public ExhibitionInputDto? InsertExhibition(ExhibitionInputDto exhibition)
        {
            throw new NotImplementedException();
        }

        public ExhibitionInputDto? UpdateExhibition(int id, ExhibitionInputDto exhibition)
        {
            throw new NotImplementedException();
        }

        public bool DeleteExhibition(int id)
        {
            throw new NotImplementedException();
        }

        public ArtworkExhibitionDto? AllocateArtwork(int artworkId, int exhibitionId)
        {
            throw new NotImplementedException();
        }

        public bool DeallocateArtwork(int artworkId, int exhibitionId)
        {
            throw new NotImplementedException();
        }
    }
}
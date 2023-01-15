namespace Aboriginal_Art_Gallery_of_Australia.Models.Database_Models
{
    /// <summary>
    /// The Exhibition class is responsible for handling the database model associated with exhibitions. 
    /// </summary>
    public class Exhibition
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
    }
}
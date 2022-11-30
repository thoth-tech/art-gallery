namespace Aboriginal_Art_Gallery_of_Australia.Models.Database_Models
{
    public class Artwork
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Media { get; set; }
        public string PrimaryImageURL { get; set; }
        public string? SecondaryImageURL { get; set; }
        public int YearCreated { get; set; }
        public int NationID { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public Artwork(int id, string title, string description, string media, string primaryImageURL, string? secondaryImageURL, int yearCreated, int nationID, DateTime modifiedAt, DateTime createdAt)
        {
            Id = id;
            Title = title;
            Description = description;
            Media = media;
            PrimaryImageURL = primaryImageURL;
            SecondaryImageURL = secondaryImageURL;
            YearCreated = yearCreated;
            NationID = nationID;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
        }
    }
}

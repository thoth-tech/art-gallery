namespace Aboriginal_Art_Gallery_of_Australia.Models.Database_Models
{
    public class Artwork
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PrimaryImageURL { get; set; }
        public string SecondaryImageURL { get; set; }
        public int YearCreated { get; set; }
        public int MediaId { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public Artwork(int id, string title, string description, string primaryImageURL, string secondaryImageURL, int yearCreated, int mediaId, DateTime modifiedAt, DateTime createdAt)
        {
            Id = id;
            Title = title;
            Description = description;
            PrimaryImageURL = primaryImageURL;
            SecondaryImageURL = secondaryImageURL;
            YearCreated = yearCreated;
            MediaId = mediaId;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
        }
    }
}

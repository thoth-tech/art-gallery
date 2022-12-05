namespace Aboriginal_Art_Gallery_of_Australia.Models.Database_Models
{
    public class Exhibition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BackgroundImageUrl { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public Exhibition(int id, string name, string description, string backgroundImageUrl, DateTime modifiedAt, DateTime createdAt)
        {
            Id = id;
            Name = name;
            Description = description;
            BackgroundImageUrl = backgroundImageUrl;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
        }
    }
}
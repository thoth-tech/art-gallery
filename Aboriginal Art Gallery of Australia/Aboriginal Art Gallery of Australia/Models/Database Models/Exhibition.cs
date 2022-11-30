namespace Aboriginal_Art_Gallery_of_Australia.Models.Database_Models
{
    public class Exhibition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? BackdropImageURL { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public Exhibition(int id, string name, string description, string backdropImageURL, DateTime modifiedAt, DateTime createdAt)
        {
            Id = id;
            Name = name;
            Description = description;
            BackdropImageURL = backdropImageURL;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
        }
    }
}
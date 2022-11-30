namespace Aboriginal_Art_Gallery_of_Australia.Models.Database_Models
{
    public class Nation
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public Nation(int id, string title, DateTime modifiedAt, DateTime createdAt)
        {
            Id = id;
            Title = title;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
        }
    }
}
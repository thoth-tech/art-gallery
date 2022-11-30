namespace Aboriginal_Art_Gallery_of_Australia.Models.DTOs
{
    public class NationInputDto
    {
        public string Title { get; set; }

        public NationInputDto(string title)
        {
            Title = title;
        }
    }

    public class NationOutputDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public NationOutputDto(int id, string title, DateTime modifiedAt, DateTime createdAt)
        {
            Id = id;
            Title = title;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
        }
    }
}
namespace Aboriginal_Art_Gallery_of_Australia.Models.DTOs
{
    public class NationInputDto
    {
        public string Title { get; set; } = null!;

        public NationInputDto()
        {
        }

        public NationInputDto(string title)
        {
            Title = title;
        }
    }

    public class NationOutputDto
    {
        public int NationId { get; set; }
        public string Title { get; set; } = null!;
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public NationOutputDto()
        {
        }

        public NationOutputDto(int id, string title, DateTime modifiedAt, DateTime createdAt)
        {
            NationId = id;
            Title = title;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
        }
    }
}
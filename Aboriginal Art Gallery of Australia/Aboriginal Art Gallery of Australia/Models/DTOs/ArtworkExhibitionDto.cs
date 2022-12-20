namespace Aboriginal_Art_Gallery_of_Australia.Models.DTOs
{
    public class ArtworkExhibitionDto
    {
        public int ArtworkId { get; set; }
        public int ExhibitionName { get; set; }

        public ArtworkExhibitionDto(int artworkId, int exhibitionName)
        {
            this.ArtworkId = artworkId;
            this.ExhibitionName = exhibitionName;
        }

        public ArtworkExhibitionDto()
        {
        }
    }
}
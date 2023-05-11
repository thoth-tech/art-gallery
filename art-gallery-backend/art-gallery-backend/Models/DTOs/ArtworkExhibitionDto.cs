namespace Art_Gallery_Backend.Models.DTOs
{
    /// <summary>
    /// The ArtworkExhibitionDto class handles the transfer of entity ID data to facilitate relationships between artworks and exhibitions inside the database.
    /// </summary>
    public class ArtworkExhibitionDto
    {
        public int ArtworkId { get; set; }
        public int ExhibitionId { get; set; }

        public ArtworkExhibitionDto(int artworkId, int exhibitionId)
        {
            ArtworkId = artworkId;
            ExhibitionId = exhibitionId;
        }

        public ArtworkExhibitionDto() { }
    }
}

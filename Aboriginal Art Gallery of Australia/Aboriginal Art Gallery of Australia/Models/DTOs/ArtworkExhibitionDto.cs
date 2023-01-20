namespace Aboriginal_Art_Gallery_of_Australia.Models.DTOs
{
    /// <summary>
    /// The ArtworkExhibitionDto class handles the transfer of entity ID data to facilitate relationships between artworks and exhibitions inside the database.
    /// </summary>
    public class ArtworkExhibitionDto
    {
        public int ArtworkId { get; set; }
        public int ExhibitionName { get; set; }

        public ArtworkExhibitionDto(int artworkId, int exhibitionName)
        {
            ArtworkId = artworkId;
            ExhibitionName = exhibitionName;
        }

        public ArtworkExhibitionDto() { }
    }
}
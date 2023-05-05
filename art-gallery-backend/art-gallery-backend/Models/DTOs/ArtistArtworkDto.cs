namespace Art_Gallery_Backend.Models.DTOs
{
    /// <summary>
    /// The ArtistArtworkDto class handles the transfer of entity ID data to facilitate relationships between artists and artworks inside the database.
    /// </summary>
    public class ArtistArtworkDto
    {
        public int ArtistId { get; set; } = 0;
        public int ArtworkId { get; set; } = 0;

        public ArtistArtworkDto(int artistId, int artworkId)
        {
            ArtistId = artistId;
            ArtworkId = artworkId;
        }

        public ArtistArtworkDto() { }
    }
}

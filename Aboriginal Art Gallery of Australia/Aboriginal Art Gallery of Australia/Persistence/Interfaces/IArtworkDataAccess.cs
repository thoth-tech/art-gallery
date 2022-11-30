using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces
{
    public interface IArtworkDataAccess
    {
        /// <summary>
        /// Retrieves a complete list of artworks from the database/repository.
        /// </summary>
        /// <returns>Returns the collection of artwork as a list.</returns>
        List<ArtworkOutputDto> GetArtworks();

        /// <summary>
        /// Retrieves the artwork matching the specified ID from the database/repository.
        /// </summary>
        /// <returns>Returns the matching artwork if successful or null if unsuccessful.</returns>
        ArtworkOutputDto? GetArtworkById(int id);

        /// <summary>
        /// Inserts an artwork into the database/repository.
        /// </summary>
        /// <param name="artwork">The new artwork to be inserted.</param>
        /// <returns>Returns a copy of the inserted artwork if successful or null if unsuccessful.</returns>
        ArtworkInputDto? InsertArtwork(ArtworkInputDto artwork);

        /// <summary>
        /// Updates the artwork matching the specified ID in the database/repository.
        /// </summary>
        /// <param name="artwork">The updated artwork details</param>
        /// <returns>Returns a copy of the updated artwork if successful or null if unsuccessful.</returns>
        ArtworkInputDto? UpdateArtwork(int id, ArtworkInputDto artwork);

        /// <summary>
        /// Delete the artwork matching the specified ID from the database/repository.
        /// </summary>
        /// <param name="id">The specified artwork ID to be matched.</param>
        /// <returns>Returns the result of the delete operation as a boolean value.</returns>
        bool DeleteArtwork(int id);

        ArtistArtworkDto? AssignArtist(int artistId, int artworkId);
        bool DeassignArtist(int artistId, int artworkId);
        ArtworkExhibitionDto? AssignExhibition(int artworkId, int exhibitionId);
        bool DeassignExhibition(int artworkId, int exhibitionId);
    }
}

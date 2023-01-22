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
        /// Retrieves the artwork of the day from the database/repository.
        /// </summary>
        /// <returns>Returns the matching artwork if successful or null if unsuccessful.</returns>
        ArtworkOutputDto? GetArtworkOfTheDay();

        /// <summary>
        /// Inserts an artwork into the database/repository.
        /// </summary>
        /// <param name="artwork">The new artwork to be inserted.</param>
        /// <returns>Returns a copy of the inserted artwork if successful or null if unsuccessful.</returns>
        ArtworkInputDto? InsertArtwork(ArtworkInputDto artwork);

        /// <summary>
        /// Updates the artwork matching the specified ID in the database/repository.
        /// </summary>
        /// <param name="artwork">The updated artwork details.</param>
        /// <returns>Returns a copy of the updated artwork if successful or null if unsuccessful.</returns>
        ArtworkInputDto? UpdateArtwork(int id, ArtworkInputDto artwork);

        /// <summary>
        /// Delete the artwork matching the specified ID from the database/repository.
        /// </summary>
        /// <param name="id">The specified artwork ID to be matched.</param>
        /// <returns>Returns the result of the delete operation as a boolean value.</returns>
        bool DeleteArtwork(int id);

        /// <summary>
        /// Allocates the specified artists to the specified artwork in the database/repository.
        /// </summary>
        /// <param name="artistId">The Artist ID of the specified artist.</param>
        /// <param name="artworkId">The Artworks ID of the specified artwork.</param>
        /// <returns>Returns a copy of the updated record if successful or null if unsuccessful.</returns>
        ArtistArtworkDto? AllocateArtist(int artistId, int artworkId);

        /// <summary>
        /// Deallocates the specified artists to the specified artwork in the database/repository.
        /// </summary>
        /// <param name="artistId">The Artist ID of the specified artist.</param>
        /// <param name="artworkId">The Artwork ID of the specified artwork.</param>
        /// <returns>Returns the result of the deallocation operation as a boolean value.</returns>
        bool DeallocateArtist(int artistId, int artworkId);
    }
}

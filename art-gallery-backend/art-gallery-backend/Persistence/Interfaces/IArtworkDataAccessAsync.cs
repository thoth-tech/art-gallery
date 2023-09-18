using Art_Gallery_Backend.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Art_Gallery_Backend.Persistence.Interfaces
{
    /// <summary>
    /// The IArtworkDataAccess interface defines a contract. Any class that implements that contract must provide an implementation 
    /// of the members defined in the interface.
    /// </summary>
    public interface IArtworkDataAccessAsync
    {
        /// <summary>
        /// Retrieves a complete list of artworks from the database/repository.
        /// </summary>
        /// <returns>Returns the collection of artwork as a list.</returns>
        Task<List<ArtworkOutputDto>> GetArtworksAsync();

        /// <summary>
        /// Retrieves the artwork matching the specified ID from the database/repository.
        /// </summary>
        /// <returns>Returns the matching artwork if successful or null if unsuccessful.</returns>
        Task<ArtworkOutputDto?> GetArtworkByIdAsync(int id);

        /// <summary>
        /// Inserts an artwork into the database/repository.
        /// </summary>
        /// <param name="artwork">The new artwork to be inserted.</param>
        /// <returns>Returns a copy of the inserted artwork if successful or null if unsuccessful.</returns>
        Task<ArtworkInputDto?> InsertArtworkAsync(ArtworkInputDto artwork);

        /// <summary>
        /// Updates the artwork matching the specified ID in the database/repository.
        /// </summary>
        /// <param name="id">The ID of the artwork type to be updated.</param>
        /// <param name="artwork">The updated artwork details.</param>
        /// <returns>Returns a copy of the updated artwork if successful or null if unsuccessful.</returns>
        Task<ArtworkInputDto?> UpdateArtworkAsync(int id, ArtworkInputDto artwork);

        /// <summary>
        /// Delete the artwork matching the specified ID from the database/repository.
        /// </summary>
        /// <param name="id">The specified artwork ID to be matched.</param>
        /// <returns>Returns the result of the delete operation as a boolean value.</returns>
        Task<bool> DeleteArtworkAsync(int id);

        /// <summary>
        /// Allocates the specified artists to the specified artwork in the database/repository.
        /// </summary>
        /// <param name="artistId">The Artist ID of the specified artist.</param>
        /// <param name="artworkId">The Artworks ID of the specified artwork.</param>
        /// <returns>Returns a copy of the updated record if successful or null if unsuccessful.</returns>
        Task<ArtistArtworkDto?> AllocateArtistAsync(int artistId, int artworkId);

        /// <summary>
        /// Deallocates the specified artists to the specified artwork in the database/repository.
        /// </summary>
        /// <param name="artistId">The Artist ID of the specified artist.</param>
        /// <param name="artworkId">The Artwork ID of the specified artwork.</param>
        /// <returns>Returns the result of the deallocation operation as a boolean value.</returns>
        Task<bool> DeallocateArtistAsync(int artistId, int artworkId);
    }
}

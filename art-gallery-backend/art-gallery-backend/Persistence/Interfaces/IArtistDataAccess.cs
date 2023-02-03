using Art_Gallery_Backend.Models.DTOs;

namespace Art_Gallery_Backend.Persistence.Interfaces
{
    /// <summary>
    /// The IArtistDataAccess interface defines a contract. Any class that implements that contract must provide an implementation 
    /// of the members defined in the interface.
    /// </summary>
    public interface IArtistDataAccess
    {
        /// <summary>
        /// Retrieves a complete list of artists from the database/repository.
        /// </summary>
        /// <returns>Returns the collection of artist as a list.</returns>
        List<ArtistOutputDto> GetArtists();

        /// <summary>
        /// Retrieves the artist matching the specified ID from the database/repository.
        /// </summary>
        /// <returns>Returns the matching artist if successful or null if unsuccessful.</returns>
        ArtistOutputDto? GetArtistById(int id);

        /// <summary>
        /// Inserts an artist into the database/repository.
        /// </summary>
        /// <param name="artist">The new artist to be inserted.</param>
        /// <returns>Returns a copy of the inserted artist if successful or null if unsuccessful.</returns>
        ArtistInputDto? InsertArtist(ArtistInputDto artist);

        /// <summary>
        /// Updates the artist matching the specified ID in the database/repository.
        /// </summary>
        /// <param name="id">The ID of the artist to be updated.</param>
        /// <param name="artist">The updated artist details</param>
        /// <returns>Returns a copy of the updated artist if successful or null if unsuccessful.</returns>
        ArtistInputDto? UpdateArtist(int id, ArtistInputDto artist);

        /// <summary>
        /// Delete the artist matching the specified ID from the database/repository.
        /// </summary>
        /// <param name="id">The specified artist ID to be matched.</param>
        /// <returns>Returns the result of the delete operation as a boolean value.</returns>
        bool DeleteArtist(int id);
    }
}
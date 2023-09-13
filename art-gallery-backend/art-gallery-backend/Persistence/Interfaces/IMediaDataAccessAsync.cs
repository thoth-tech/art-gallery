using Art_Gallery_Backend.Models.DTOs;

namespace Art_Gallery_Backend.Persistence.Interfaces
{
    /// <summary>
    /// The IMediaDataAccess interface defines a contract. Any class that implements that contract must provide an implementation 
    /// of the members defined in the interface.
    /// </summary>
    public interface IMediaDataAccessAsync
    {
        /// <summary>
        /// Retrieves a complete list of media types from the database/repository.
        /// </summary>
        /// <returns>Returns the collection of media types as a list.</returns>
        Task<List<MediaOutputDto>> GetMediaTypesAsync();

        /// <summary>
        /// Retrieves the media type matching the specified ID from the database/repository.
        /// </summary>
        /// <returns>Returns the matching media type if successful or null if unsuccessful.</returns>
        Task<MediaOutputDto?> GetMediaTypeByIdAsync(int id);

        /// <summary>
        /// Inserts a media type into the database/repository.
        /// </summary>
        /// <param name="media">The new media type to be inserted.</param>
        /// <returns>Returns a copy of the inserted media type if successful or null if unsuccessful.</returns>
        Task<MediaInputDto?> InsertMediaTypeAsync(MediaInputDto media);

        /// <summary>
        /// Updates the media type matching the specified ID in the database/repository.
        /// </summary>
        /// <param name="id">The ID of the media type to be updated.</param>
        /// <param name="media">The updated media type details.</param>
        /// <returns>Returns a copy of the updated media type if successful or null if unsuccessful.</returns>
        Task<MediaInputDto?> UpdateMediaTypeAsync(int id, MediaInputDto media);

        /// <summary>
        /// Delete the media type matching the specified ID from the database/repository.
        /// </summary>
        /// <param name="id">The specified media type ID to be matched.</param>
        /// <returns>Returns the result of the delete operation as a boolean value.</returns>
        Task<bool> DeleteMediaTypeAsync(int id);
    }
}

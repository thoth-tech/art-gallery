using Art_Gallery_Backend.Models.DTOs;

namespace Art_Gallery_Backend.Persistence.Interfaces
{
    /// <summary>
    /// The IExhibitionDataAccess interface defines a contract. Any class that implements that contract must provide an implementation 
    /// of the members defined in the interface.
    /// </summary>
    public interface IExhibitionDataAccessAsync
    {
        /// <summary>
        /// Retrieves a complete list of exhibitions from the database/repository.
        /// </summary>
        /// <returns>Returns the collection of exhibition as a list.</returns>
        Task <List<ExhibitionOutputDto>> GetExhibitionsAsync();

        /// <summary>
        /// Retrieves the exhibition matching the specified ID from the database/repository.
        /// </summary>
        /// <returns>Returns the matching exhibition if successful or null if unsuccessful.</returns>
        Task<ExhibitionOutputDto?> GetExhibitionByIdAsync(int id);

        /// <summary>
        /// Retrieves the artworks from the exhibition matching the specified ID from the database/repository.
        /// </summary>
        /// <returns>Returns the matching exhibition and its assosiated artwork if successful or null if unsuccessful.</returns>
        Task<ExhibitionArtworkOutputDto?> GetExhibitionArtworksByIdAsync(int id);

        /// <summary>
        /// Inserts an exhibition into the database/repository.
        /// </summary>
        /// <param name="exhibition">The new exhibition to be inserted.</param>
        /// <returns>Returns a copy of the inserted exhibition if successful or null if unsuccessful.</returns>
        Task<ExhibitionInputDto?> InsertExhibitionAsync(ExhibitionInputDto exhibition);

        /// <summary>
        /// Updates the exhibition matching the specified ID in the database/repository.
        /// </summary>
        /// <param name="id">The ID of the exhibition to be updated.</param>
        /// <param name="exhibition">The updated exhibition details.</param>
        /// <returns>Returns a copy of the updated exhibition if successful or null if unsuccessful.</returns>
        Task<ExhibitionInputDto?> UpdateExhibitionAsync(int id, ExhibitionInputDto exhibition);

        /// <summary>
        /// Delete the exhibition matching the specified ID from the database/repository.
        /// </summary>
        /// <param name="id">The specified exhibition ID to be matched.</param>
        /// <returns>Returns the result of the delete operation as a boolean value.</returns>
        Task<bool> DeleteExhibitionAsync(int id);

        /// <summary>
        ///  Allocates the specified artwork to the specified exhibition in the database/repository.
        /// </summary>
        /// <param name="artworkId">The Artist ID of the specified artist.</param>
        /// <param name="exhibitionId">The Exhibition ID of the specified exhibition.</param>
        /// <returns>Returns a copy of the updated record if successful or null if unsuccessful.</returns>
        Task<ArtworkExhibitionDto?> AllocateArtworkAsync(int artworkId, int exhibitionId);

        /// <summary>
        /// Deallocates the specified artwork from the specified exhibition in the database/repository.
        /// </summary>
        /// <param name="artworkId">The Artist ID of the specified artist.</param>
        /// <param name="exhibitionId">The Exhibition ID of the specified exhibition.</param>
        /// <returns>Returns the result of the deallocation operation as a boolean value.</returns>
        Task<bool> DeallocateArtworkAsync(int artworkId, int exhibitionId);
    }
}

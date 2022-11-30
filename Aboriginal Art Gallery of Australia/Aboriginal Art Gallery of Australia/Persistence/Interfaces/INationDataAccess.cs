using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces
{
    public interface INationDataAccess
    {
        /// <summary>
        /// Retrieves a complete list of nations from the database/repository.
        /// </summary>
        /// <returns>Returns the collection of nation as a list.</returns>
        List<NationOutputDto> GetNations();

        /// <summary>
        /// Retrieves the nation matching the specified ID from the database/repository.
        /// </summary>
        /// <returns>Returns the matching nation if successful or null if unsuccessful.</returns>
        NationOutputDto? GetNationById(int id);

        /// <summary>
        /// Inserts an nation into the database/repository.
        /// </summary>
        /// <param name="nation">The new nation to be inserted.</param>
        /// <returns>Returns a copy of the inserted nation if successful or null if unsuccessful.</returns>
        NationInputDto? InsertNation(NationInputDto nation);

        /// <summary>
        /// Updates the nation matching the specified ID in the database/repository.
        /// </summary>
        /// <param name="nation">The updated nation details</param>
        /// <returns>Returns a copy of the updated nation if successful or null if unsuccessful.</returns>
        NationInputDto? UpdateNation(int id, NationInputDto nation);

        /// <summary>
        /// Delete the nation matching the specified ID from the database/repository.
        /// </summary>
        /// <param name="id">The specified nation ID to be matched.</param>
        /// <returns>Returns the result of the delete operation as a boolean value.</returns>
        bool DeleteNation(int id);
    }
}
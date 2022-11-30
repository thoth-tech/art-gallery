using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces
{
    public interface IExhibitionDataAccess
    {
        /// <summary>
        /// Retrieves a complete list of exhibitions from the database/repository.
        /// </summary>
        /// <returns>Returns the collection of exhibition as a list.</returns>
        List<ExhibitionOutputDto> GetExhibitions();

        /// <summary>
        /// Retrieves the exhibition matching the specified ID from the database/repository.
        /// </summary>
        /// <returns>Returns the matching exhibition if successful or null if unsuccessful.</returns>
        ExhibitionOutputDto? GetExhibitionById(int id);

        /// <summary>
        /// Retrieves the artworks from the exhibition matching the specified ID from the database/repository.
        /// </summary>
        /// <returns>Returns the matching exhibition and its assosiated artwork if successful or null if unsuccessful.</returns>
        ExhibitionArtworkOutputDto? GetExhibitionArtworksById(int id);

        /// <summary>
        /// Inserts an exhibition into the database/repository.
        /// </summary>
        /// <param name="exhibition">The new exhibition to be inserted.</param>
        /// <returns>Returns a copy of the inserted exhibition if successful or null if unsuccessful.</returns>
        ExhibitionInputDto? InsertExhibition(ExhibitionInputDto exhibition);

        /// <summary>
        /// Updates the exhibition matching the specified ID in the database/repository.
        /// </summary>
        /// <param name="exhibition">The updated exhibition details</param>
        /// <returns>Returns a copy of the updated exhibition if successful or null if unsuccessful.</returns>
        ExhibitionInputDto? UpdateExhibition(int id, ExhibitionInputDto exhibition);

        /// <summary>
        /// Delete the exhibition matching the specified ID from the database/repository.
        /// </summary>
        /// <param name="id">The specified exhibition ID to be matched.</param>
        /// <returns>Returns the result of the delete operation as a boolean value.</returns>
        bool DeleteExhibition(int id);

        ArtworkExhibitionDto? AssignArtwork(int artworkId, int exhibitionId);
        bool DeassignArtwork(int artworkId, int exhibitionId);
    }
}
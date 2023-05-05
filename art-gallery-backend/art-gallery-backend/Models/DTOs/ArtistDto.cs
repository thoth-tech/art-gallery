namespace Art_Gallery_Backend.Models.DTOs
{
    /// <summary>
    /// The ArtistInputDto class is used to decouple the service layer from the database layer. It provides a means of mapping the necessary user input to the appropriate database model.
    /// </summary>
    public class ArtistInputDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string ProfileImageUrl { get; set; } = string.Empty;
        public string PlaceOfBirth { get; set; } = string.Empty;
        public int? YearOfBirth { get; set; } = 0;
        public int? YearOfDeath { get; set; } = null;

        public ArtistInputDto(string firstName, string lastName, string? displayName, string profileImageURL, string placeOfBirth, int? yearOfBirth, int? yearOfDeath)
        {
            if (displayName is null or "")
            {
                displayName = $"{firstName} {lastName}";
            }

            FirstName = firstName;
            LastName = lastName;
            DisplayName = displayName;
            ProfileImageUrl = profileImageURL;
            PlaceOfBirth = placeOfBirth;
            YearOfBirth = yearOfBirth;
            YearOfDeath = yearOfDeath;
        }

        public ArtistInputDto() { }
    }

    /// <summary>
    /// The ArtistOutputDto class is used to decouple the database layer from the service layer. It provides a means of mapping the necessary information from the database to the appropriate user output.
    /// </summary>
    public class ArtistOutputDto
    {
        public int ArtistId { get; set; } = 0;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string ProfileImageUrl { get; set; } = string.Empty;
        public string PlaceOfBirth { get; set; } = string.Empty;
        public int? YearOfBirth { get; set; } = 0;
        public int? YearOfDeath { get; set; } = null;
        public DateTime ModifiedAt { get; set; } = new DateTime();
        public DateTime CreatedAt { get; set; } = new DateTime();

        public ArtistOutputDto(int artistId, string firstName, string lastName, string displayName, string profileImageURL, string placeOfBirth, int? yearOfBirth, int? yearOfDeath, DateTime modifiedAt, DateTime createdAt)
        {
            ArtistId = artistId;
            FirstName = firstName;
            LastName = lastName;
            DisplayName = displayName;
            ProfileImageUrl = profileImageURL;
            PlaceOfBirth = placeOfBirth;
            YearOfBirth = yearOfBirth;
            YearOfDeath = yearOfDeath;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
        }

        public ArtistOutputDto() { }
    }
}

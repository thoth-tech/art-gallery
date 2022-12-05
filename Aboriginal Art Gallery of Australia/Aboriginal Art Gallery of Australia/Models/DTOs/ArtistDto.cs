namespace Aboriginal_Art_Gallery_of_Australia.Models.DTOs
{
    public class ArtistInputDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string ProfileImageURL { get; set; }
        public string PlaceOfBirth { get; set; }
        public int YearOfBirth { get; set; }
        public int? YearOfDeath { get; set; } = null;

        public ArtistInputDto(string firstName, string lastName, string displayName, string profileImageURL, string placeOfBirth, int yearOfBirth, int? yearOfDeath)
        {
            FirstName = firstName;
            LastName = lastName;
            DisplayName = displayName;
            ProfileImageURL = profileImageURL;
            PlaceOfBirth = placeOfBirth;
            YearOfBirth = yearOfBirth;
            YearOfDeath = yearOfDeath;
        }
    }

    public class ArtistOutputDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string ProfileImageURL { get; set; }
        public string PlaceOfBirth { get; set; }
        public int YearOfBirth { get; set; }
        public int? YearOfDeath { get; set; } = null;
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public ArtistOutputDto(int id, string firstName, string lastName, string displayName, string profileImageURL, string placeOfBirth, int yearOfBirth, int? yearOfDeath, DateTime modifiedAt, DateTime createdAt)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DisplayName = displayName;
            ProfileImageURL = profileImageURL;
            PlaceOfBirth = placeOfBirth;
            YearOfBirth = yearOfBirth;
            YearOfDeath = yearOfDeath;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
        }
    }
}
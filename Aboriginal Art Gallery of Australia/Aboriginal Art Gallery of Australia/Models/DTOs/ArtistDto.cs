namespace Aboriginal_Art_Gallery_of_Australia.Models.DTOs
{
    public class ArtistInputDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string PlaceOfBirth { get; set; } = null!;
        public int YearOfBirth { get; set; }
        public int? YearOfDeath { get; set; } = null;

        public ArtistInputDto()
        {
        }

        public ArtistInputDto(string firstName, string lastName, string displayName, string placeOfBirth, int yearOfBirth, int? yearOfDeath)
        {
            FirstName = firstName;
            LastName = lastName;
            DisplayName = displayName;
            PlaceOfBirth = placeOfBirth;
            YearOfBirth = yearOfBirth;
            YearOfDeath = yearOfDeath;
        }
    }

    public class ArtistOutputDto
    {
        public int ArtistId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string PlaceOfBirth { get; set; } = null!;
        public int YearOfBirth { get; set; }
        public int? YearOfDeath { get; set; } = null;
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public ArtistOutputDto()
        {
        }

        public ArtistOutputDto(int id, string firstName, string lastName, string displayName, string placeOfBirth, int yearOfBirth, int? yearOfDeath, DateTime modifiedAt, DateTime createdAt)
        {
            ArtistId = id;
            FirstName = firstName;
            LastName = lastName;
            DisplayName = displayName;
            PlaceOfBirth = placeOfBirth;
            YearOfBirth = yearOfBirth;
            YearOfDeath = yearOfDeath;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
        }
    }
}
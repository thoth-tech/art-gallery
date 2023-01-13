namespace Aboriginal_Art_Gallery_of_Australia.Models.Database_Models
{
    public class Artist
    {
        public int ArtistId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string ProfileImageURL { get; set; }
        public string PlaceOfBirth { get; set; }
        public int YearOfBirth { get; set; }
        public int? YearOfDeath { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public Artist(int artistId, string firstName, string lastName, string displayName, string profileImageURL, string placeOfBirth, int yearOfBirth, int? yearOfDeath, DateTime modifiedAt, DateTime createdAt)
        {
            ArtistId = artistId;
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
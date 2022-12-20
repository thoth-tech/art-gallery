using Aboriginal_Art_Gallery_of_Australia.Models.Database_Models;
using System.ComponentModel.DataAnnotations;

namespace Aboriginal_Art_Gallery_of_Australia.Models.DTOs
{
    public class ArtistInputDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string ProfileImageURL { get; set; }
        public string PlaceOfBirth { get; set; }
        public int? YearOfBirth { get; set; }
        public int? YearOfDeath { get; set; } = null;

        public ArtistInputDto(){
            this.FirstName = "";
            this.LastName = "";
            this.DisplayName = "";
            this.ProfileImageURL = "";
            this.PlaceOfBirth = "";
        }

        public ArtistInputDto(string firstName, string lastName, string? displayName, string profileImageURL, string placeOfBirth, int? yearOfBirth, int? yearOfDeath)
        {
            if (displayName == null || displayName == "")
                displayName = $"{firstName} {lastName}";

            this.FirstName = firstName;
            this.LastName = lastName;
            this.DisplayName = displayName;
            this.ProfileImageURL = profileImageURL;
            this.PlaceOfBirth = placeOfBirth;
            this.YearOfBirth = yearOfBirth;
            this.YearOfDeath = yearOfDeath;
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

        public ArtistOutputDto(){
            this.FirstName = "";
            this.LastName = "";
            this.DisplayName = "";
            this.ProfileImageURL = "";
            this.PlaceOfBirth = "";
        }

        public ArtistOutputDto(int id, string firstName, string lastName, string displayName, string profileImageURL, string placeOfBirth, int yearOfBirth, int? yearOfDeath, DateTime modifiedAt, DateTime createdAt)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DisplayName = displayName;
            this.ProfileImageURL = profileImageURL;
            this.PlaceOfBirth = placeOfBirth;
            this.YearOfBirth = yearOfBirth;
            this.YearOfDeath = yearOfDeath;
            this.ModifiedAt = modifiedAt;
            this.CreatedAt = createdAt;
        }
    }
}
using Art_Gallery_Backend.Middleware;
using Art_Gallery_Backend.Models.Database_Models;
using Art_Gallery_Backend.Models.DTOs;
using Art_Gallery_Backend.Persistence;
using Art_Gallery_Backend.Persistence.Implementations.ADO;
using Art_Gallery_Backend.Persistence.Implementations.RP;
using Art_Gallery_Backend.Persistence.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

/*
 Register Services to the container below.
 */

#region JWT Token authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});
#endregion

#region Authorisation
var securityScheme = new OpenApiSecurityScheme()
{
    Name = "Authorization",
    Type = SecuritySchemeType.Http,
    Scheme = "bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "Please enter a valid token",
};

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Admin", "User"));
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
});

var securityRequirement = new OpenApiSecurityRequirement()
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "bearer"
            }
        },
        new string[] {}
    }
};
#endregion

#region Swagger UI
builder.Services.AddDateOnlyTimeOnlyStringConverters();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DDGCIT Gallery API",
        Version = "1.0.0",
        Description = "A new backend services for the DDGCIT Art Gallery.",
        Contact = new OpenApiContact
        {
            Name = "John Doe",
            Email = "jdoe@deakin.edu.au"
        }
    });
    options.AddSecurityDefinition("bearer", securityScheme);
    options.AddSecurityRequirement(securityRequirement);
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    options.UseDateOnlyTimeOnlyStringConverters();
});
#endregion

// Middleware Services
builder.Services.AddSingleton<ArtistOfTheDayMiddleware>();
builder.Services.AddSingleton<ArtworkOfTheDayMiddleware>();

/*
 Swap between implementations using dependency injection, simply uncomment them below;
 */

// Implementation 1 - ADO
// builder.Services.AddScoped<IArtistDataAccess, ArtistADO>();
// builder.Services.AddScoped<IArtworkDataAccess, ArtworkADO>();
// builder.Services.AddScoped<IExhibitionDataAccess, ExhibitionADO>();
// builder.Services.AddScoped<IMediaDataAccess, MediaADO>();
// builder.Services.AddScoped<IUserDataAccess, UserADO>();

// Implementation 2 - Repository Pattern
builder.Services.AddScoped<IArtistDataAccessAsync, ArtistRepository>();
builder.Services.AddScoped<IArtworkDataAccessAsync, ArtworkRepository>();
builder.Services.AddScoped<IExhibitionDataAccess, ExhibitionRepository>();
builder.Services.AddScoped<IMediaDataAccess, MediaRepository>();
builder.Services.AddScoped<IUserDataAccess, UserRepository>();

// Implementation 3 - Active Record
//builder.Services.AddScoped<IArtistDataAccess, Artist>();
//builder.Services.AddScoped<IArtworkDataAccess, Artwork>();
//builder.Services.AddScoped<IExhibitionDataAccess, Exhibition>();
//builder.Services.AddScoped<IMediaDataAccess, Media>();
//builder.Services.AddScoped<IUserDataAccess, User>();

// builder.Services.AddAuthorization();

var app = builder.Build();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(setup =>
    {
        setup.InjectStylesheet("/styles/theme-deakin.css");
        setup.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });

}

// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


#region Map Artist Endpoint

/// <summary>
/// Gets all artists.
/// </summary>
/// <param name="_artistRepo"> The repository providing data access methods for the artist context. </param>
/// <returns> A list of all artists. </returns>
/// <response code="200"> Returns a list of all artists, or an empty
/// list if there are currently none stored. </response>
app.MapGet("api/artists/", async (IArtistDataAccessAsync _artistRepo) => await _artistRepo.GetArtistsAsync());

/// <summary>
/// Gets an artist with the specified id.
/// </summary>
/// <param name="_artistRepo"> The repository providing data access methods for the artist context. </param>
/// <param name="artistId"> The id for the artist to be returned (taken from the request URL). </param>
/// <returns> An artist with the specified id. </returns>
/// <response code="200"> Returns the artist with the specified id. </response>
/// <response code="404"> If no artist with the specified id exitst. </response>
app.MapGet("api/artists/{artistId}", async (IArtistDataAccessAsync _artistRepo, int artistId) =>
{
    var result = await _artistRepo.GetArtistByIdAsync(artistId);

    if (result == null) return Results.NotFound($"No artist can be found with an {nameof(artistId)} of {artistId}");

    return Results.Ok(result);
});

/// <summary>
/// Gets the artist of the day.
/// </summary>
/// <param name="_showcase"> The repository providing data access methods from the Artist Of The Day middleware. </param>
/// <param name="_artistRepo"> The repository providing data access methods for the artist context. </param>
/// <returns> The artist of the day. </returns>
/// <response code="200"> Returns the artist of the day. </response>
app.MapGet("api/artists/of-the-day", async ([FromServices] ArtistOfTheDayMiddleware _showcase, IArtistDataAccessAsync _artistRepo) => 
_showcase.GetArtistOfTheDay(await _artistRepo.GetArtistsAsync()));

/// <summary>
/// Adds a new artist.
/// </summary>
/// <param name="_artistRepo"> The repository providing data access methods for the artist context. </param>
/// <param name="artist"> A new artist (from the HTTP request body). </param>
/// <returns> The newly added artist. </returns>
/// <remarks>
/// Sample request body:
///
///     POST /api/artists/
///     {
///         "fistName": "John",
///         "lastName": "Smith",
///         "displayName": "John",
///         "profileImageUrl": "https://www.sample.url/picture.jpg",
///         "placeOfBirth": "Melbourne",
///         "yearOfBirth": 1980,
///         "yearOfDeath": 2020
///     }
///
/// </remarks>
/// <response code="200"> Returns the newly added artist. </response>
/// <response code="400"> If the request body contains any null values, the ProfileImageUrl is not in the correct format,
/// the birth or death date is greater than the current date, or if the death date is later than the birth date. </response>
app.MapPost("api/artists/", [Authorize] async (IArtistDataAccessAsync _artistRepo, ArtistInputDto artist) =>
{
    PropertyInfo[] properties = artist.GetType().GetProperties();
    foreach (PropertyInfo property in properties)
    {
        var propertyValue = property.GetValue(artist, null);

        if (property.PropertyType == typeof(string))
        {
            if (propertyValue == null || propertyValue.Equals(""))
                return Results.BadRequest($"A {property.Name} is required.");

            if (property.Name.Contains(nameof(artist.ProfileImageUrl)) && propertyValue.ToString()!.IsValidURL() == false)
                return Results.BadRequest($"An absolute {property.Name} is required in the following format: https://www.sample.url/picture.jpg");
        }

        if (property.PropertyType == typeof(int?))
        {
            if (propertyValue != null && ((int)propertyValue > DateTime.Today.Year))
                return Results.BadRequest($"{property.Name} can not be greater then {DateTime.Today.Year}.");

            if (property.Name.Contains(nameof(artist.YearOfBirth)) && propertyValue == null)
                return Results.BadRequest($"A {property.Name} is required.");

            if (property.Name.Contains(nameof(artist.YearOfDeath)) && propertyValue != null && ((int)propertyValue <= artist.YearOfBirth))
                return Results.BadRequest($"The {property.Name} can not be before the year of birth.");
        }
    }

    var result = await _artistRepo.InsertArtistAsync(artist);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue creating this database entry.");
});

/*
 * Without Reflection
 * 
app.MapPost("api/artists/", [Authorize] (IArtistDataAccess _artistRepo, ArtistInputDto artist) =>
{
    if (artist is not null)
    {
        if (artist.FirstName.IsNullOrEmpty()) return Results.BadRequest($"Artist {nameof(artist.FirstName)} is required.");
        else if (artist.LastName.IsNullOrEmpty()) return Results.BadRequest($"Artist {nameof(artist.LastName)} is required.");
        else if (artist.DisplayName.IsNullOrEmpty()) return Results.BadRequest($"Artist {nameof(artist.DisplayName)} is required.");
        else if (artist.ProfileImageUrl.IsNullOrEmpty()) return Results.BadRequest($"Artist {nameof(artist.ProfileImageUrl)} is required.");
        else if (artist.PlaceOfBirth.IsNullOrEmpty()) return Results.BadRequest($"Artist {nameof(artist.PlaceOfBirth)} is required.");

        if (!artist.ProfileImageUrl.IsValidURL()) return Results.BadRequest($"An absolute {nameof(artist.ProfileImageUrl)} is " +
        $"required in the following format: https://www.sample.url/picture.jpg");

        if (artist.YearOfBirth is not null && artist.YearOfDeath is not null)
        {
            if (artist.YearOfBirth > DateTime.Today.Year || artist.YearOfDeath > DateTime.Today.Year)
                return Results.BadRequest($"The entered years can not be greater than {DateTime.Today.Year}.");

            if (artist.YearOfDeath <= artist.YearOfBirth)
                return Results.BadRequest($"The {nameof(artist.YearOfDeath)} can not be before the {nameof(artist.YearOfBirth)}.");
        }
        else if (artist.YearOfBirth is null) return Results.BadRequest($"A {nameof(artist.YearOfBirth)} is required.");
    }
    else return Results.BadRequest($"Artist details required.");

    var result = _artistRepo.InsertArtist(artist);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue creating this database entry.");
});
*/

/// <summary>
/// Updates an artist.
/// </summary>
/// <param name="_artistRepo"> The repository providing data access methods for the artist context. </param>
/// <param name="artistId"> The id for the artist we are looking to update (from the request URL). </param>
/// <param name="artist"> Updated details for the artist (from the HTTP request body). </param>
/// <returns> No content. </returns>
/// <remarks>
/// Sample request body:
///
///     PUT /api/artists/{artistId}
///     {
///         "fistName": "John",
///         "lastName": "Smith",
///         "displayName": "John",
///         "profileImageUrl": "https://www.sample.url/picture.jpg",
///         "placeOfBirth": "Sydney",
///         "yearOfBirth": 1970,
///         "yearOfDeath": 2022
///     }
///
/// </remarks>
/// <response code="204"> If the artist is udpated successfully. </response>
/// <response code="404"> If the specified id is not associated with any artist. </response>
/// <response code="400"> If the request body contains any null values, the ProfileImageUrl is not in the correct format,
/// the birth or death date is greater than the current date, or if the death date is later than the birth date.  </response>
app.MapPut("api/artists/{artistId}", [Authorize] async (IArtistDataAccessAsync _artistRepo, int artistId, ArtistInputDto artist) =>
{
    if (await _artistRepo.GetArtistByIdAsync(artistId) == null)
        return Results.NotFound($"No artist can be found with an {nameof(artistId)} of {artistId}");

    PropertyInfo[] properties = artist.GetType().GetProperties();
    foreach (PropertyInfo property in properties)
    {
        var propertyValue = property.GetValue(artist, null);

        if (property.PropertyType == typeof(string) && propertyValue != null && !propertyValue.Equals(""))
        {
            if (property.Name.Contains(nameof(artist.ProfileImageUrl)) && propertyValue.ToString()!.IsValidURL() == false)
                return Results.BadRequest($"An absolute {property.Name} is required in the following format: https://www.sample.url/picture.jpg");
        }

        if (property.PropertyType == typeof(int?))
        {
            if (propertyValue != null && ((int)propertyValue > DateTime.Today.Year))
                return Results.BadRequest($"{property.Name} can not be greater then {DateTime.Today.Year}.");

            if (property.Name.Contains(nameof(artist.YearOfDeath)) && propertyValue != null && ((int)propertyValue <= artist.YearOfBirth))
                return Results.BadRequest($"The {property.Name} can not be before the year of birth.");
        }
    }

    var result = await _artistRepo.UpdateArtistAsync(artistId, artist);
    return result is not null ? Results.NoContent() : Results.BadRequest("There was an issue updating this database entry.");
});

/*
 * Without Reflection
 * 
app.MapPut("api/artists/{artistId}", [Authorize] (IArtistDataAccess _artistRepo, int artistId, ArtistInputDto artist) =>
{
    if (_artistRepo.GetArtistById(artistId) == null)
        return Results.NotFound($"No artist can be found with an {nameof(artistId)} of {artistId}");

    if (artist is not null)
    {
        if (!artist.ProfileImageUrl.IsValidURL()) return Results.BadRequest($"An absolute {nameof(artist.ProfileImageUrl)} is " +
        $"required in the following format: https://www.sample.url/picture.jpg");

        if (artist.YearOfBirth is not null && artist.YearOfDeath is not null)
        {
            if (artist.YearOfBirth > DateTime.Today.Year || artist.YearOfDeath > DateTime.Today.Year)
                return Results.BadRequest($"The entered years can not be greater than {DateTime.Today.Year}.");

            if (artist.YearOfDeath <= artist.YearOfBirth)
                return Results.BadRequest($"The {artist.YearOfDeath} can not be before the {artist.YearOfBirth}.");
        }
    }
    else return Results.BadRequest($"A missing value is required.");

    var result = _artistRepo.UpdateArtist(artistId, artist);
    return result is not null ? Results.NoContent() : Results.BadRequest("There was an issue updating this database entry.");
});
*/

/// <summary>
/// Deletes an artist with the specified id.
/// </summary>
/// <param name="_artistRepo"> The repository providing data access methods for the artist context. </param>
/// <param name="artistId"> The id for the artist we are trying to delete (from the request URL). </param>
/// <returns> No content. </returns>
/// <response code="204"> No content. </response>
/// <response code="404"> If no artist with the specified id exits. </response>
/// <response code="400"> If there is an issues executing the query in the database. </response>
app.MapDelete("api/artists/{artistId}", [Authorize] async (IArtistDataAccessAsync _artistRepo, int artistId) =>
{
    if (await _artistRepo.GetArtistByIdAsync(artistId) == null)
        return Results.NotFound($"No artist can be found with an ID of {artistId}");

    var result = await _artistRepo.DeleteArtistAsync(artistId);
    return result is true ? Results.NoContent() : Results.BadRequest("There was an issue deleting this database entry.");
});

#endregion

#region Map Artwork Endpoints

/// <summary>
/// Gets all artworks.
/// </summary>
/// <param name="_artworkRepo"> The repository providing data access methods for the artwork context. </param>
/// <returns> A list of all artworks. </returns>
/// <response code="200"> Returns a list of all artworks, or an empty
/// list if there are currently none stored. </response>
app.MapGet("api/artworks/", async (IArtworkDataAccessAsync _artworkRepo) => await _artworkRepo.GetArtworksAsync());

/// <summary>
/// Gets an artwork with the specified id.
/// </summary>
/// <param name="_artworkRepo"> The repository providing data access methods for the artwork context. </param>
/// <param name="artworkId"> The id for the artwork to be returned (taken from the request URL). </param>
/// <returns> An artwork with the specified id. </returns>
/// <response code="200"> Returns the artwork with the specified id. </response>
/// <response code="404"> If no artwork with the specified id exitst. </response>
app.MapGet("api/artworks/{artworkId}", async (IArtworkDataAccessAsync _artworkRepo, int artworkId) =>
{
    var result = await _artworkRepo.GetArtworkByIdAsync(artworkId);
    return result is not null ? Results.Ok(result) : Results.NotFound($"No artwork can be found with an {nameof(artworkId)} of {artworkId}");
});

/// <summary>
/// Gets the artwork of the day.
/// </summary>
/// <param name="_showcase"> The repository providing data access methods from the Artwork Of The Day middleware. </param>
/// <param name="_artworkRepo"> The repository providing data access methods for the artwork context. </param>
/// <returns> The artwork of the day. </returns>
/// <response code="200"> Returns the artwork of the day. </response>
app.MapGet("api/artworks/of-the-day", async ([FromServices] ArtworkOfTheDayMiddleware _showcase, IArtworkDataAccessAsync _artworkRepo) => 
_showcase.GetArtworkOfTheDay(await _artworkRepo.GetArtworksAsync()));

/// <summary>
/// Adds a new artwork.
/// </summary>
/// <param name="_artworkRepo"> The repository providing data access methods for the artwork context. </param>
/// <param name="_mediaRepo"> The repository providing data access methods for the media context. </param>
/// <param name="artwork"> A new artwork (from the HTTP request body). </param>
/// <returns> The newly added artwork. </returns>
/// <remarks>
/// Sample request body:
///
///     POST /api/artworks/
///     {
///         "title": "Example Title",
///         "description": "Example Description",
///         "primaryImageUrl": "https://www.sample.url/picture.jpg",
///         "secondaryImageUrl": "https://www.sample.url/picture.jpg",
///         "yearCreated": 2000,
///         "mediaId": 1,
///         "price": 100.00
///     }
///
/// </remarks>
/// <response code="200"> Returns the newly added artwork. </response>
/// <response code="400"> If the request body contains any null values, the PrimaryImageUrl or SecondaryImageUrl are not in the correct format,
/// the year created is greater than the current year, or if the associated media type does not exist. </response>
app.MapPost("api/artworks/", [Authorize] async (IArtworkDataAccessAsync _artworkRepo, IMediaDataAccess _mediaRepo, ArtworkInputDto artwork) =>
{
    PropertyInfo[] properties = artwork.GetType().GetProperties();
    foreach (PropertyInfo property in properties)
    {
        var propertyValue = property.GetValue(artwork, null);

        if (property.PropertyType == typeof(string))
        {
            if (propertyValue == null || propertyValue.Equals(""))
                if (property.Name.Contains("Primary"))
                    return Results.BadRequest($"A {property.Name} is required.");

            if (property.Name.Contains("Url") && propertyValue != null)
                if (propertyValue.ToString()!.IsValidURL() == false)
                    return Results.BadRequest($"An absolute {property.Name} is required in the following format: https://www.sample.url/picture.jpg");
        }

        if (property.PropertyType == typeof(int?))
        {
            if (propertyValue == null || propertyValue.Equals(""))
                return Results.BadRequest($"A {property.Name} is required.");

            if (property.Name.Contains(nameof(artwork.YearCreated)) && ((int)propertyValue > DateTime.Today.Year))
                return Results.BadRequest($"{property.Name} can not be greater than {DateTime.Today.Year}.");

            if (property.Name.Contains(nameof(artwork.MediaId)) && (_mediaRepo.GetMediaTypeById((int)propertyValue) == null))
                return Results.BadRequest($"No mediatype can be found with an {property.Name} of {propertyValue}");
        }

        // if (property.Name.Contains(nameof(artwork.Price)) && property.PropertyType is not double)
        //     return Results.BadRequest($"A decimal value {property.Name} is required.");
    }
    var result = await _artworkRepo.InsertArtworkAsync(artwork);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue creating this database entry.");
});

/*
 * Without Reflection
 * 
app.MapPost("api/artworks/", [Authorize] (IArtworkDataAccess _artworkRepo, IMediaDataAccess _mediaRepo, ArtworkInputDto artwork) =>
{
    if (artwork is not null)
    {
        if (artwork.Title.IsNullOrEmpty()) return Results.BadRequest($"Artwork {nameof(artwork.Title)} is required.");
        else if (artwork.Description.IsNullOrEmpty()) return Results.BadRequest($"Artwork {nameof(artwork.Description)} is required.");
        else if (artwork.PrimaryImageUrl.IsNullOrEmpty()) return Results.BadRequest($"Artwork {nameof(artwork.PrimaryImageUrl)} is required.");
        else if (artwork.SecondaryImageUrl.IsNullOrEmpty()) return Results.BadRequest($"Artwork {nameof(artwork.SecondaryImageUrl)} is required.");
        else if (artwork.MediaId == null) return Results.BadRequest($"Artwork {nameof(artwork.MediaId)} is required.");

        if (!artwork.PrimaryImageUrl.IsValidURL() || !artwork.SecondaryImageUrl.IsValidURL())
            return Results.BadRequest($"An absolute primary or secondary image url is required in the following format: https://www.sample.url/picture.jpg");

        if (_mediaRepo.GetMediaTypeById((int)artwork.MediaId) == null)
            return Results.NotFound($"No mediatype can be found with an {nameof(artwork.MediaId)} of {artwork.MediaId}");

        if (artwork.YearCreated is not null)
        {
            if (artwork.YearCreated > DateTime.Today.Year)
                return Results.BadRequest($"The entered {nameof(artwork.YearCreated)} can not be greater than {DateTime.Today.Year}.");
        }
        else return Results.BadRequest($"A {nameof(artwork.YearCreated)} value is required.");
    }
    else return Results.BadRequest($"Artwork details required.");

    var result = _artworkRepo.InsertArtwork(artwork);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue creating this database entry.");
});
*/

/// <summary>
/// Updates an artwork.
/// </summary>
/// <param name="_artworkRepo"> The repository providing data access methods for the artwork context. </param>
/// <param name="_mediaRepo"> The repository providing data access methods for the media context. </param>
/// <param name="artworkId"> The id for the artwork we are looking to update (from the request URL). </param>
/// <param name="artwork"> Updated details for the artwork (from the HTTP request body). </param>
/// <returns> No content. </returns>
/// <remarks>
/// Sample request body:
///
///     PUT /api/artworks/{artworkId}
///     {
///         "title": "Example Title",
///         "description": "Example Description",
///         "primaryImageUrl": "https://www.sample.url/picture.jpg",
///         "secondaryImageUrl": "https://www.sample.url/picture.jpg",
///         "yearCreated": 2010,
///         "mediaId": 2,
///         "price": 100.00
///     }
///
/// </remarks>
/// <response code="204"> If the artwork is udpated successfully. </response>
/// <response code="404"> If the specified id is not associated with any artwork. </response>
/// <response code="400"> If the request body contains any null values, the PrimaryImageUrl or SecondaryImageUrl are not in the correct format,
/// the year created is greater than the current year, or if the associated media type does not exist. </response>
app.MapPut("api/artworks/{artworkId}", [Authorize] async (IArtworkDataAccessAsync _artworkRepo, IMediaDataAccess _mediaRepo, int artworkId, ArtworkInputDto artwork) =>
{
    if (await _artworkRepo.GetArtworkByIdAsync(artworkId) == null)
        return Results.NotFound($"No artwork can be found with an {nameof(artworkId)} of {artworkId}");

    PropertyInfo[] properties = artwork.GetType().GetProperties();
    foreach (PropertyInfo property in properties)
    {
        var propertyValue = property.GetValue(artwork, null);

        if (property.PropertyType == typeof(string) && propertyValue != null)
        {
            if (property.Name.Contains("URL") && propertyValue.ToString()!.IsValidURL() == false)
                return Results.BadRequest($"An absolute {property.Name} is required in the following format: https://www.sample.url/picture.jpg");
        }

        if (property.PropertyType == typeof(int?) && propertyValue != null)
        {
            if (property.Name.Contains(nameof(artwork.YearCreated)) && ((int)propertyValue > DateTime.Today.Year))
                return Results.BadRequest($"{property.Name} can not be greater then {DateTime.Today.Year}.");

            if (property.Name.Contains(nameof(artwork.MediaId)) && (_mediaRepo.GetMediaTypeById((int)propertyValue) == null))
                return Results.NotFound($"No mediatype can be found with an {property.Name} of {propertyValue}");
        }

        // if (property.Name.Contains(nameof(artwork.Price)) && property.PropertyType is not double)
        //     return Results.BadRequest($"A decimal value {property.Name} is required.");
    }

    var result = await _artworkRepo.UpdateArtworkAsync(artworkId, artwork);
    return result is not null ? Results.NoContent() : Results.BadRequest("There was an issue updating this database entry.");
});

/*
 * Without Reflection
 * 
app.MapPut("api/artworks/{artworkId}", [Authorize] (IArtworkDataAccess _artworkRepo, IMediaDataAccess _mediaRepo, int artworkId, ArtworkInputDto artwork) =>
{
    if (_artworkRepo.GetArtworkById(artworkId) == null)
        return Results.NotFound($"No artwork can be found with an {nameof(artworkId)} of {artworkId}");

    if (artwork is not null)
    {
        if (!artwork.PrimaryImageUrl.IsValidURL() || !artwork.SecondaryImageUrl.IsValidURL())
            return Results.BadRequest($"An absolute primary or secondary image url is required in the following format: https://www.sample.url/picture.jpg");

        if (artwork.YearCreated > DateTime.Today.Year && artwork.YearCreated is not null)
            return Results.BadRequest($"The entered {nameof(artwork.YearCreated)} can not be greater than {DateTime.Today.Year}.");

        if (_mediaRepo.GetMediaTypeById((int)artwork.MediaId!) == null)
            return Results.NotFound($"No mediatype can be found with a {nameof(artwork.MediaId)} of {artwork.MediaId}");
    }
    else return Results.BadRequest($"Artwork details required.");

    var result = _artworkRepo.UpdateArtwork(artworkId, artwork);
    return result is not null ? Results.NoContent() : Results.BadRequest("There was an issue updating this database entry.");
});
*/

/// <summary>
/// Deletes an artwork with the specified id.
/// </summary>
/// 
/// <param name="_artworkRepo"> The repository providing data access methods for the artwork context. </param>
/// <param name="artworkId"> The id for the artwork we are trying to delete (from the request URL). </param>
/// 
/// <returns> No content. </returns>
/// <response code="204"> No content. </response>
/// <response code="404"> If no artwork with the specified id exits. </response>
/// <response code="400"> If there is an issues executing the query in the database. </response>
app.MapDelete("api/artworks/{artworkId}", [Authorize] async (IArtworkDataAccessAsync _artworkRepo, int artworkId) =>
{
    if (await _artworkRepo.GetArtworkByIdAsync(artworkId) == null)
        return Results.NotFound($"No artwork can be found with an {nameof(artworkId)} of {artworkId}.");

    var result = await _artworkRepo.DeleteArtworkAsync(artworkId);
    return result is true ? Results.NoContent() : Results.BadRequest("There was an issue deleting this database entry.");
});

/// <summary>
/// Adds a new artwork/artist pair to the artist/artwork bridging table.
/// </summary>
/// 
/// <param name="_artworkRepo"> The repository providing data access methods for the artwork context. </param>
/// <param name="_artistRepo"> The repository providing data access methods for the artist context. </param>
/// <param name="artworkId"> The id for the artwork we are trying to delete (from the request URL). </param>
/// <param name="artistId"> The id for the artist we are trying to delete (from the request URL). </param>
/// 
/// <returns> The newly added artwork/artist pair. </returns>
/// <response code="200"> Returns the newly added artwork/artist pair. </response>
/// <response code="404"> If no artist or artwork with the specified ids exist. </response>
/// <response code="400"> If there is an issues executing the query in the database. </response>
app.MapPost("api/artworks/{artworkId}/allocate/artist/{artistId}", [Authorize] async (IArtworkDataAccessAsync _artworkRepo, IArtistDataAccessAsync _artistRepo, int artworkId, int artistId) =>
{
    if (await _artworkRepo.GetArtworkByIdAsync(artworkId) == null)
        return Results.NotFound($"No artwork can be found with an {nameof(artworkId)} of {artworkId}.");
    else if (await _artistRepo.GetArtistByIdAsync(artistId) == null)
        return Results.NotFound($"No artist can be found with an {nameof(artistId)} of {artistId}.");

    var result = await _artworkRepo.AllocateArtistAsync(artistId, artworkId);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue deleting this database entry.");
});

/// <summary>
/// Deletes an artwork/artist pair with the specified ids from the artist/artwork bridging table.
/// </summary>
/// 
/// <param name="_artworkRepo"> The repository providing data access methods for the artwork context. </param>
/// <param name="_artistRepo"> The repository providing data access methods for the artist context. </param>
/// <param name="artworkId"> The id for the artwork we are trying to delete (from the request URL). </param>
/// <param name="artistId"> The id for the artist we are trying to delete (from the request URL). </param>
/// 
/// <returns> No content. </returns>
/// <response code="204"> No content. </response>
/// <response code="404"> If no artist or artwork with the specified ids exist. </response>
/// <response code="400"> If there is an issues executing the query in the database. </response>
app.MapDelete("api/artworks/{artworkId}/deallocate/artist/{artistId}", [Authorize] async (IArtworkDataAccessAsync _artworkRepo, IArtistDataAccessAsync _artistRepo, int artworkId, int artistId) =>
{
    if (await _artworkRepo.GetArtworkByIdAsync(artworkId) == null)
        return Results.NotFound($"No artwork can be found with an {nameof(artworkId)} of {artworkId}.");
    else if (await _artistRepo.GetArtistByIdAsync(artistId) == null)
        return Results.NotFound($"No artist can be found with an {nameof(artistId)} of {artistId}.");

    var result = await _artworkRepo.DeallocateArtistAsync(artistId, artworkId);
    return result is true ? Results.NoContent() : Results.BadRequest("There was an issue deleting this database entry.");
});
#endregion

#region Map Media Endpoints

/// <summary>
/// Gets all media types.
/// </summary>
/// <param name="_mediaRepo"> The repository providing data access methods for the media context. </param>
/// <returns> A list of all media types. </returns>
/// <response code="200"> Returns a list of all media types, or an empty
/// list if there are currently none stored. </response>
app.MapGet("api/media/", (IMediaDataAccess _mediaRepo) => _mediaRepo.GetMediaTypes());

/// <summary>
/// Gets a media type with the specified id.
/// </summary>
/// <param name="_mediaRepo"> The repository providing data access methods for the media context. </param>
/// <param name="mediaId"> The id for the media type to be returned (taken from the request URL). </param>
/// <returns> A media type with the specified id. </returns>
/// <response code="200"> Returns the media type with the specified id. </response>
/// <response code="404"> If no media type with the specified id exitst. </response>
app.MapGet("api/media/{mediaId}", (IMediaDataAccess _mediaRepo, int mediaId) =>
{
    if (_mediaRepo.GetMediaTypeById(mediaId) == null)
        return Results.NotFound($"No media type can be found with an {nameof(mediaId)} of {mediaId}.");

    var result = _mediaRepo.GetMediaTypeById(mediaId);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue accessing this database entry.");
});

/// <summary>
/// Adds a new media type.
/// </summary>
/// <param name="_mediaRepo"> The repository providing data access methods for the media context. </param>
/// <param name="media"> A new media type (from the HTTP request body). </param>
/// <returns> The newly added media type. </returns>
/// <remarks>
/// Sample request body:
///
///     POST /api/media/
///     {
///         "mediaType": "Example Media Type",
///         "description": "Example Description."
///     }
///
/// </remarks>
/// <response code="200"> Returns the newly added media type. </response>
/// <response code="400"> If the request body contains any null values. </response>
/// <response code="409"> If the media type already exists. </response>
app.MapPost("api/media/", [Authorize] (IMediaDataAccess _mediaRepo, MediaInputDto media) =>
{
    PropertyInfo[] properties = media.GetType().GetProperties();
    List<MediaOutputDto> mediaTypes = _mediaRepo.GetMediaTypes();

    foreach (PropertyInfo property in properties)
    {
        var propertyValue = property.GetValue(media, null);

        if (property.PropertyType == typeof(string))
        {
            if (propertyValue == null || propertyValue.Equals(""))
                return Results.BadRequest($"A {property.Name} is required.");

            if (property.Name.Contains(nameof(media.MediaType)) && (_mediaRepo.GetMediaTypes().Exists(x => x.MediaType == media.MediaType) == true))
                return Results.Conflict($"A {nameof(media.MediaType)} matching this type already exists.");
        }
    }

    var result = _mediaRepo.InsertMediaType(media);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue creating this database entry.");
});

/*
 * Without Reflection
 * 
app.MapPost("api/media/", [Authorize] (IMediaDataAccess _mediaRepo, MediaInputDto media) =>
{
    if (media is not null)
    {
        if (media.MediaType.IsNullOrEmpty()) return Results.BadRequest($"A {nameof(media.MediaType)} is required.");
        else if (media.Description.IsNullOrEmpty()) return Results.BadRequest($"A {nameof(media.Description)} is required.");

        if (_mediaRepo.GetMediaTypes().Exists(x => x.MediaType.ToLower() == media.MediaType.ToLower()))
            return Results.Conflict($"A {nameof(media.MediaType)} matching this type already exists.");
    }
    else return Results.BadRequest($"Media details required.");

    var result = _mediaRepo.InsertMediaType(media);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue creating this database entry.");
});
*/

/// <summary>
/// Updates a media type.
/// </summary>
/// <param name="_mediaRepo"> The repository providing data access methods for the media context. </param>
/// <param name="mediaId"> The id for the media type we are looking to update (from the request URL). </param>
/// <param name="media"> Updated details for the media type (from the HTTP request body). </param>
/// <returns> No content. </returns>
/// <remarks>
/// Sample request body:
///
///     PUT /api/media/{mediaId}
///     {
///         "mediaType": "Example Media Type",
///         "description": "Example Description."
///     }
///
/// </remarks>
/// <response code="204"> If the media type is udpated successfully. </response>
/// <response code="400"> If the request body contains any null values. </response>
/// <response code="404"> If the specified id is not associated with any media type. </response>
/// <response code="409"> If the media type already exists. </response>
app.MapPut("api/media/{mediaId}", [Authorize] (IMediaDataAccess _mediaRepo, int mediaId, MediaInputDto media) =>
{
    if (_mediaRepo.GetMediaTypeById(mediaId) is null)
        return Results.NotFound($"No media type can be found with an {nameof(mediaId)} of {mediaId}.");

    PropertyInfo[] properties = media.GetType().GetProperties();
    List<MediaOutputDto> mediaTypes = _mediaRepo.GetMediaTypes();

    foreach (PropertyInfo property in properties)
    {
        var propertyValue = property.GetValue(media, null);

        if (property.PropertyType == typeof(string))
        {
            if (property.Name.Contains(nameof(media.MediaType)) && (_mediaRepo.GetMediaTypes().Exists(x => x.MediaType == media.MediaType) == true))
                return Results.Conflict($"A {nameof(media.MediaType)} matching this type already exists.");
        }
    }
    var result = _mediaRepo.UpdateMediaType(mediaId, media);
    return result is not null ? Results.NoContent() : Results.BadRequest("There was an issue updating this database entry.");
});

/*
 * Without Reflection
 * 
app.MapPut("api/media/{mediaId}", [Authorize] (IMediaDataAccess _mediaRepo, int mediaId, MediaInputDto media) =>
{
    if (_mediaRepo.GetMediaTypeById(mediaId) == null)
        return Results.NotFound($"No media type can be found with a {nameof(mediaId)} of {mediaId}.");

    if (media is not null)
    {
        if (_mediaRepo.GetMediaTypes().Exists(x => x.MediaType.ToLower() == media.MediaType.ToLower()))
            return Results.Conflict($"A {nameof(media.MediaType)} matching this type already exists.");
    }
    else return Results.BadRequest($"Media details required.");

    var result = _mediaRepo.UpdateMediaType(mediaId, media);
    return result is not null ? Results.NoContent() : Results.BadRequest("There was an issue updating this database entry.");
});
*/

/// <summary>
/// Deletes a media type with the specified id.
/// </summary>
/// 
/// <param name="_mediaRepo"> The repository providing data access methods for the media context. </param>
/// <param name="mediaId"> The id for the media type we are trying to delete (from the request URL). </param>
/// 
/// <returns> No content. </returns>
/// <response code="204"> No content. </response>
/// <response code="404"> If no media type with the specified id exits. </response>
/// <response code="400"> If there is an issues executing the query in the database. </response>
app.MapDelete("api/media/{mediaId}", [Authorize] (IMediaDataAccess _mediaRepo, int mediaId) =>
{
    if (_mediaRepo.GetMediaTypeById(mediaId) == null)
        return Results.NotFound($"No media type can be found with a {nameof(mediaId)} of {mediaId}.");

    var result = _mediaRepo.DeleteMediaType(mediaId);
    return result is true ? Results.NoContent() : Results.BadRequest("There was an issue deleting this database entry.");
});

#endregion

#region Map Exhibition Endpoints

/// <summary>
/// Gets all exhibitions.
/// </summary>
/// <param name="_exhibitionRepo"> The repository providing data access methods for the exhibition context. </param>
/// <returns> A list of all exhibitions. </returns>
/// <response code="200"> Returns a list of all exhibitions, or an empty
/// list if there are currently none stored. </response>
app.MapGet("api/exhibitions/", [AllowAnonymous] (IExhibitionDataAccess _exhibitionRepo) => _exhibitionRepo.GetExhibitions());

/// <summary>
/// Gets an exhibition with the specified id.
/// </summary>
/// <param name="_exhibitionRepo"> The repository providing data access methods for the exhibition context. </param>
/// <param name="exhibitionId"> The id for the exhibition to be returned (taken from the request URL). </param>
/// <returns> An exhibition with the specified id. </returns>
/// <response code="200"> Returns the exhibition with the specified id. </response>
/// <response code="404"> If no exhibition with the specified id exitst. </response>
app.MapGet("api/exhibitions/{exhibitionId}", [Authorize] (IExhibitionDataAccess _exhibitionRepo, int exhibitionId) =>
{
    var result = _exhibitionRepo.GetExhibitionById(exhibitionId);
    return result is not null ? Results.Ok(result) : Results.NotFound($"No exhibition can be found with an {nameof(exhibitionId)} of {exhibitionId}");
});

/// <summary>
/// Gets an exhibition, and it's associated artworks, with the specified id.
/// </summary>
/// <param name="_exhibitionRepo"> The repository providing data access methods for the exhibition context. </param>
/// <param name="exhibitionId"> The id for the exhibition to be returned (taken from the request URL). </param>
/// <returns> An exhibition, and it's associated artworks, with the specified id. </returns>
/// <response code="200"> Returns the exhibition with the specified id. </response>
/// <response code="404"> If no exhibition with the specified id exitst. </response>
app.MapGet("api/exhibitions/{exhibitionId}/artworks", [AllowAnonymous] (IExhibitionDataAccess _exhibitionRepo, int exhibitionId) =>
{
    var result = _exhibitionRepo.GetExhibitionArtworksById(exhibitionId);
    return result is not null ? Results.Ok(result) : Results.NotFound($"No exhibition can be found with an {nameof(exhibitionId)} of {exhibitionId}");
});

/// <summary>
/// Adds a new exhibition.
/// </summary>
/// <param name="_exhibitionRepo"> The repository providing data access methods for the exhibition context. </param>
/// <param name="exhibition"> A new exhibition (from the HTTP request body). </param>
/// <returns> The newly added exhibition. </returns>
/// <remarks>
/// Sample request body:
///
///     POST /api/exhibitions/
///     {
///         "name": "Example Name",
///         "description": "Example Description",
///         "backgroundImageUrl": "https://www.sample.url/picture.jpg",
///         "startDate": "2022-02-10",
///         "endDate": "2022-08-10"
///     }
///
/// </remarks>
/// <response code="200"> Returns the newly added exhibition. </response>
/// <response code="400"> If the request body contains any null values, the BackgroundImageUrl is not in the correct format,
/// or the end date is greater than the start date. </response>
app.MapPost("api/exhibitions/", [Authorize(Policy = "AdminOnly")] (IExhibitionDataAccess _exhibitionRepo, ExhibitionInputDto exhibition) =>
{
    PropertyInfo[] properties = exhibition.GetType().GetProperties();
    foreach (PropertyInfo property in properties)
    {
        var propertyValue = property.GetValue(exhibition, null);

        if (property.PropertyType == typeof(string))
        {
            if (propertyValue == null || propertyValue.Equals(""))
                return Results.BadRequest($"A {property.Name} is required.");

            if (property.Name.Contains("Url") && propertyValue.ToString()!.IsValidURL() == false)
                return Results.BadRequest($"An absolute {property.Name} is required in the following format: https://www.sample.url/picture.jpg");
        }

        if (property.PropertyType == typeof(DateOnly))
        {
            if (propertyValue == null || propertyValue.Equals(""))
                return Results.BadRequest($"A {property.Name} is required.");

            if (property.Name.Contains(nameof(exhibition.StartDate)) && ((DateOnly)propertyValue > exhibition.EndDate))
                return Results.BadRequest($"{property.Name} can not be after {exhibition.EndDate} (The end date).");
        }
    }

    var result = _exhibitionRepo.InsertExhibition(exhibition);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue creating this database entry.");
});

/*
 * Without Reflection
 * 
app.MapPost("api/exhibitions/", [Authorize(Policy = "AdminOnly")] (IExhibitionDataAccess _exhibitionRepo, ExhibitionInputDto exhibition) =>
{
    if (exhibition is not null)
    {
        if (exhibition.Name.IsNullOrEmpty()) return Results.BadRequest($"An exhibition {nameof(exhibition.Name)} is required.");
        else if (exhibition.Description.IsNullOrEmpty()) return Results.BadRequest($"An exhibition {nameof(exhibition.Description)} is required.");
        else if (exhibition.BackgroundImageUrl.IsNullOrEmpty()) return Results.BadRequest($"An exhibition {nameof(exhibition.BackgroundImageUrl)} is required.");

        if (!exhibition.BackgroundImageUrl.IsValidURL())
            return Results.BadRequest($"An absolute {nameof(exhibition.BackgroundImageUrl)} is required " +
                $"in the following format: https://www.sample.url/picture.jpg");

        if (exhibition.StartDate != default && exhibition.EndDate != default)
        {
            if (exhibition.StartDate > exhibition.EndDate)
                return Results.BadRequest($"The {nameof(exhibition.StartDate)} can not be after {exhibition.EndDate} (The end date).");
        }
        else return Results.BadRequest($"A {nameof(exhibition.StartDate)} and {nameof(exhibition.EndDate)} is required.");
    }
    else return Results.BadRequest($"Exhibition details required.");

    var result = _exhibitionRepo.InsertExhibition(exhibition);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue creating this database entry.");
});
*/

/// <summary>
/// Updates an exhibition.
/// </summary>
/// <param name="_exhibitionRepo"> The repository providing data access methods for the exhibition context. </param>
/// <param name="exhibitionId"> The id for the exhibition we are looking to update (from the request URL). </param>
/// <param name="exhibition"> Updated details for the exhibition(from the HTTP request body). </param>
/// <returns> No content. </returns>
/// <remarks>
/// Sample request body:
///
///     PUT /api/exhibitions/{exhibitionId}
///     {
///         "name": "Example Name",
///         "description": "Example Description",
///         "backgroundImageUrl": "https://www.sample.url/picture.jpg",
///         "startDate": "2022-11-10",
///         "endDate": "2023-02-10"
///     }
///
/// </remarks>
/// <response code="204"> If the media type is udpated successfully. </response>
/// <response code="400"> If the request body contains any null values, the BackgroundImageUrl is not in the correct format,
/// or the end date is greater than the start date. </response>
/// <response code="404"> If the specified id is not associated with any exhibition. </response>
app.MapPut("api/exhibitions/{exhibitionId}", [Authorize(Policy = "AdminOnly")] (IExhibitionDataAccess _exhibitionRepo, int exhibitionId, ExhibitionInputDto exhibition) =>
{
    if (_exhibitionRepo.GetExhibitionById(exhibitionId) == null)
        return Results.NotFound($"No exhibition can be found with an {nameof(exhibitionId)} of {exhibitionId}.");

    PropertyInfo[] properties = exhibition.GetType().GetProperties();
    foreach (PropertyInfo property in properties)
    {
        var propertyValue = property.GetValue(exhibition, null);

        if (property.PropertyType == typeof(string) && propertyValue != null)
        {
            if (property.Name.Contains("Url") && propertyValue.ToString()!.IsValidURL() == false && propertyValue.ToString() != "")
                return Results.BadRequest($"An absolute {property.Name} is required in the following format: https://www.sample.url/picture.jpg");
        }

        if (property.PropertyType == typeof(DateOnly))
        {
            if (property.Name.Contains(nameof(exhibition.StartDate)) && (DateOnly)propertyValue! != default(DateOnly) && ((DateOnly)propertyValue > exhibition.EndDate))
                return Results.BadRequest($"{property.Name} can not be after then {exhibition.EndDate}.");
        }
    }

    var result = _exhibitionRepo.UpdateExhibition(exhibitionId, exhibition);
    return result is not null ? Results.NoContent() : Results.BadRequest("There was an issue updating this database entry.");
});

/*
 * Without Reflection
 * 
app.MapPut("api/exhibitions/{exhibitionId}", [Authorize(Policy = "AdminOnly")] (IExhibitionDataAccess _exhibitionRepo, int exhibitionId, ExhibitionInputDto exhibition) =>
{
    if (_exhibitionRepo.GetExhibitionById(exhibitionId) == null)
        return Results.NotFound($"No exhibition can be found with an {nameof(exhibitionId)} of {exhibitionId}.");

    if (exhibition is not null)
    {
        if (!exhibition.BackgroundImageUrl.IsValidURL())
            return Results.BadRequest($"An absolute {nameof(exhibition.BackgroundImageUrl)} is required " +
                $"in the following format: https://www.sample.url/picture.jpg");

        if (exhibition.StartDate != default && exhibition.EndDate != default)
        {
            if (exhibition.StartDate > exhibition.EndDate)
                return Results.BadRequest($"The {nameof(exhibition.StartDate)} can not be after {exhibition.EndDate} (The end date).");
        }
    }
    else return Results.BadRequest($"Exhibition details required.");

    var result = _exhibitionRepo.UpdateExhibition(exhibitionId, exhibition);
    return result is not null ? Results.NoContent() : Results.BadRequest("There was an issue updating this database entry.");

});
*/

/// <summary>
/// Deletes an exhibition with the specified id.
/// </summary>
/// 
/// <param name="_exhibitionRepo"> The repository providing data access methods for the exhibition context. </param>
/// <param name="exhibitionId"> The id for the exhibition we are trying to delete (from the request URL). </param>
/// 
/// <returns> No content. </returns>
/// <response code="204"> No content. </response>
/// <response code="404"> If no exhibition with the specified id exits. </response>
/// <response code="400"> If there is an issues executing the query in the database. </response>
app.MapDelete("api/exhibitions/{exhibitionId}", [Authorize(Policy = "AdminOnly")] (IExhibitionDataAccess _exhibitionRepo, int exhibitionId) =>
{
    if (_exhibitionRepo.GetExhibitionById(exhibitionId) == null)
        return Results.NotFound($"No exhibition can be found with an {nameof(exhibitionId)} of {exhibitionId}.");

    var result = _exhibitionRepo.DeleteExhibition(exhibitionId);
    return result is true ? Results.NoContent() : Results.BadRequest("There was an issue deleting this database entry.");
});

/// <summary>
/// Adds a new exhibition/artwork pair to the artwork/exhibition bridging table.
/// </summary>
/// 
/// <param name="_exhibitionRepo"> The repository providing data access methods for the exhibition context. </param>
/// <param name="_artworkRepo"> The repository providing data access methods for the artwork context. </param>
/// <param name="exhibitionId"> The id for the exhibition we are trying to delete (from the request URL). </param>
/// <param name="artworkId"> The id for the artwork we are trying to delete (from the request URL). </param>
/// 
/// <returns> The newly added exhibition/artwork pair. </returns>
/// <response code="200"> Returns the newly added exhibition/artwork pair. </response>
/// <response code="404"> If no exhibition or artwork with the specified ids exist. </response>
/// <response code="400"> If there is an issues executing the query in the database. </response>
app.MapPost("api/exhibitions/{exhibitionId}/allocate/artwork/{artworkId}", [Authorize(Policy = "AdminOnly")] async (IExhibitionDataAccess _exhibitionRepo, IArtworkDataAccessAsync _artworkRepo, int exhibitionId, int artworkId) =>
{
    if (_exhibitionRepo.GetExhibitionById(exhibitionId) == null)
        return Results.NotFound($"No exhibition can be found with an {nameof(exhibitionId)} of {exhibitionId}.");

    if (await _artworkRepo.GetArtworkByIdAsync(artworkId) == null)
        return Results.NotFound($"No artwork can be found with an {nameof(artworkId)} of {artworkId}");

    var result = _exhibitionRepo.AllocateArtwork(artworkId, exhibitionId);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue creating this database entry.");
});

/// <summary>
/// Deletes an exhibition/artwork pair with the specified ids from the artwork/exhibition bridging table.
/// </summary>
/// 
/// <param name="_exhibitionRepo"> The repository providing data access methods for the exhibition context. </param>
/// <param name="_artworkRepo"> The repository providing data access methods for the artwork context. </param>
/// <param name="exhibitionId"> The id for the exhibition we are trying to delete (from the request URL). </param>
/// <param name="artworkId"> The id for the artwork we are trying to delete (from the request URL). </param>
/// 
/// <returns> No content. </returns>
/// <response code="204"> No content. </response>
/// <response code="404"> If no exhibition or artwork with the specified ids exist. </response>
/// <response code="400"> If there is an issues executing the query in the database. </response>
app.MapDelete("api/exhibitions/{exhibitionId}/deallocate/artwork/{artworkId}", [Authorize(Policy = "AdminOnly")] async (IExhibitionDataAccess _exhibitionRepo, IArtworkDataAccessAsync _artworkRepo, int exhibitionId, int artworkId) =>
{
    if (_exhibitionRepo.GetExhibitionById(exhibitionId) == null)
        return Results.NotFound($"No exhibition can be found with an {nameof(exhibitionId)} of {exhibitionId}.");

    if (await _artworkRepo.GetArtworkByIdAsync(artworkId) == null)
        return Results.NotFound($"No artwork can be found with an {nameof(artworkId)} of {artworkId}");

    var result = _exhibitionRepo.DeallocateArtwork(exhibitionId, artworkId);
    return result is true ? Results.NoContent() : Results.BadRequest("There was an issue deleting this database entry.");
});

#endregion

#region Map User Endpoints

/// <summary>
/// Gets all users.
/// </summary>
/// <param name="_accountRepo"> The repository providing data access methods for the account context. </param>
/// <returns> A list of all users. </returns>
/// <response code="200"> Returns a list of all users, or an empty
/// list if there are currently none stored. </response>
app.MapGet("api/users/", [Authorize] (IUserDataAccess _accountRepo) => _accountRepo.GetUsers());

/// <summary>
/// Gets a user with the specified id.
/// </summary>
/// <param name="_accountRepo"> The repository providing data access methods for the account context. </param>
/// <param name=")"> The id for the user to be returned (taken from the request URL). </param>
/// <returns> A user with the specified id. </returns>
/// <response code="200"> Returns the user with the specified id. </response>
/// <response code="404"> If no user with the specified id exitst. </response>
app.MapGet("api/users/{accountId}", [Authorize(Policy = "UserOnly")] (IUserDataAccess _accountRepo, int accountId) =>
{
    var result = _accountRepo.GetUserById(accountId);
    return result is not null ? Results.Ok(result) : Results.BadRequest();
});

/// <summary>
/// Creates a new user.
/// </summary>
/// <param name="_accountRepo"> The repository providing data access methods for the account context. </param>
/// <param name="user"> A new user (from the HTTP request body). </param>
/// <returns> The newly created user. </returns>
/// <remarks>
/// Sample request body:
///
///     POST /api/users/signup/
///     {
///         "firstName": "John",
///         "lastName": "Smith",
///         "email": "example@gmail.com",
///         "password": "Password12345!",
///         "role": "User"
///     }
///
/// </remarks>
/// <response code="200"> Returns the newly created user. </response>
/// <response code="400"> If the request body contains any null values, or the email/passwprd are invalid. </response>
app.MapPost("api/users/signup/", [AllowAnonymous] (IUserDataAccess _accountRepo, UserInputDto user) =>
{
    PropertyInfo[] properties = user.GetType().GetProperties();
    foreach (PropertyInfo property in properties)
    {
        var propertyValue = property.GetValue(user, null);

        if (property.PropertyType == typeof(string))
        {
            if (propertyValue == null || propertyValue.Equals(""))
            {
                if (!property.Name.Contains(nameof(user.Role)))
                {
                    return Results.BadRequest($"A {property.Name} is required.");
                }
            }
            if (propertyValue != null)
            {
                if (property.Name.Contains("Email") && (_accountRepo.GetUsers().Exists(x => x.Email == user.Email) == true))
                    return Results.Conflict($"An account with this email already exists.");

                if (property.Name.Contains("Email") && propertyValue.ToString()!.IsValidEmail() == false)
                    return Results.BadRequest($"A valid email is required.");

                if (property.Name.Contains("Password") && propertyValue.ToString()!.IsValidPassword() == false)
                    return Results.BadRequest($"A valid password is required. Password must contain at least 14 characters. " +
                        $"The maximum password length is 24.");
            }
        }
    }

    var result = _accountRepo.InsertUser(user);
    return result is not null ? Results.Ok(result) : Results.BadRequest();
});

/*
 * Without Reflection
 * 
app.MapPost("api/users/signup/", [AllowAnonymous] (IUserDataAccess _accountRepo, UserInputDto user) =>
{
    if (user is not null)
    {
        if (user.FirstName.IsNullOrEmpty()) return Results.BadRequest($"A user {nameof(user.FirstName)} is required.");
        else if (user.LastName.IsNullOrEmpty()) return Results.BadRequest($"A user {nameof(user.LastName)} is required.");
        else if (user.Email.IsNullOrEmpty()) return Results.BadRequest($"A user {nameof(user.Email)} is required.");
        else if (user.Password.IsNullOrEmpty()) return Results.BadRequest($"A user {nameof(user.Password)} is required.");
        else if (user.Role.IsNullOrEmpty()) return Results.BadRequest($"A user {nameof(user.Role)} is required.");

        if (_accountRepo.GetUsers().Exists(x => x.Email == user.Email) == true)
            return Results.Conflict($"An account with this email already exists.");
        if (!user.Email.IsValidEmail())
            return Results.BadRequest($"A valid email is required.");
        if (!user.Password.IsValidPassword().Contains("validated"))
            return Results.BadRequest(user.Password.IsValidPassword());
    }
    else return Results.BadRequest($"User details required.");

    var result = _accountRepo.InsertUser(user);
    return result is not null ? Results.Ok(result) : Results.BadRequest();
});
*/

/// <summary>
/// Logs in an existing user.
/// </summary>
/// <param name="_accountRepo"> The repository providing data access methods for the account context. </param>
/// <param name="user"> An existing user (from the HTTP request body). </param>
/// <returns> The authentication token. </returns>
/// <remarks>
/// Sample request body:
///
///     POST /api/users/login/
///     {
///         "email": "example@gmail.com",
///         "password": "Password12345!",
///     }
///
/// </remarks>
/// <response code="200"> Returns the authentication token. </response>
/// <response code="400"> If the request body contains any null values, or the user cannot be authenticated. </response>
app.MapPost("api/users/login/", [AllowAnonymous] (IUserDataAccess _accountRepo, LoginDto login) =>
{
    PropertyInfo[] properties = login.GetType().GetProperties();

    foreach (PropertyInfo property in properties)
    {
        var propertyValue = property.GetValue(login, null);

        if (property.PropertyType == typeof(string))
        {
            if (propertyValue == null || propertyValue.Equals(""))
                return Results.BadRequest($"A {property.Name} is required.");
        }
    }

    try
    {
        var result = _accountRepo.AuthenticateUser(login);
        return result is not null ? Results.Ok(result) : Results.BadRequest();
    } catch (Npgsql.PostgresException)
    {
        return Results.BadRequest($"Password authentication failed.");
    }
});

/*
 * Without Reflection
 * 
app.MapPost("api/users/login/", [AllowAnonymous] (IUserDataAccess _accountRepo, LoginDto login) =>
{
    if (login is not null)
    {
        if (login.Email.IsNullOrEmpty()) return Results.BadRequest($"A user {nameof(login.Email)} is required.");
        else if (login.Password.IsNullOrEmpty()) return Results.BadRequest($"A user {nameof(login.Password)} is required.");
    }
    else return Results.BadRequest($"Login details required.");

    var result = _accountRepo.AuthenticateUser(login);
    return result is not null ? Results.Ok(result) : Results.BadRequest();
});
*/

/// <summary>
/// Updates a user.
/// </summary>
/// <param name="_accountRepo"> The repository providing data access methods for the account context. </param>
/// <param name="accountId"> The id for the account we are looking to update (from the request URL). </param>
/// <param name="user"> Updated details for the user (from the HTTP request body). </param>
/// <returns> No content. </returns>
/// <remarks>
/// Sample request body:
///
///     PUT /api/users/{accountId}
///     {
///         "firstName": "John",
///         "lastName": "Smith",
///         "email": "example@gmail.com",
///         "password": "Password24680%",
///         "role": "Admin"
///     }
///
/// </remarks>
/// <response code="204"> If the user is udpated successfully. </response>
/// <response code="404"> If the specified id is not associated with any user. </response>
/// <response code="400"> If the request body contains any null values, or a role other than
/// 'user' and 'admin' is assigned to the user.  </response>
app.MapPut("api/users/{accountId}", [Authorize(Policy = "UserOnly")] (IUserDataAccess _accountRepo, int accountId, UserInputDto user) =>
{
    if (_accountRepo.GetUserById(accountId) == null)
        return Results.NotFound($"No user can be found with an {nameof(accountId)} of {accountId}");

    PropertyInfo[] properties = user.GetType().GetProperties();
    foreach (PropertyInfo property in properties)
    {
        var propertyValue = property.GetValue(user, null);

        if (property.PropertyType == typeof(string) && propertyValue != null && !propertyValue.Equals(""))
        {
            if (property.Name.Contains(nameof(user.Role)) && (propertyValue.ToString()!.ToLower() != "user"
            && propertyValue.ToString()!.ToLower() != "admin"))
                return Results.BadRequest($"A {property.Name} is required to be either User or Admin");
        }
    }
    var result = _accountRepo.UpdateUser(accountId, user);
    return result is not null ? Results.NoContent() : Results.BadRequest("There was an issue updating this database entry.");
});

/*
 * Without Reflection
 * 
app.MapPut("api/users/{accountId}", [Authorize(Policy = "UserOnly")] (IUserDataAccess _accountRepo, int accountId, UserInputDto user) =>
{
    if (_accountRepo.GetUserById(accountId) == null)
        return Results.NotFound($"No user can be found with an {nameof(accountId)} of {accountId}");

    if (user is not null)
    {
        if (user.Role.ToString().ToLower() != "user" && user.Role.ToString().ToLower() != "admin")
            return Results.BadRequest($"A {nameof(user.Role)} is required to be either User or Admin");
    }
    else return Results.BadRequest($"User details required.");

    var result = _accountRepo.UpdateUser(accountId, user);
    return result is not null ? Results.NoContent() : Results.BadRequest("There was an issue updating this database entry.");
});
*/

/// <summary>
/// Deletes a user with the specified id.
/// </summary>
/// <param name="_accountRepo"> The repository providing data access methods for the account context. </param>
/// <param name=")"> The id for the user we are trying to delete (from the request URL). </param>
/// <returns> No content. </returns>
/// <response code="204"> No content. </response>
/// <response code="404"> If no user with the specified id exits. </response>
/// <response code="400"> If there is an issues executing the query in the database. </response>
app.MapDelete("api/users/{accountId}", [Authorize(Policy = "AdminOnly")](IUserDataAccess _accountRepo, int accountId) =>
{
    if (_accountRepo.GetUserById(accountId) == null)
        return Results.NotFound($"No account can be found with an {nameof(accountId)} of {accountId}.");

    var result = _accountRepo.DeleteUser(accountId);
    return result is true ? Results.NoContent() : Results.BadRequest();
});


#endregion

app.Run();

using Aboriginal_Art_Gallery_of_Australia.Middleware;
using Aboriginal_Art_Gallery_of_Australia.Models.Database_Models;
using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.ADO;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

/*
 Register Services to the container below.
 */
builder.Services.AddDateOnlyTimeOnlyStringConverters();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.UseDateOnlyTimeOnlyStringConverters();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Aboriginal Art Gallery API",
        Description = "New backend service that provides resources for the Aboriginal Art Gallery of Australia",
        Contact = new OpenApiContact
        {
            Name = "John Doe",
            Email = "jdoe@deakin.edu.au"
        }
    });
});

// Middleware Services
builder.Services.AddSingleton<ArtistOfTheDayMiddleware>();
builder.Services.AddSingleton<ArtworkOfTheDayMiddleware>();



/*
 Swap between implementations using dependency injection, simply uncomment them below;
 */

// Implementaion 1 - ADO
builder.Services.AddScoped<IArtistDataAccess, ArtistADO>();
builder.Services.AddScoped<IArtworkDataAccess, ArtworkADO>();
builder.Services.AddScoped<IExhibitionDataAccess, ExhibitionADO>();
builder.Services.AddScoped<IMediaDataAccess, MediaADO>();
builder.Services.AddScoped<IUserDataAccess, UserADO>();

// Implementation 2 - Repository Pattern


// Implementation 3 - Entity Framework

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/*
 Map Artist Endpoints
 */

app.MapGet("api/artists/", (IArtistDataAccess _artistRepo) => _artistRepo.GetArtists());

app.MapGet("api/artists/{artistId}", (IArtistDataAccess _artistRepo, int artistId) =>
{
    if (_artistRepo.GetArtistById(artistId) == null)
        return Results.NotFound($"No artist can be found with an {nameof(artistId)} of {artistId}");

    var result = _artistRepo.GetArtistById(artistId);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue accessing this database entry.");
});

app.MapGet("api/artists/of-the-day", ([FromServices] ArtistOfTheDayMiddleware _showcase, IArtistDataAccess _artistRepo) => _showcase.GetArtistOfTheDay(_artistRepo.GetArtists()));

app.MapPost("api/artists/", (IArtistDataAccess _artistRepo, ArtistInputDto artist) =>
{
    PropertyInfo[] properties = artist.GetType().GetProperties();
    foreach (PropertyInfo property in properties)
    {
        var propertyValue = property.GetValue(artist, null);

        if (property.PropertyType == typeof(string))
        {
            if (propertyValue == null || propertyValue.Equals(""))
                return Results.BadRequest($"A {property.Name} is required.");

            if (property.Name.Contains(nameof(artist.ProfileImageURL)) && propertyValue.ToString()!.IsValidURL() == false)
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

    var result = _artistRepo.InsertArtist(artist);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue creating this database entry.");
});

app.MapPut("api/artists/{artistId}", (IArtistDataAccess _artistRepo, int artistId, ArtistInputDto artist) =>
{
    if (_artistRepo.GetArtistById(artistId) == null)
        return Results.NotFound($"No artist can be found with an {nameof(artistId)} of {artistId}");

    PropertyInfo[] properties = artist.GetType().GetProperties();
    foreach (PropertyInfo property in properties)
    {
        var propertyValue = property.GetValue(artist, null);

        if (property.PropertyType == typeof(string))
        {
            if (propertyValue == null || propertyValue.Equals(""))
                return Results.BadRequest($"A {property.Name} is required.");

            if (property.Name.Contains(nameof(artist.ProfileImageURL)) && propertyValue.ToString()!.IsValidURL() == false)
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

    var result = _artistRepo.UpdateArtist(artistId, artist);
    return result is not null ? Results.NoContent() : Results.BadRequest("There was an issue updating this database entry.");
});

app.MapDelete("api/artists/{artistId}", (IArtistDataAccess _artistRepo, int artistId) =>
{
    if (_artistRepo.GetArtistById(artistId) == null)
        return Results.NotFound($"No artist can be found with an {nameof(artistId)} of {artistId}");

    var result = _artistRepo.DeleteArtist(artistId);
    return result is true ? Results.NoContent() : Results.BadRequest("There was an issue deleting this database entry.");
});

/*
 Map Artwork Endpoints
 */

app.MapGet("api/artworks/", (IArtworkDataAccess _artworkRepo) => _artworkRepo.GetArtworks());


app.MapGet("api/artworks/{artworkId}", (IArtworkDataAccess _artworkRepo, int artworkId) =>
{
    if (_artworkRepo.GetArtworkById(artworkId) == null)
        return Results.NotFound($"No artwork can be found with an {nameof(artworkId)} of {artworkId}");

    var result = _artworkRepo.GetArtworkById(artworkId);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue accessing this database entry.");
});

app.MapGet("api/artwork/of-the-day", ([FromServices] ArtworkOfTheDayMiddleware _showcase, IArtworkDataAccess _artworkRepo) => _showcase.GetArtworkOfTheDay(_artworkRepo.GetArtworks()));

app.MapPost("api/artworks/", (IArtworkDataAccess _artworkRepo, IMediaDataAccess _mediaRepo, ArtworkInputDto artwork) =>
{
    PropertyInfo[] properties = artwork.GetType().GetProperties();
    foreach (PropertyInfo property in properties)
    {
        var propertyValue = property.GetValue(artwork, null);

        if (property.PropertyType == typeof(string))
        {
            if (propertyValue == null || propertyValue.Equals(""))
                return Results.BadRequest($"A {property.Name} is required.");

            if (property.Name.Contains("URL") && propertyValue.ToString()!.IsValidURL() == false)
                return Results.BadRequest($"An absolute {property.Name} is required in the following format: https://www.sample.url/picture.jpg");
        }

        if (property.PropertyType == typeof(int?))
        {
            if (propertyValue == null || propertyValue.Equals(""))
                return Results.BadRequest($"A {property.Name} is required.");

            if (property.Name.Contains(nameof(artwork.YearCreated)) && ((int)propertyValue > DateTime.Today.Year))
                return Results.BadRequest($"{property.Name} can not be greater then {DateTime.Today.Year}.");

            if (property.Name.Contains(nameof(artwork.MediaId)) && (_mediaRepo.GetMediaTypeById((int)propertyValue) == null))
                return Results.BadRequest($"No mediatype can be found with an {property.Name} of {propertyValue}");
        }
    }
    var result = _artworkRepo.InsertArtwork(artwork);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue creating this database entry.");
});

app.MapPut("api/artworks/{artworkId}", (IArtworkDataAccess _artworkRepo, IMediaDataAccess _mediaRepo, int artworkId, ArtworkInputDto artwork) =>
{
    if (_artworkRepo.GetArtworkById(artworkId) == null)
        return Results.NotFound($"No artwork can be found with an {nameof(artworkId)} of {artworkId}");

    PropertyInfo[] properties = artwork.GetType().GetProperties();
    foreach (PropertyInfo property in properties)
    {
        var propertyValue = property.GetValue(artwork, null);

        if (property.PropertyType == typeof(string))
        {
            if (propertyValue == null || propertyValue.Equals(""))
                return Results.BadRequest($"A {property.Name} is required.");

            if (property.Name.Contains("URL") && propertyValue.ToString()!.IsValidURL() == false)
                return Results.BadRequest($"An absolute {property.Name} is required in the following format: https://www.sample.url/picture.jpg");
        }

        if (property.PropertyType == typeof(int?))
        {
            if (propertyValue == null || propertyValue.Equals(""))
                return Results.BadRequest($"A {property.Name} is required.");

            if (property.Name.Contains(nameof(artwork.YearCreated)) && ((int)propertyValue > DateTime.Today.Year))
                return Results.BadRequest($"{property.Name} can not be greater then {DateTime.Today.Year}.");

            if (property.Name.Contains(nameof(artwork.MediaId)) && (_mediaRepo.GetMediaTypeById((int)propertyValue) == null))
                return Results.NotFound($"No mediatype can be found with an {property.Name} of {propertyValue}");
        }
    }

    var result = _artworkRepo.UpdateArtwork(artworkId, artwork);
    return result is not null ? Results.NoContent() : Results.BadRequest("There was an issue updating this database entry.");
});

app.MapDelete("api/artworks/{artworkId}", (IArtworkDataAccess _artworkRepo, int artworkId) =>
{
    if (_artworkRepo.GetArtworkById(artworkId) == null)
        Results.NotFound($"No artwork can be found with an {nameof(artworkId)} of {artworkId}.");

    var result = _artworkRepo.DeleteArtwork(artworkId);
    return result is true ? Results.NoContent() : Results.BadRequest("There was an issue deleting this database entry.");
});

app.MapPost("api/artworks/{artworkId}/assign/artist/{artistId}", (IArtworkDataAccess _artworkRepo, IArtistDataAccess _artistRepo, int artworkId, int artistId) =>
{
    if (_artworkRepo.GetArtworkById(artworkId) == null)
        return Results.NotFound($"No artwork can be found with an {nameof(artworkId)} of {artworkId}.");
    else if (_artistRepo.GetArtistById(artistId) == null)
        return Results.NotFound($"No artist can be found with an {nameof(artistId)} of {artistId}.");

    var result = _artworkRepo.AssignArtist(artistId, artworkId);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue deleting this database entry.");
});

app.MapDelete("api/artworks/{artworkId}/deassign/artist/{artistId}", (IArtworkDataAccess _artworkRepo, IArtistDataAccess _artistRepo, int artworkId, int artistId) =>
{
    if (_artworkRepo.GetArtworkById(artworkId) == null)
        return Results.NotFound($"No artwork can be found with an {nameof(artworkId)} of {artworkId}.");
    else if (_artistRepo.GetArtistById(artistId) == null)
        return Results.NotFound($"No artist can be found with an {nameof(artistId)} of {artistId}.");

    var result = _artworkRepo.DeassignArtist(artistId, artworkId);
    return result is true ? Results.NoContent() : Results.BadRequest("There was an issue deleting this database entry.");
});

/*
 Map Media Endpoints
 */

app.MapGet("api/media/", (IMediaDataAccess _mediaRepo) => _mediaRepo.GetMediaTypes());

app.MapGet("api/media/{mediaId}", (IMediaDataAccess _mediaRepo, int mediaId) =>
{
    if (_mediaRepo.GetMediaTypeById(mediaId) == null)
        Results.NotFound($"No media type can be found with an {nameof(mediaId)} of {mediaId}.");

    var result = _mediaRepo.GetMediaTypeById(mediaId);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue accessing this database entry.");
});

app.MapPost("api/medias/", (IMediaDataAccess _mediaRepo, MediaInputDto media) =>
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

app.MapPut("api/medias/{mediaId}", (IMediaDataAccess _mediaRepo, int mediaId, MediaInputDto media) =>
{
    if (_mediaRepo.GetMediaTypeById(mediaId) == null)
        Results.NotFound($"No media type can be found with an {nameof(mediaId)} of {mediaId}.");

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
    var result = _mediaRepo.UpdateMediaType(mediaId, media);
    return result is not null ? Results.NoContent() : Results.BadRequest("There was an issue updating this database entry.");
});

app.MapDelete("api/medias/{mediaId}", (IMediaDataAccess _mediaRepo, int mediaId) =>
{
    if (_mediaRepo.GetMediaTypeById(mediaId) == null)
        Results.NotFound($"No media type can be found with an {nameof(mediaId)} of {mediaId}.");

    var result = _mediaRepo.DeleteMediaType(mediaId);
    return result is true ? Results.NoContent() : Results.BadRequest("There was an issue deleting this database entry.");
});



/*
 Map Exhibition Endpoints
 */

app.MapGet("api/exhibitions/", (IExhibitionDataAccess _exhibitionRepo) => _exhibitionRepo.GetExhibitions());

app.MapGet("api/exhibitions/{exhibitionId}", (IExhibitionDataAccess _exhibitionRepo, int exhibitionId) =>
{
    var result = _exhibitionRepo.GetExhibitionById(exhibitionId);
    return result is not null ? Results.Ok(result) : Results.NotFound($"No exhibition can be found with an {nameof(exhibitionId)} of {exhibitionId}");
});

app.MapGet("api/exhibitions/{exhibitionId}/artworks", (IExhibitionDataAccess _exhibitionRepo, int exhibitionId) =>
{
    var result = _exhibitionRepo.GetExhibitionArtworksById(exhibitionId);
    return result is not null ? Results.Ok(result) : Results.NotFound($"No exhibition can be found with an {nameof(exhibitionId)} of {exhibitionId}");
});

app.MapPost("api/exhibitions/", (IExhibitionDataAccess _exhibitionRepo, ExhibitionInputDto exhibition) =>
{

    PropertyInfo[] properties = exhibition.GetType().GetProperties();
    foreach (PropertyInfo property in properties)
    {
        var propertyValue = property.GetValue(exhibition, null);

        if (property.PropertyType == typeof(string))
        {
            if (propertyValue == null || propertyValue.Equals(""))
                return Results.BadRequest($"A {property.Name} is required.");

            if (property.Name.Contains("URL") && propertyValue.ToString()!.IsValidURL() == false)
                return Results.BadRequest($"An absolute {property.Name} is required in the following format: https://www.sample.url/picture.jpg");
        }

        if (property.PropertyType == typeof(DateOnly))
        {
            if (propertyValue == null || propertyValue.Equals(""))
                return Results.BadRequest($"A {property.Name} is required.");

            if (property.Name.Contains(nameof(exhibition.StartDate)) && ((DateOnly)propertyValue >= exhibition.EndDate))
                return Results.BadRequest($"{property.Name} can not be greater then {exhibition.EndDate} (The end date).");
        }
    }

    var result = _exhibitionRepo.InsertExhibition(exhibition);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue creating this database entry.");
});

app.MapPut("api/exhibitions/{id}", (IExhibitionDataAccess _exhibitionRepo, int exhibitionId, ExhibitionInputDto exhibition) =>
{
    if (_exhibitionRepo.GetExhibitionById(exhibitionId) == null)
        return Results.NotFound($"No exhibition can be found with an {nameof(exhibitionId)} of {exhibitionId}.");

    PropertyInfo[] properties = exhibition.GetType().GetProperties();
    foreach (PropertyInfo property in properties)
    {
        var propertyValue = property.GetValue(exhibition, null);

        if (property.PropertyType == typeof(string))
        {
            if (propertyValue == null || propertyValue.Equals(""))
                return Results.BadRequest($"A {property.Name} is required.");

            if (property.Name.Contains("URL") && propertyValue.ToString()!.IsValidURL() == false)
                return Results.BadRequest($"An absolute {property.Name} is required in the following format: https://www.sample.url/picture.jpg");
        }

        if (property.PropertyType == typeof(DateOnly))
        {
            if (propertyValue == null || propertyValue.Equals(""))
                return Results.BadRequest($"A {property.Name} is required.");

            if (property.Name.Contains(nameof(exhibition.StartDate)) && ((DateOnly)propertyValue >= exhibition.EndDate))
                return Results.BadRequest($"{property.Name} can not be greater then {exhibition.EndDate}.");
        }
    }

    var result = _exhibitionRepo.InsertExhibition(exhibition);
    return result is not null ? Results.NoContent() : Results.BadRequest("There was an issue updating this database entry.");
});

app.MapDelete("api/exhibitions/{exhibitionId}", (IExhibitionDataAccess _exhibitionRepo, int exhibitionId) =>
{
    if (_exhibitionRepo.GetExhibitionById(exhibitionId) == null)
        return Results.NotFound($"No exhibition can be found with an {nameof(exhibitionId)} of {exhibitionId}.");

    var result = _exhibitionRepo.DeleteExhibition(exhibitionId);
    return result is true ? Results.NoContent() : Results.BadRequest("There was an issue deleting this database entry.");
});

app.MapPost("api/exhibitions/{exhibitionId}/assign/artwork/{artworkId}", (IExhibitionDataAccess _exhibitionRepo, IArtworkDataAccess _artworkRepo, int exhibitionId, int artworkId) =>
{
    if (_exhibitionRepo.GetExhibitionById(exhibitionId) == null)
        return Results.NotFound($"No exhibition can be found with an {nameof(exhibitionId)} of {exhibitionId}.");

    if (_artworkRepo.GetArtworkById(artworkId) == null)
        return Results.NotFound($"No artwork can be found with an {nameof(artworkId)} of {artworkId}");

    var result = _exhibitionRepo.AssignArtwork(artworkId, exhibitionId);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue creating this database entry.");
});

app.MapDelete("api/exhibitions/{exhibitionId}/deassign/artwork/{artworkId}", (IExhibitionDataAccess _exhibitionRepo, IArtworkDataAccess _artworkRepo, int exhibitionId, int artworkId) =>
{
    if (_exhibitionRepo.GetExhibitionById(exhibitionId) == null)
        return Results.NotFound($"No exhibition can be found with an {nameof(exhibitionId)} of {exhibitionId}.");

    if (_artworkRepo.GetArtworkById(artworkId) == null)
        return Results.NotFound($"No artwork can be found with an {nameof(artworkId)} of {artworkId}");

    var result = _exhibitionRepo.DeassignArtwork(exhibitionId, artworkId);
    return result is true ? Results.NoContent() : Results.BadRequest("There was an issue deleting this database entry.");
});

app.Run();
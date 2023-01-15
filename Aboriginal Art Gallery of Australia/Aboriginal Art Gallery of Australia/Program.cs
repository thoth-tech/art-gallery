using Aboriginal_Art_Gallery_of_Australia.Middleware;
using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.ADO;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.RP;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
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
        ValidateLifetime = false,
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
    Description = "JSON Web Token based security",
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
    options.UseDateOnlyTimeOnlyStringConverters();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Aboriginal Art Gallery API",
        Version = "1.0.0",
        Description = "New backend service that provides resources for the Aboriginal Art Gallery of Australia",
        Contact = new OpenApiContact
        {
            Name = "John Doe",
            Email = "jdoe@deakin.edu.au"
        }
    });
    options.AddSecurityDefinition("bearer", securityScheme);
    options.AddSecurityRequirement(securityRequirement);
});
#endregion

// Middleware Services
builder.Services.AddSingleton<ArtistOfTheDayMiddleware>();
builder.Services.AddSingleton<ArtworkOfTheDayMiddleware>();

/*
 Swap between implementations using dependency injection, simply uncomment them below;
 */

// Implementation 1 - ADO
builder.Services.AddScoped<IArtistDataAccess, ArtistADO>();
builder.Services.AddScoped<IArtworkDataAccess, ArtworkADO>();
builder.Services.AddScoped<IExhibitionDataAccess, ExhibitionADO>();
builder.Services.AddScoped<IMediaDataAccess, MediaADO>();
builder.Services.AddScoped<IUserDataAccess, UserADO>();

// Implementation 2 - Repository Pattern
/*builder.Services.AddScoped<IArtistDataAccess, ArtistRepository>();
builder.Services.AddScoped<IArtworkDataAccess, ArtworkRepository>();
builder.Services.AddScoped<IExhibitionDataAccess, ExhibitionRepository>();
builder.Services.AddScoped<IMediaDataAccess, MediaRepository>();
builder.Services.AddScoped<IUserDataAccess, UserRepository>();*/


// builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

#region Map Artist Endpoints

app.MapGet("api/artists/", (IArtistDataAccess _artistRepo) => _artistRepo.GetArtists());

app.MapGet("api/artists/{artistId}", (IArtistDataAccess _artistRepo, int artistId) =>
{
    if (_artistRepo.GetArtistById(artistId) == null)
        return Results.NotFound($"No artist can be found with an {nameof(artistId)} of {artistId}");

    var result = _artistRepo.GetArtistById(artistId);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue accessing this database entry.");
});

app.MapGet("api/artists/of-the-day", ([FromServices] ArtistOfTheDayMiddleware _showcase, IArtistDataAccess _artistRepo) => _showcase.GetArtistOfTheDay(_artistRepo.GetArtists()));

app.MapPost("api/artists/", [Authorize] (IArtistDataAccess _artistRepo, ArtistInputDto artist) =>
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

    var result = _artistRepo.InsertArtist(artist);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue creating this database entry.");
});

app.MapPut("api/artists/{artistId}", [Authorize] (IArtistDataAccess _artistRepo, int artistId, ArtistInputDto artist) =>
{
    if (_artistRepo.GetArtistById(artistId) == null)
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

    var result = _artistRepo.UpdateArtist(artistId, artist);
    return result is not null ? Results.NoContent() : Results.BadRequest("There was an issue updating this database entry.");
});

app.MapDelete("api/artists/{artistId}", [Authorize] (IArtistDataAccess _artistRepo, int artistId) =>
{
    if (_artistRepo.GetArtistById(artistId) == null)
        return Results.NotFound($"No artist can be found with an {nameof(artistId)} of {artistId}");

    var result = _artistRepo.DeleteArtist(artistId);
    return result is true ? Results.NoContent() : Results.BadRequest("There was an issue deleting this database entry.");
});

#endregion

#region Map Artwork Endpoints

app.MapGet("api/artworks/", (IArtworkDataAccess _artworkRepo) => _artworkRepo.GetArtworks());

app.MapGet("api/artworks/{artworkId}", (IArtworkDataAccess _artworkRepo, int artworkId) =>
{
    if (_artworkRepo.GetArtworkById(artworkId) == null)
        return Results.NotFound($"No artwork can be found with an {nameof(artworkId)} of {artworkId}");

    var result = _artworkRepo.GetArtworkById(artworkId);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue accessing this database entry.");
});

app.MapGet("api/artworks/of-the-day", ([FromServices] ArtworkOfTheDayMiddleware _showcase, IArtworkDataAccess _artworkRepo) => _showcase.GetArtworkOfTheDay(_artworkRepo.GetArtworks()));

app.MapPost("api/artworks/", [Authorize] (IArtworkDataAccess _artworkRepo, IMediaDataAccess _mediaRepo, ArtworkInputDto artwork) =>
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

app.MapPut("api/artworks/{artworkId}", [Authorize] (IArtworkDataAccess _artworkRepo, IMediaDataAccess _mediaRepo, int artworkId, ArtworkInputDto artwork) =>
{
    if (_artworkRepo.GetArtworkById(artworkId) == null)
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
    }

    var result = _artworkRepo.UpdateArtwork(artworkId, artwork);
    return result is not null ? Results.NoContent() : Results.BadRequest("There was an issue updating this database entry.");
});

app.MapDelete("api/artworks/{artworkId}", [Authorize] (IArtworkDataAccess _artworkRepo, int artworkId) =>
{
    if (_artworkRepo.GetArtworkById(artworkId) == null)
        Results.NotFound($"No artwork can be found with an {nameof(artworkId)} of {artworkId}.");

    var result = _artworkRepo.DeleteArtwork(artworkId);
    return result is true ? Results.NoContent() : Results.BadRequest("There was an issue deleting this database entry.");
});

app.MapPost("api/artworks/{artworkId}/assign/artist/{artistId}", [Authorize] (IArtworkDataAccess _artworkRepo, IArtistDataAccess _artistRepo, int artworkId, int artistId) =>
{
    if (_artworkRepo.GetArtworkById(artworkId) == null)
        return Results.NotFound($"No artwork can be found with an {nameof(artworkId)} of {artworkId}.");
    else if (_artistRepo.GetArtistById(artistId) == null)
        return Results.NotFound($"No artist can be found with an {nameof(artistId)} of {artistId}.");

    var result = _artworkRepo.AssignArtist(artistId, artworkId);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue deleting this database entry.");
});

app.MapDelete("api/artworks/{artworkId}/deassign/artist/{artistId}", [Authorize] (IArtworkDataAccess _artworkRepo, IArtistDataAccess _artistRepo, int artworkId, int artistId) =>
{
    if (_artworkRepo.GetArtworkById(artworkId) == null)
        return Results.NotFound($"No artwork can be found with an {nameof(artworkId)} of {artworkId}.");
    else if (_artistRepo.GetArtistById(artistId) == null)
        return Results.NotFound($"No artist can be found with an {nameof(artistId)} of {artistId}.");

    var result = _artworkRepo.DeassignArtist(artistId, artworkId);
    return result is true ? Results.NoContent() : Results.BadRequest("There was an issue deleting this database entry.");
});
#endregion

#region Map Media Endpoints

app.MapGet("api/media/", (IMediaDataAccess _mediaRepo) => _mediaRepo.GetMediaTypes());

app.MapGet("api/media/{mediaId}", (IMediaDataAccess _mediaRepo, int mediaId) =>
{
    if (_mediaRepo.GetMediaTypeById(mediaId) == null)
        Results.NotFound($"No media type can be found with an {nameof(mediaId)} of {mediaId}.");

    var result = _mediaRepo.GetMediaTypeById(mediaId);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue accessing this database entry.");
});

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

app.MapPut("api/media/{mediaId}", [Authorize] (IMediaDataAccess _mediaRepo, int mediaId, MediaInputDto media) =>
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
            if (property.Name.Contains(nameof(media.MediaType)) && (_mediaRepo.GetMediaTypes().Exists(x => x.MediaType == media.MediaType) == true))
                return Results.Conflict($"A {nameof(media.MediaType)} matching this type already exists.");
        }
    }
    var result = _mediaRepo.UpdateMediaType(mediaId, media);
    return result is not null ? Results.NoContent() : Results.BadRequest("There was an issue updating this database entry.");
});

app.MapDelete("api/media/{mediaId}", [Authorize] (IMediaDataAccess _mediaRepo, int mediaId) =>
{
    if (_mediaRepo.GetMediaTypeById(mediaId) == null)
        Results.NotFound($"No media type can be found with an {nameof(mediaId)} of {mediaId}.");

    var result = _mediaRepo.DeleteMediaType(mediaId);
    return result is true ? Results.NoContent() : Results.BadRequest("There was an issue deleting this database entry.");
});

#endregion

#region Map Exhibition Endpoints

app.MapGet("api/exhibitions/", [AllowAnonymous] (IExhibitionDataAccess _exhibitionRepo) => _exhibitionRepo.GetExhibitions());

app.MapGet("api/exhibitions/{exhibitionId}", [Authorize] (IExhibitionDataAccess _exhibitionRepo, int exhibitionId) =>
{
    var result = _exhibitionRepo.GetExhibitionById(exhibitionId);
    return result is not null ? Results.Ok(result) : Results.NotFound($"No exhibition can be found with an {nameof(exhibitionId)} of {exhibitionId}");
});

app.MapGet("api/exhibitions/{exhibitionId}/artworks", [AllowAnonymous] (IExhibitionDataAccess _exhibitionRepo, int exhibitionId) =>
{
    var result = _exhibitionRepo.GetExhibitionArtworksById(exhibitionId);
    return result is not null ? Results.Ok(result) : Results.NotFound($"No exhibition can be found with an {nameof(exhibitionId)} of {exhibitionId}");
});

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

            if (property.Name.Contains("URL") && propertyValue.ToString()!.IsValidURL() == false)
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
            if (property.Name.Contains("URL") && propertyValue.ToString()!.IsValidURL() == false && propertyValue.ToString() != "")
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

app.MapDelete("api/exhibitions/{exhibitionId}", [Authorize(Policy = "AdminOnly")] (IExhibitionDataAccess _exhibitionRepo, int exhibitionId) =>
{
    if (_exhibitionRepo.GetExhibitionById(exhibitionId) == null)
        return Results.NotFound($"No exhibition can be found with an {nameof(exhibitionId)} of {exhibitionId}.");

    var result = _exhibitionRepo.DeleteExhibition(exhibitionId);
    return result is true ? Results.NoContent() : Results.BadRequest("There was an issue deleting this database entry.");
});

app.MapPost("api/exhibitions/{exhibitionId}/assign/artwork/{artworkId}", [Authorize(Policy = "AdminOnly")] (IExhibitionDataAccess _exhibitionRepo, IArtworkDataAccess _artworkRepo, int exhibitionId, int artworkId) =>
{
    if (_exhibitionRepo.GetExhibitionById(exhibitionId) == null)
        return Results.NotFound($"No exhibition can be found with an {nameof(exhibitionId)} of {exhibitionId}.");

    if (_artworkRepo.GetArtworkById(artworkId) == null)
        return Results.NotFound($"No artwork can be found with an {nameof(artworkId)} of {artworkId}");

    var result = _exhibitionRepo.AssignArtwork(artworkId, exhibitionId);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue creating this database entry.");
});

app.MapDelete("api/exhibitions/{exhibitionId}/deassign/artwork/{artworkId}", [Authorize(Policy = "AdminOnly")] (IExhibitionDataAccess _exhibitionRepo, IArtworkDataAccess _artworkRepo, int exhibitionId, int artworkId) =>
{
    if (_exhibitionRepo.GetExhibitionById(exhibitionId) == null)
        return Results.NotFound($"No exhibition can be found with an {nameof(exhibitionId)} of {exhibitionId}.");

    if (_artworkRepo.GetArtworkById(artworkId) == null)
        return Results.NotFound($"No artwork can be found with an {nameof(artworkId)} of {artworkId}");

    var result = _exhibitionRepo.DeassignArtwork(exhibitionId, artworkId);
    return result is true ? Results.NoContent() : Results.BadRequest("There was an issue deleting this database entry.");
});

#endregion

#region Map User Endpoints

app.MapGet("api/users/", [Authorize] (IUserDataAccess _accountRepo) => _accountRepo.GetUsers());

app.MapGet("api/users/{id}", [Authorize(Policy = "UserOnly")] (IUserDataAccess _accountRepo, int id) =>
{
    var result = _accountRepo.GetUserById(id);
    return result is not null ? Results.Ok(result) : Results.BadRequest();
});

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

            if (property.Name.Contains("email") && propertyValue.ToString()!.IsValidEmail() == false)
                return Results.BadRequest($"A valid email is required.");

            if (property.Name.Contains("password") && propertyValue.ToString()!.IsValidPassword() == false)
            return Results.BadRequest($"A valid password is required.");
        }
    }

    var result = _accountRepo.InsertUser(user);
    return result is not null ? Results.Ok(result) : Results.BadRequest();
});

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
    var result = _accountRepo.AuthenticateUser(login);
    return result is not null ? Results.Ok(result) : Results.BadRequest();
});

app.MapPut("api/users/{id}", [Authorize(Policy = "UserOnly")] (IUserDataAccess _accountRepo, int accountId, UserInputDto user) =>
{
    if (_accountRepo.GetUserById(accountId) == null)
        return Results.NotFound($"No user can be found with an {nameof(accountId)} of {accountId}");

    PropertyInfo[] properties = user.GetType().GetProperties();
    foreach (PropertyInfo property in properties)
    {
        var propertyValue = property.GetValue(user, null);

        if (property.PropertyType == typeof(string) && propertyValue != null && !propertyValue.Equals(""))
        {
            if (property.Name.Contains(nameof(user.Role)) && (propertyValue.ToString() != "User" && propertyValue.ToString() != "Admin"))
                return Results.BadRequest($"A {property.Name} is required to be either User or Admin");
        }
    }
    var result = _accountRepo.UpdateUser(accountId, user);
    return result is not null ? Results.NoContent() : Results.BadRequest("There was an issue updating this database entry.");
});

app.MapDelete("api/users/{id}", [Authorize(Policy = "AdminOnly")](IUserDataAccess _accountRepo, int id) =>
{
    var result = _accountRepo.DeleteUser(id);
    return result is true ? Results.NoContent() : Results.BadRequest();
});

#endregion

app.Run();
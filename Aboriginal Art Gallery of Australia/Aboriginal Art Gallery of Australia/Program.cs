using System.Text;
using Aboriginal_Art_Gallery_of_Australia.Models.Database_Models;
using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.ADO;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

/*
 Register Services to the container below.
 */

#region JWT
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

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
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
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Auth header using the bearer scheme. Enter 'Bearer' [space]
         and then your token in the text input below. Example: 'Bearer abcd1234'",
         Name = "Authorisation",
         In = ParameterLocation.Header,
         Type = SecuritySchemeType.ApiKey,
         Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

/*
 Swap between implementations using dependency injection, simply uncomment them below;
 */

// Implementation 1 - ADO
builder.Services.AddScoped<IArtistDataAccess, ArtistADO>();
builder.Services.AddScoped<IArtworkDataAccess, ArtworkADO>();
builder.Services.AddScoped<IExhibitionDataAccess, ExhibitionADO>();
builder.Services.AddScoped<INationDataAccess, NationADO>();
builder.Services.AddScoped<IUserDataAccess, UserADO>();

// Implementation 2 - Repository Pattern


// Implementation 3 - Entity Framework

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

/*
 Map Artist Endpoints
 */

app.MapGet("api/artists/", (IArtistDataAccess _artistRepo) => _artistRepo.GetArtists());

app.MapGet("api/artists/{artistId}", (IArtistDataAccess _artistRepo, int artistId) =>
{
    if (artistId <= 0)
        return Results.BadRequest($"Provide a valid {nameof(artistId)}.");
    var result = _artistRepo.GetArtistById(artistId);
    return result is not null ? Results.Ok(result) : Results.NotFound();
});

app.MapPost("api/artists/", (IArtistDataAccess _artistRepo, ArtistInputDto artist) =>
{
    //Option 1
    if (artist.DisplayName == null || artist.DisplayName == "") artist.DisplayName = $"{artist.FirstName} {artist.LastName}";
    PropertyInfo[] properties = artist.GetType().GetProperties();

    foreach (PropertyInfo property in properties)
    {
        var propertyValue = property.GetValue(artist, null);

        if (property.PropertyType == typeof(string))
        {
            if (propertyValue == null || propertyValue.Equals(""))
                return Results.BadRequest($"A {property.Name} is required.");

            if (property.Name.Contains("URL") && propertyValue.ToString()!.IsValidURL() == false)
                return Results.BadRequest($"An absolute {property.Name} is required.");
        }

        if (property.PropertyType == typeof(int?))
        {
            if (propertyValue != null && ((int)propertyValue > DateTime.Today.Year))
                return Results.BadRequest($"{property.Name} can not be greater then {DateTime.Today.Year}.");

            if (property.Name.Contains(nameof(artist.YearOfBirth)) && propertyValue == null)
                return Results.BadRequest($"A {property.Name} is required.");

            if (property.Name.Contains(nameof(artist.YearOfDeath)) && propertyValue != null && ((int)propertyValue <= artist.YearOfBirth))
                return Results.BadRequest($"A {property.Name} can not be before the year of birth.");
        }
    }

    var result = _artistRepo.InsertArtist(artist);
    return result is not null ? Results.Ok(result) : Results.BadRequest("There was an issue inserting the artist into the database.");
});

app.MapPut("api/artists/{artistId}", (IArtistDataAccess _repo, int artistId, ArtistInputDto artist) =>
{
    //Option 2
    if (artist == null)
        return Results.BadRequest($"Provide a valid {nameof(artist)}.");
    else if (artist.FirstName == null || artist.FirstName == "")
        return Results.BadRequest($"A {nameof(artist.FirstName)} is required.");
    else if (artist.LastName == null || artist.LastName == "")
        return Results.BadRequest($"A {nameof(artist.LastName)} is required.");
    else if (artist.DisplayName == null || artist.DisplayName == "")
        artist.DisplayName = $"{artist.FirstName} {artist.LastName}";
    else if (artist.ProfileImageURL.IsValidURL() == false)
        return Results.BadRequest($"An absolute {nameof(artist.ProfileImageURL)} is required.");
    else if (artist.PlaceOfBirth == null || artist.PlaceOfBirth == "")
        return Results.BadRequest($"A {nameof(artist.PlaceOfBirth)} is required.");
    else if (artist.YearOfBirth == null)
        return Results.BadRequest($"A {nameof(artist.YearOfBirth)} is required.");
    else if (artist.YearOfBirth > DateTime.Today.Year)
        return Results.BadRequest($"{nameof(artist.YearOfBirth)} can not be greater then {DateTime.Today.Year}.");
    else if (artist.YearOfDeath != null && artist.YearOfDeath > DateTime.Today.Year)
        return Results.BadRequest($"{nameof(artist.YearOfDeath)} can not be greater then {DateTime.Today.Year}.");
    var result = _repo.UpdateArtist(artistId, artist);

    return result is not null ? Results.NoContent() : Results.NotFound($"No artist can be found with an id of {artistId}");
});


app.MapDelete("api/artists/{artistId}", (IArtistDataAccess _repo, int artistId) =>
{
    if (artistId <= 0)
        return Results.BadRequest($"Provide a valid {nameof(artistId)}.");
    var result = _repo.DeleteArtist(artistId);
    return result is true ? Results.NoContent() : Results.NotFound($"No artist can be found with an id of {artistId}");
});

/*
 Map Artwork Endpoints
 */

app.MapGet("api/artworks/", (IArtworkDataAccess _repo) => _repo.GetArtworks());

app.MapGet("api/artworks/{id}", (IArtworkDataAccess _repo, int id) =>
{
    var result = _repo.GetArtworkById(id);
    return result is not null ? Results.Ok(result) : Results.BadRequest();
});

app.MapPost("api/artworks/", (IArtworkDataAccess _repo, ArtworkInputDto artwork) =>
{
    var result = _repo.InsertArtwork(artwork);
    return result is not null ? Results.Ok(result) : Results.BadRequest();
});

app.MapPut("api/artworks/{id}", (IArtworkDataAccess _repo, int id, ArtworkInputDto artwork) =>
{
    var result = _repo.UpdateArtwork(id, artwork);
    return result is not null ? Results.NoContent() : Results.BadRequest();
});

app.MapDelete("api/artworks/{id}", (IArtworkDataAccess _repo, int id) =>
{
    var result = _repo.DeleteArtwork(id);
    return result is true ? Results.NoContent() : Results.BadRequest();
});

app.MapPost("api/artworks/{artworkId}/assign/artist/{artistId}", (IArtworkDataAccess _repo, int artistId, int artworkId) =>
{
    var result = _repo.AssignArtist(artistId, artworkId);
    return result is not null ? Results.Ok(result) : Results.BadRequest();
});

app.MapDelete("api/artworks/{artworkId}/deassign/artist/{artistId}", (IArtworkDataAccess _repo, int artistId, int artworkId) =>
{
    var result = _repo.DeassignArtist(artistId, artworkId);
    return result is true ? Results.NoContent() : Results.BadRequest();
});

/*
 Map Nation Endpoints
 */

app.MapGet("api/nations/", (INationDataAccess _repo) => _repo.GetNations());

app.MapGet("api/nations/{id}", (INationDataAccess _repo, int id) =>
{
    var result = _repo.GetNationById(id);
    return result is not null ? Results.Ok(result) : Results.BadRequest();
});

app.MapPost("api/nations/", (INationDataAccess _repo, NationInputDto nation) =>
{
    var result = _repo.InsertNation(nation);
    return result is not null ? Results.Ok(result) : Results.BadRequest();
});

app.MapPut("api/nations/{id}", (INationDataAccess _repo, int id, NationInputDto nation) =>
{
    var result = _repo.UpdateNation(id, nation);
    return result is not null ? Results.NoContent() : Results.BadRequest();
});

app.MapDelete("api/nations/{id}", (INationDataAccess _repo, int id) =>
{
    var result = _repo.DeleteNation(id);
    return result is true ? Results.NoContent() : Results.BadRequest();
});

/*
 Map Exhibition Endpoints
 */

app.MapGet("api/exhibitions/", (IExhibitionDataAccess _repo) => _repo.GetExhibitions());

app.MapGet("api/exhibitions/{id}", (IExhibitionDataAccess _repo, int id) =>
{
    var result = _repo.GetExhibitionById(id);
    return result is not null ? Results.Ok(result) : Results.BadRequest();
});

app.MapGet("api/exhibitions/{id}/artworks", (IExhibitionDataAccess _repo, int id) =>
{
    var result = _repo.GetExhibitionArtworksById(id);
    return result is not null ? Results.Ok(result) : Results.BadRequest();
});

app.MapPost("api/exhibitions/", (IExhibitionDataAccess _repo, ExhibitionInputDto exhibition) =>
{
    var result = _repo.InsertExhibition(exhibition);
    return result is not null ? Results.Ok(result) : Results.BadRequest();
});

app.MapPut("api/exhibitions/{id}", (IExhibitionDataAccess _repo, int id, ExhibitionInputDto exhibition) =>
{
    var result = _repo.UpdateExhibition(id, exhibition);
    return result is not null ? Results.NoContent() : Results.BadRequest();
});

app.MapDelete("api/exhibitions/{id}", (IExhibitionDataAccess _repo, int id) =>
{
    var result = _repo.DeleteExhibition(id);
    return result is true ? Results.NoContent() : Results.BadRequest();
});

app.MapPost("api/exhibitions/{exhibitionId}/assign/artwork/{artworkId}", (IExhibitionDataAccess _repo, int exhibitionId, int artworkId) =>
{
    var result = _repo.AssignArtwork(artworkId, exhibitionId);
    return result is not null ? Results.Ok(result) : Results.BadRequest();
});

app.MapDelete("api/exhibitions/{exhibitionId}/deassign/artwork/{artworkId}", (IExhibitionDataAccess _repo, int exhibitionId, int artworkId) =>
{
    var result = _repo.DeassignArtwork(exhibitionId, artworkId);
    return result is true ? Results.NoContent() : Results.BadRequest();
});

/*
    Map User Endpoints
*/

app.MapGet("api/users/", (IUserDataAccess _repo) => _repo.GetUsers());

app.MapPost("api/users/register/", (IUserDataAccess _repo, UserInputDto user) =>
{
    var result = _repo.InsertUser(user);
    return result is not null ? Results.Ok(result) : Results.BadRequest();
});

app.MapPost("api/users/login/", (IUserDataAccess _repo, LoginDto login) =>
{
    var result = _repo.AuthenticateUser(login);
    return result is not null ? Results.Ok(result) : Results.BadRequest();
});

app.Run();
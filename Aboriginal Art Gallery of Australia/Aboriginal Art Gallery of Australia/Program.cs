using Aboriginal_Art_Gallery_of_Australia.Models.DTOs;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.ADO;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Implementations.RP;
using Aboriginal_Art_Gallery_of_Australia.Persistence.Interfaces;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

/*
 Register Services to the container below.
 */

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
});


/*
 Swap between implementations using dependency injection, simply uncomment them below;
 */

// Implementaion 1 - ADO
//builder.Services.AddScoped<IArtistDataAccess, ArtistADO>();
//builder.Services.AddScoped<IArtworkDataAccess, ArtworkADO>();
//builder.Services.AddScoped<IExhibitionDataAccess, ExhibitionADO>();
//builder.Services.AddScoped<INationDataAccess, NationADO>();
//builder.Services.AddScoped<IUserDataAccess, UserADO>();


// Implementation 2 - Repository Pattern
builder.Services.AddScoped<IArtistDataAccess, ArtistRepository>();
builder.Services.AddScoped<IArtworkDataAccess, ArtworkRepository>();
builder.Services.AddScoped<IExhibitionDataAccess, ExhibitionRepository>();
builder.Services.AddScoped<INationDataAccess, NationRepository>();
builder.Services.AddScoped<IUserDataAccess, UserRepository>();


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

app.MapGet("api/artists/", (IArtistDataAccess _repo) => _repo.GetArtists());

app.MapGet("api/artists/{id}", (IArtistDataAccess _repo, int id) =>
{
    var result = _repo.GetArtistById(id);
    return result is not null ? Results.Ok(result) : Results.BadRequest();
});

app.MapPost("api/artists/", (IArtistDataAccess _repo, ArtistInputDto artist) =>
{
    var result = _repo.InsertArtist(artist);
    return result is not null ? Results.Ok(result) : Results.BadRequest();
});

app.MapPut("api/artists/{id}", (IArtistDataAccess _repo, int id, ArtistInputDto artist) =>
{
    var result = _repo.UpdateArtist(id, artist);
    return result is not null ? Results.NoContent() : Results.BadRequest();
});

app.MapDelete("api/artists/{id}", (IArtistDataAccess _repo, int id) =>
{
    var result = _repo.DeleteArtist(id);
    return result is true ? Results.NoContent() : Results.BadRequest();
});

app.MapPost("api/artists/{artistId}/assign/artwork/{artworkId}", (IArtistDataAccess _repo, int artistId, int artworkId) =>
{
    var result = _repo.AssignArtwork(artistId, artworkId);
    return result is not null ? Results.Ok(result) : Results.BadRequest();
});

app.MapDelete("api/artists/{artistId}/deassign/artwork/{artworkId}", (IArtistDataAccess _repo, int artistId, int artworkId) =>
{
    var result = _repo.DeassignArtwork(artistId, artworkId);
    return result is true ? Results.NoContent() : Results.BadRequest();
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

app.MapPost("api/artworks/{artworkId}/assign/exhibition/{exhibitionId}", (IArtworkDataAccess _repo, int artworkId, int exhibitionId) =>
{
    var result = _repo.AssignExhibition(artworkId, exhibitionId);
    return result is not null ? Results.Ok(result) : Results.BadRequest();
});

app.MapDelete("api/artworks/{artworkId}/deassign/exhibition/{exhibitionId}", (IArtworkDataAccess _repo, int artworkId, int exhibitionId) =>
{
    var result = _repo.DeassignExhibition(artworkId, exhibitionId);
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

app.Run();
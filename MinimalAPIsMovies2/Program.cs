using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies2;
using MinimalAPIsMovies2.Entities;
using MinimalAPIsMovies2.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Service zone - BEGIN

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("DefaultConnection"));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(configuration =>
    {
        configuration.WithOrigins(builder.Configuration["allowedOrigins"]!).AllowAnyMethod().AllowAnyHeader();
    });
    options.AddPolicy("free", configuration =>
    {
        configuration.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddOutputCache();

builder.Services.AddOutputCache();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IGenresRepository, GenresRepository>();
//Service Zone - END


var app = builder.Build();


//Middlewares zone - BEGIN

//if (builder.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseOutputCache();

app.MapGet("/", () => "Hello World");

 var genresEndpoints = app.MapGroup("/genres");

genresEndpoints.MapGet("/", GetGenres)
.CacheOutput(c=> c.Expire(TimeSpan.FromSeconds(60)).Tag("genres-get"));

genresEndpoints.MapGet("/{id:int}", GetById);

genresEndpoints.MapPost("/", Create );


genresEndpoints.MapPut("/{id:int}", Update);

genresEndpoints.MapDelete("/{id:int}", Delete);

//Middlewares zone - END

app.Run();


static async Task<Ok<List<Genre>>> GetGenres(IGenresRepository repository)
{
    var genres = await repository.GetAll();
    return TypedResults.Ok(genres);
}

static async Task<Results<Ok<Genre>,NotFound>> GetById(int id, IGenresRepository repository) 
{
    var genre = await repository.GetById(id);

    if (genre is null)
    {
        return TypedResults.NotFound();
    }
    return TypedResults.Ok(genre);
}

static async Task<Created<Genre>> Create (Genre genre, IGenresRepository repository, IOutputCacheStore outputCacheStore) 
{
    var id = await repository.Create(genre);
    await outputCacheStore.EvictByTagAsync("genres-get", default);
    return TypedResults.Created($"/{id}", genre);
}

static async Task<Results<NotFound,NoContent>> Update (int id, Genre genre, IGenresRepository repository, IOutputCacheStore outputCacheStore) 
{
    var exists = await repository.Exists(id);

    if (!exists)
    {
        return TypedResults.NotFound();
    }
    await repository.Update(genre);
    await outputCacheStore.EvictByTagAsync("genre-get", default);
    return TypedResults.NoContent();
}

static async Task<Results<NotFound,NoContent>> Delete(int id, IGenresRepository repository, IOutputCacheStore outputCacheStore) 
{
    var exists = await repository.Exists(id);

    if (!exists)
    {
        return TypedResults.NotFound();
    }

    await repository.Delete(id);
    await outputCacheStore.EvictByTagAsync("genres-get", default);
    return TypedResults.NoContent();
}
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Cors;
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

genresEndpoints.MapGet("/", async (IGenresRepository repository)=>
{
    return await repository.GetAll();
    
        
}).CacheOutput(c=> c.Expire(TimeSpan.FromSeconds(60)).Tag("genres-get"));

genresEndpoints.MapGet("/{id:int}", async (int id, IGenresRepository repository) =>
{
    var genre = await repository.GetById(id);

    if (genre is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(genre);
}); 
genresEndpoints.MapPost("/", async (Genre genre, IGenresRepository repository,IOutputCacheStore outputCacheStore) =>
{
    var id = await repository.Create(genre);
    await outputCacheStore.EvictByTagAsync("genres-get", default);
    return Results.Created($"/{id}", genre);
});


genresEndpoints.MapPut("/{id:int}", async (int id, Genre genre, IGenresRepository repository, IOutputCacheStore outputCacheStore) =>
{
     var exists = await repository.Exists(id);

    if (!exists)
    {
        return Results.NotFound();
    }
    await repository.Update(genre);
    await outputCacheStore.EvictByTagAsync("genre-get", default);
    return Results.NoContent();
});

genresEndpoints.MapDelete("/{id:int}", async(int id, IGenresRepository repository, IOutputCacheStore outputCacheStore)=>
{
    var exists = await repository.Exists(id);

    if (!exists)
    {
        return Results.NotFound();
    }

    await repository.Delete(id);
    await outputCacheStore.EvictByTagAsync("genres-get", default);
    return Results.NoContent();
});

//Middlewares zone - END

app.Run();

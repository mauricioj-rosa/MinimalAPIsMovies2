using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies2;
using MinimalAPIsMovies2.Entities;
using MinimalAPIsMovies2.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Service zone - BEGIN

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("DefaultConnection"));

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



app.MapGet("/genres", async (IGenresRepository repository)=>
{
    return await repository.GetAll();
    
        
}).CacheOutput(c=> c.Expire(TimeSpan.FromSeconds(15)));

app.MapGet("/genres/{id:int}", async (int id, IGenresRepository repository) =>
{
    var genre = await repository.GetById(id);

    if (genre is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(genre);
}); 

app.MapPost("/genres", async (Genre genre, IGenresRepository repository) =>
{
    var id = await repository.Create(genre);
    return Results.Created($"/genres/{id}", genre);
});


//Middlewares zone - END

app.Run();

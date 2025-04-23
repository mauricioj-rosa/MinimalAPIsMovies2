using Microsoft.AspNetCore.Cors;
using MinimalAPIsMovies2.Entities;

var builder = WebApplication.CreateBuilder(args);

// Service zone - BEGIN

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

//Service Zone - END


var app = builder.Build();

builder.Services.AddOutputCache();

//Middlewares zone - BEGIN

app.UseCors();

app.UseOutputCache();

app.MapGet("/", () => "Hello World");



app.MapGet("/genres", ()=>
{
    var genres = new List<Genre>()
    {
        new Genre
        {
            Id = 1,
            Name = "Drama"
        },
        new Genre
        {
            Id = 2,
            Name= "Action"
        },
        new Genre
        {
            Id = 3,
            Name= "Comedy"
        }
    };

    return genres; 
}).CacheOutput(c=> c.Expire(TimeSpan.FromSeconds(15)));

//Middlewares zone - END

app.Run();

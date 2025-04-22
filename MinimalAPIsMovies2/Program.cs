using MinimalAPIsMovies2.Entities;

var builder = WebApplication.CreateBuilder(args);

// Service zone - BEGIN


var lastName = builder.Configuration.GetValue<string>("lastName");


//Service Zone - END


var app = builder.Build();


//Middlewares zone - BEGIN


app.MapGet("/", () => lastName);

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
});

//Middlewares zone - END

app.Run();

using GameStore.Api.Dtos;

const string GetGameEndpointName = "GetGame";
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GameDto> games = [
    new (1, "Sonic Shadows X", "Adventure", 59.99M, new DateOnly(2025, 7, 10)),
    new (2, "Bloodborne", "Soulslike", 20.00M, new DateOnly(2017, 06, 06)),
    new (3, "Monster Hunter Wilds", "RPG", 59.99M, new DateOnly(2025, 05, 22)),
];

//GET games
app.MapGet("/games", () => games);


app.MapGet("/games/{id}", (int id) => games.Find(game => game.Id == id))
    .WithName(GetGameEndpointName);

//POST games
app.MapPost("/games", (CreateGameDto newGame) =>
{
    GameDto game = new (
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );
    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpointName,new { id = game.Id }, game); 
});

// PUT /games/1
app.MapPut("/games/{id}", (int id, UpdateGameDto updateGameDto) => 
{
    var index = games.FindIndex(game => game.Id == id);
    games[index] = new GameDto(
        id,
        updateGameDto.Name,
        updateGameDto.Genre,
        updateGameDto.Price,
        updateGameDto.ReleaseDate
    ); 
    return Results.NoContent();
});

//Delete /games/id

app.MapDelete("/games/{id}", (int id) =>
{
    games.RemoveAll(game => game.Id == id);
    return Results.NoContent();
});

app.Run();

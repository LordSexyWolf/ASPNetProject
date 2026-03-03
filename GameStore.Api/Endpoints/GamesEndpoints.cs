namespace GameStore.Api.Endpointss;
using GameStore.Api.Dtos;

public static class GamesEndpoints 
{
    const string GetGameEndpointName = "GetGame";
    private static readonly List<GameDto> games = [
        new (1, "Sonic Shadows X", "Adventure", 59.99M, new DateOnly(2025, 7, 10)),
        new (2, "Bloodborne", "Soulslike", 20.00M, new DateOnly(2017, 06, 06)),
        new (3, "Monster Hunter Wilds", "RPG", 59.99M, new DateOnly(2025, 05, 22)),
    ];

    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");
        //GET games
        group.MapGet("/", () => games);


        group.MapGet("/{id}", (int id) =>
        {
            var game = games.Find(game => game.Id == id);
            return game is null ? Results.NotFound() : Results.Ok(game);
        })
            .WithName(GetGameEndpointName);

        //POST games
        group.MapPost("/", (CreateGameDto newGame) =>
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
        group.MapPut("/{id}", (int id, UpdateGameDto updateGameDto) => 
        {
            var index = games.FindIndex(game => game.Id == id);

            if(index == -1)
            {
                return Results.NotFound();
            }

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

        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);
            return Results.NoContent();
        });
    }
}
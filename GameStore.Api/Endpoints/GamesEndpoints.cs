namespace GameStore.Api.Endpointss;

using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

public static class GamesEndpoints 
{
    const string GetGameEndpointName = "GetGame";
    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");
        //GET games
        group.MapGet("/", async (GameStoreContext context) 
        => await context.Games
                        .Include(game => game.Genre)
                        .Select(game => new GameSummaryDto(
                                game.Id,
                                game.Name,
                                game.Genre!.Name,
                                game.Price,
                                game.ReleaseDate
                        ))
                        .AsNoTracking()
                        .ToListAsync());

        //GET games/id
        group.MapGet("/{id}", async (int id, GameStoreContext context) =>
        {
            var game = await context.Games.FindAsync(id);
            return game is null ? Results.NotFound() : Results.Ok(
                new GameDetailsDto(
                    game.Id,
                    game.Name,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                )
            );
        })
            .WithName(GetGameEndpointName);

        //POST games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext context) =>
        {
            Game game = new()
            {
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };
            context.Games.Add(game);
            await context.SaveChangesAsync();

            
            GameDetailsDto gameDto = new(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );

            return Results.CreatedAtRoute(GetGameEndpointName,new { id = gameDto.Id }, gameDto); 
        });

        // PUT /games/1
        group.MapPut("/{id}", async (int id, UpdateGameDto updateGameDto, GameStoreContext context) => 
        {
            var game = await context.Games.FindAsync(id);

            if(game is null)
            {
                return Results.NotFound();
            }

            game.Name = updateGameDto.Name;
            game.GenreId = updateGameDto.GenreId;
            game.Price = updateGameDto.Price;
            game.ReleaseDate = updateGameDto.ReleaseDate;
            
            await context.SaveChangesAsync();
            return Results.NoContent();
        });

        //Delete /games/id

        group.MapDelete("/{id}", async (int id, GameStoreContext context) =>
        {
            await context.Games
                        .Where(g => g.Id == id)
                        .ExecuteDeleteAsync();
            return Results.NoContent();
        });
    }
}
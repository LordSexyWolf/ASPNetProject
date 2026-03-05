using GameStore.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Dtos;

public static class GenreDtoExtensions
{
    public static void MapGenreEnpoints(this WebApplication app)
    {
        var group = app.MapGroup("/genres");

        //Get /genres
        group.MapGet("/", async (GameStoreContext context) =>
            await context.Genres
                        .Select(g => new GenreDto(g.Id, g.Name))
                        .AsNoTracking()
                        .ToListAsync());
    }
}
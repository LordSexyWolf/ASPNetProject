using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Endpointss;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();
builder.AddGameStoreDb();

var app = builder.Build();

app.MapGamesEndpoints();
app.MapGenreEnpoints();

app.MigrateDb();

app.Run();

using GameStore.Api.Dtos;
using GameStore.Api.Endpointss;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGamesEndpoints();

app.Run();

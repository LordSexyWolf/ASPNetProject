using GameStore.Api.Dtos;
using GameStore.Api.Endpointss;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

var app = builder.Build();

app.MapGamesEndpoints();

app.Run();

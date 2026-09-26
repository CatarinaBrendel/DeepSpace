using DeepSpace.Domain;
using DeepSpace.Domain.Power;
using DeepSpace.Engine;
using DeepSpace.Server.Components;
using DeepSpace.Server.Game;
using DeepSpace.Server.Simulation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSingleton(_ =>
{
    var spacecraft = new Spacecraft();

    spacecraft.AddComponent("generator_1", new Generator(outputWatts: 400));
    spacecraft.AddComponent("battery_1", new Battery(capacityWh: 5_000, chargeWh: 3_500));
    spacecraft.AddComponent("life_support", new Consumer(powerDrawWatts: 250));

    var simulation = new GameSimulation(spacecraft);

    return new GameSession(simulation);
});

builder.Services.AddHostedService<SimulationHost>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.MapGameEndpoints();
app.MapComponentEndpoints();

app.Run();
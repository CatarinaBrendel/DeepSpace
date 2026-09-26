using DeepSpace.Domain.Power;
using DeepSpace.Engine;

namespace DeepSpace.Server.Game;

public static class GameEndpoints
{
    public static IEndpointRouteBuilder MapGameEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/game");

        group.MapGet("/state", GetState);

        return endpoints;
    }

    private static GameStateResponse GetState(GameSession gameSession)
    {
        return gameSession.Read(simulation =>
        {
            var battery = simulation.Spacecraft
                .GetComponents<Battery>()
                .FirstOrDefault();

            var generators = simulation.Spacecraft
                .GetComponentsWithIds<Generator>()
                .Select(entry => new GeneratorStateResponse(
                    Id: entry.Key,
                    OutputWatts: entry.Value.OutputWatts,
                    IsRunning: entry.Value.IsRunning))
                .ToArray();

            return new GameStateResponse(
                ElapsedGameSeconds: simulation.ElapsedGameTime.TotalSeconds,
                Battery: battery is null
                    ? null
                    : new BatteryStateResponse(
                        CapacityWh: battery.CapacityWh,
                        ChargeWh: battery.ChargeWh,
                        StateOfCharge: battery.StateOfCharge),
                Generators: generators);
        });
    }
}
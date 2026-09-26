using DeepSpace.Server.Game;

namespace DeepSpace.Server.Components;

public static class ComponentEndpoints
{
    public static IEndpointRouteBuilder MapComponentEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/components");

        group.MapPost("/{id}/start", Start);
        group.MapPost("/{id}/stop", Stop);

        return endpoints;
    }

    private static IResult Start(string id, GameSession gameSession)
    {
        var started = gameSession.Update(simulation => simulation.StartGenerator(id));

        return started
            ? Results.NoContent()
            : Results.NotFound();
    }

    private static IResult Stop(string id, GameSession gameSession)
    {
        var stopped = gameSession.Update(simulation => simulation.StopGenerator(id));

        return stopped
            ? Results.NoContent()
            : Results.NotFound();
    }
}
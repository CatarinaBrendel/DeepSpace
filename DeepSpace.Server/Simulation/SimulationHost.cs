using System.Diagnostics;

using DeepSpace.Server.Game;

namespace DeepSpace.Server.Simulation;

public sealed class SimulationHost(GameSession gameSession) : BackgroundService
{
    private static readonly TimeSpan TickInterval = TimeSpan.FromSeconds(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var previousElapsed = stopwatch.Elapsed;

        using var timer = new PeriodicTimer(TickInterval);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            var currentElapsed = stopwatch.Elapsed;
            var elapsedRealTime = currentElapsed - previousElapsed;

            previousElapsed = currentElapsed;

            gameSession.Update(simulation => simulation.Advance(elapsedRealTime));
        }
    }
}
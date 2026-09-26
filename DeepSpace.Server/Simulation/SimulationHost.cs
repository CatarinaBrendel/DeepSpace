using System.Diagnostics;

using DeepSpace.Engine;

namespace DeepSpace.Server.Simulation;

public sealed class SimulationHost(GameSimulation simulation) : BackgroundService
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

            simulation.Advance(elapsedRealTime);
        }
    }
}
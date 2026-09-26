using DeepSpace.Domain.Power;

namespace DeepSpace.Engine.Power;

public sealed class PowerSimulation
{
    public PowerSimulationResult Step(
        Generator generator,
        Consumer consumer,
        Battery battery,
        TimeSpan elapsed)
    {
        if (elapsed < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(elapsed), "Elapsed time must not be negative.");

        var hours = elapsed.TotalHours;

        var generatedWh = generator.IsRunning ? generator.OutputWatts * hours : 0;
        var demandedWh = consumer.IsEnabled ? consumer.PowerDrawWatts * hours : 0;

        var balanceWh = generatedWh - demandedWh;

        var storedWh = 0.0;
        var suppliedWh = 0.0;
        var unmetDemandWh = 0.0;

        if (balanceWh > 0)
        {
            storedWh = battery.Store(balanceWh);
        }
        else if (balanceWh < 0)
        {
            var deficitWh = -balanceWh;

            suppliedWh = battery.Supply(deficitWh);
            unmetDemandWh = deficitWh - suppliedWh;
        }

        return new PowerSimulationResult(
            GeneratedWh: generatedWh,
            DemandedWh: demandedWh,
            BatteryStoredWh: storedWh,
            BatterySuppliedWh: suppliedWh,
            UnmetDemandWh: unmetDemandWh);
    }
}
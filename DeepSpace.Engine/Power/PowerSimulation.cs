using DeepSpace.Domain;
using DeepSpace.Domain.Power;

namespace DeepSpace.Engine.Power;

public sealed class PowerSimulation
{
    public PowerSimulationResult Step(Spacecraft spacecraft, TimeSpan elapsed)
    {
        ArgumentNullException.ThrowIfNull(spacecraft);

        if (elapsed < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(elapsed), "Elapsed time must not be negative.");

        var hours = elapsed.TotalHours;

        var generatedWh = spacecraft
            .GetComponents<Generator>()
            .Where(generator => generator.IsRunning)
            .Sum(generator => generator.OutputWatts * hours);

        var demandedWh = spacecraft
            .GetComponents<Consumer>()
            .Where(consumer => consumer.IsEnabled)
            .Sum(consumer => consumer.PowerDrawWatts * hours);

        var balanceWh = generatedWh - demandedWh;

        var storedWh = 0.0;
        var suppliedWh = 0.0;
        var unmetDemandWh = 0.0;

        var battery = spacecraft.GetComponents<Battery>().FirstOrDefault();

        if (balanceWh > 0 && battery is not null)
        {
            storedWh = battery.Store(balanceWh);
        }
        else if (balanceWh < 0)
        {
            var deficitWh = -balanceWh;

            if (battery is not null)
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
using DeepSpace.Domain.Power;
using DeepSpace.Engine.Power;

namespace DeepSpace.Engine.Tests.Power;

public sealed class PowerSimulationTests
{
    [Fact]
    public void Step_WithSurplusGeneration_ChargesBattery()
    {
        var generator = new Generator(outputWatts: 400, isRunning: true);
        var consumer = new Consumer(powerDrawWatts: 250);
        var battery = new Battery(capacityWh: 5_000, chargeWh: 3_500);
        var simulation = new PowerSimulation();

        var result = simulation.Step(generator, consumer, battery, TimeSpan.FromHours(1));

        Assert.Equal(400, result.GeneratedWh);
        Assert.Equal(250, result.DemandedWh);
        Assert.Equal(150, result.BatteryStoredWh);
        Assert.Equal(0, result.BatterySuppliedWh);
        Assert.Equal(0, result.UnmetDemandWh);
        Assert.Equal(3_650, battery.ChargeWh);
    }

    [Fact]
    public void Step_WithInsufficientGeneration_DrawsFromBattery()
    {
        var generator = new Generator(outputWatts: 100, isRunning: true);
        var consumer = new Consumer(powerDrawWatts: 250);
        var battery = new Battery(capacityWh: 5_000, chargeWh: 3_500);
        var simulation = new PowerSimulation();

        var result = simulation.Step(generator, consumer, battery, TimeSpan.FromHours(1));

        Assert.Equal(100, result.GeneratedWh);
        Assert.Equal(250, result.DemandedWh);
        Assert.Equal(0, result.BatteryStoredWh);
        Assert.Equal(150, result.BatterySuppliedWh);
        Assert.Equal(0, result.UnmetDemandWh);
        Assert.Equal(3_350, battery.ChargeWh);
    }

    [Fact]
    public void Step_WithEmptyBattery_ReportsUnmetDemand()
    {
        var generator = new Generator(outputWatts: 100, isRunning: true);
        var consumer = new Consumer(powerDrawWatts: 250);
        var battery = new Battery(capacityWh: 5_000, chargeWh: 0);
        var simulation = new PowerSimulation();

        var result = simulation.Step(generator, consumer, battery, TimeSpan.FromHours(1));

        Assert.Equal(0, result.BatterySuppliedWh);
        Assert.Equal(150, result.UnmetDemandWh);
        Assert.Equal(0, battery.ChargeWh);
    }

    [Fact]
    public void Step_WithFullBattery_DoesNotExceedCapacity()
    {
        var generator = new Generator(outputWatts: 400, isRunning: true);
        var consumer = new Consumer(powerDrawWatts: 250);
        var battery = new Battery(capacityWh: 5_000, chargeWh: 5_000);
        var simulation = new PowerSimulation();

        var result = simulation.Step(generator, consumer, battery, TimeSpan.FromHours(1));

        Assert.Equal(0, result.BatteryStoredWh);
        Assert.Equal(5_000, battery.ChargeWh);
    }
}
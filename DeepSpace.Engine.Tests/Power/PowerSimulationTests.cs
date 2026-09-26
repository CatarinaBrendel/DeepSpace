using DeepSpace.Domain;
using DeepSpace.Domain.Power;
using DeepSpace.Engine.Power;

namespace DeepSpace.Engine.Tests.Power;

public sealed class PowerSimulationTests
{
    [Fact]
    public void Step_WithSurplusGeneration_ChargesBattery()
    {
        var spacecraft = new Spacecraft();
        var battery = new Battery(capacityWh: 5_000, chargeWh: 3_500);

        spacecraft.AddComponent("generator_1", new Generator(outputWatts: 400, isRunning: true));
        spacecraft.AddComponent("life_support", new Consumer(powerDrawWatts: 250));
        spacecraft.AddComponent("battery_1", battery);

        var simulation = new PowerSimulation();

        var result = simulation.Step(spacecraft, TimeSpan.FromHours(1));

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
        var spacecraft = new Spacecraft();
        var battery = new Battery(capacityWh: 5_000, chargeWh: 3_500);

        spacecraft.AddComponent("generator_1", new Generator(outputWatts: 100, isRunning: true));
        spacecraft.AddComponent("life_support", new Consumer(powerDrawWatts: 250));
        spacecraft.AddComponent("battery_1", battery);

        var simulation = new PowerSimulation();

        var result = simulation.Step(spacecraft, TimeSpan.FromHours(1));

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
        var spacecraft = new Spacecraft();
        var battery = new Battery(capacityWh: 5_000, chargeWh: 0);

        spacecraft.AddComponent("generator_1", new Generator(outputWatts: 100, isRunning: true));
        spacecraft.AddComponent("life_support", new Consumer(powerDrawWatts: 250));
        spacecraft.AddComponent("battery_1", battery);

        var simulation = new PowerSimulation();

        var result = simulation.Step(spacecraft, TimeSpan.FromHours(1));

        Assert.Equal(0, result.BatterySuppliedWh);
        Assert.Equal(150, result.UnmetDemandWh);
        Assert.Equal(0, battery.ChargeWh);
    }

    [Fact]
    public void Step_WithFullBattery_DoesNotExceedCapacity()
    {
        var spacecraft = new Spacecraft();
        var battery = new Battery(capacityWh: 5_000, chargeWh: 5_000);

        spacecraft.AddComponent("generator_1", new Generator(outputWatts: 400, isRunning: true));
        spacecraft.AddComponent("life_support", new Consumer(powerDrawWatts: 250));
        spacecraft.AddComponent("battery_1", battery);

        var simulation = new PowerSimulation();

        var result = simulation.Step(spacecraft, TimeSpan.FromHours(1));

        Assert.Equal(0, result.BatteryStoredWh);
        Assert.Equal(5_000, battery.ChargeWh);
    }

    [Fact]
    public void Step_WithMultipleGeneratorsAndConsumers_UsesAllActiveComponents()
    {
        var spacecraft = new Spacecraft();
        var battery = new Battery(capacityWh: 5_000, chargeWh: 3_000);

        spacecraft.AddComponent("generator_1", new Generator(outputWatts: 300, isRunning: true));
        spacecraft.AddComponent("generator_2", new Generator(outputWatts: 200, isRunning: true));
        spacecraft.AddComponent("life_support", new Consumer(powerDrawWatts: 250));
        spacecraft.AddComponent("communications", new Consumer(powerDrawWatts: 100));
        spacecraft.AddComponent("battery_1", battery);

        var simulation = new PowerSimulation();

        var result = simulation.Step(spacecraft, TimeSpan.FromHours(1));

        Assert.Equal(500, result.GeneratedWh);
        Assert.Equal(350, result.DemandedWh);
        Assert.Equal(150, result.BatteryStoredWh);
        Assert.Equal(3_150, battery.ChargeWh);
    }
}
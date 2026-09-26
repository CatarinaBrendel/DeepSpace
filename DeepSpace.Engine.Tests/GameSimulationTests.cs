using DeepSpace.Domain;
using DeepSpace.Domain.Power;
using DeepSpace.Engine;

namespace DeepSpace.Engine.Tests;

public sealed class GameSimulationTests
{
    [Fact]
    public void Advance_AdvancesGameTime()
    {
        var simulation = new GameSimulation(new Spacecraft());

        simulation.Advance(TimeSpan.FromSeconds(1));

        Assert.Equal(TimeSpan.FromSeconds(24), simulation.ElapsedGameTime);
    }

    [Fact]
    public void Advance_UsesGameTimeForPowerSimulation()
    {
        var spacecraft = new Spacecraft();
        var battery = new Battery(capacityWh: 5_000, chargeWh: 3_000);

        spacecraft.AddComponent("generator_1", new Generator(outputWatts: 600, isRunning: true));
        spacecraft.AddComponent("life_support", new Consumer(powerDrawWatts: 300));
        spacecraft.AddComponent("battery_1", battery);

        var simulation = new GameSimulation(spacecraft);

        var result = simulation.Advance(TimeSpan.FromMinutes(2.5));

        Assert.Equal(TimeSpan.FromHours(1), simulation.ElapsedGameTime);

        Assert.Equal(600, result.GeneratedWh);
        Assert.Equal(300, result.DemandedWh);
        Assert.Equal(300, result.BatteryStoredWh);

        Assert.Equal(3_300, battery.ChargeWh);
    }

    [Fact]
    public void Advance_AccumulatesSimulationState()
    {
        var spacecraft = new Spacecraft();
        var battery = new Battery(capacityWh: 5_000, chargeWh: 3_000);

        spacecraft.AddComponent("generator_1", new Generator(outputWatts: 600, isRunning: true));
        spacecraft.AddComponent("life_support", new Consumer(powerDrawWatts: 300));
        spacecraft.AddComponent("battery_1", battery);

        var simulation = new GameSimulation(spacecraft);

        simulation.Advance(TimeSpan.FromMinutes(1));
        simulation.Advance(TimeSpan.FromMinutes(1));

        Assert.Equal(TimeSpan.FromMinutes(48), simulation.ElapsedGameTime);
        Assert.Equal(3_240, battery.ChargeWh);
    }
}
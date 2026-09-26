using DeepSpace.Domain;
using DeepSpace.Domain.Power;

namespace DeepSpace.Engine.Tests;

public sealed class GeneratorCommandTests
{
    [Fact]
    public void StartGenerator_StartsGenerator()
    {
        var spacecraft = new Spacecraft();
        var generator = new Generator(outputWatts: 400);

        spacecraft.AddComponent("generator_1", generator);

        var simulation = new GameSimulation(spacecraft);

        var result = simulation.StartGenerator("generator_1");

        Assert.True(result);
        Assert.True(generator.IsRunning);
    }

    [Fact]
    public void StopGenerator_StopsGenerator()
    {
        var spacecraft = new Spacecraft();
        var generator = new Generator(outputWatts: 400, isRunning: true);

        spacecraft.AddComponent("generator_1", generator);

        var simulation = new GameSimulation(spacecraft);

        var result = simulation.StopGenerator("generator_1");

        Assert.True(result);
        Assert.False(generator.IsRunning);
    }

    [Fact]
    public void StartGenerator_UnknownComponent_ReturnsFalse()
    {
        var simulation = new GameSimulation(new Spacecraft());

        var result = simulation.StartGenerator("unknown");

        Assert.False(result);
    }

    [Fact]
    public void StartGenerator_ComponentIsNotGenerator_ReturnsFalse()
    {
        var spacecraft = new Spacecraft();

        spacecraft.AddComponent("life_support", new Consumer(powerDrawWatts: 250));

        var simulation = new GameSimulation(spacecraft);

        var result = simulation.StartGenerator("life_support");

        Assert.False(result);
    }
}
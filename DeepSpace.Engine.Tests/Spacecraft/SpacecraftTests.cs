using DeepSpace.Domain;
using DeepSpace.Domain.Power;

namespace DeepSpace.Engine.Tests;

public sealed class SpacecraftTests
{
    [Fact]
    public void AddComponent_AddsComponent()
    {
        var spacecraft = new Spacecraft();
        var generator = new Generator(outputWatts: 400);

        spacecraft.AddComponent("generator_1", generator);

        Assert.Same(generator, spacecraft.GetComponent<Generator>("generator_1"));
    }

    [Fact]
    public void GetComponent_WithUnknownId_ReturnsNull()
    {
        var spacecraft = new Spacecraft();

        var component = spacecraft.GetComponent<Generator>("unknown");

        Assert.Null(component);
    }

    [Fact]
    public void GetComponent_WithWrongType_ReturnsNull()
    {
        var spacecraft = new Spacecraft();
        var generator = new Generator(outputWatts: 400);

        spacecraft.AddComponent("generator_1", generator);

        Assert.Null(spacecraft.GetComponent<Battery>("generator_1"));
    }

    [Fact]
    public void AddComponent_WithDuplicateId_Throws()
    {
        var spacecraft = new Spacecraft();

        spacecraft.AddComponent("generator_1", new Generator(outputWatts: 400));

        Assert.Throws<InvalidOperationException>(
            () => spacecraft.AddComponent("generator_1", new Generator(outputWatts: 500)));
    }

    [Fact]
    public void GetComponents_ReturnsComponentsOfRequestedType()
    {
        var spacecraft = new Spacecraft();

        var generator1 = new Generator(outputWatts: 400);
        var generator2 = new Generator(outputWatts: 250);

        spacecraft.AddComponent("generator_1", generator1);
        spacecraft.AddComponent("generator_2", generator2);
        spacecraft.AddComponent("battery_1", new Battery(capacityWh: 5_000, chargeWh: 3_500));

        var generators = spacecraft.GetComponents<Generator>().ToArray();

        Assert.Equal(2, generators.Length);
        Assert.Contains(generator1, generators);
        Assert.Contains(generator2, generators);
    }

    [Fact]
    public void GetComponentsWithIds_ReturnsMatchingComponentsAndIds()
    {
        var spacecraft = new Spacecraft();

        var generator1 = new Generator(outputWatts: 400);
        var generator2 = new Generator(outputWatts: 600);

        spacecraft.AddComponent("generator_1", generator1);
        spacecraft.AddComponent("generator_2", generator2);
        spacecraft.AddComponent("battery_1", new Battery(capacityWh: 5_000, chargeWh: 3_500));

        var generators = spacecraft
            .GetComponentsWithIds<Generator>()
            .ToArray();

        Assert.Equal(2, generators.Length);
        Assert.Equal("generator_1", generators[0].Key);
        Assert.Same(generator1, generators[0].Value);
        Assert.Equal("generator_2", generators[1].Key);
        Assert.Same(generator2, generators[1].Value);
    }
}
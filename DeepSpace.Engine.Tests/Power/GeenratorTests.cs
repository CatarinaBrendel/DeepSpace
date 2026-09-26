using DeepSpace.Domain.Power;

namespace DeepSpace.Engine.Tests.Power;

public sealed class GeneratorTests
{
    [Fact]
    public void Constructor_WithValidOutput_CreatesStoppedGenerator()
    {
        var generator = new Generator(outputWatts: 400);

        Assert.Equal(400, generator.OutputWatts);
        Assert.False(generator.IsRunning);
    }

    [Fact]
    public void Start_StartsGenerator()
    {
        var generator = new Generator(outputWatts: 400);

        generator.Start();

        Assert.True(generator.IsRunning);
    }

    [Fact]
    public void Stop_StopsGenerator()
    {
        var generator = new Generator(outputWatts: 400, isRunning: true);

        generator.Stop();

        Assert.False(generator.IsRunning);
    }

    [Fact]
    public void Constructor_WithZeroOutput_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Generator(outputWatts: 0));
    }
}
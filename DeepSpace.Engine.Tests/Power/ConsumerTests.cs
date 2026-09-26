using DeepSpace.Domain.Power;

namespace DeepSpace.Engine.Tests.Power;

public sealed class ConsumerTests
{
    [Fact]
    public void Constructor_WithValidPowerDraw_CreatesEnabledConsumer()
    {
        var consumer = new Consumer(powerDrawWatts: 250);

        Assert.Equal(250, consumer.PowerDrawWatts);
        Assert.True(consumer.IsEnabled);
    }

    [Fact]
    public void Disable_DisablesConsumer()
    {
        var consumer = new Consumer(powerDrawWatts: 250);

        consumer.Disable();

        Assert.False(consumer.IsEnabled);
    }

    [Fact]
    public void Enable_EnablesConsumer()
    {
        var consumer = new Consumer(powerDrawWatts: 250, isEnabled: false);

        consumer.Enable();

        Assert.True(consumer.IsEnabled);
    }

    [Fact]
    public void Constructor_WithZeroPowerDraw_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Consumer(powerDrawWatts: 0));
    }
}
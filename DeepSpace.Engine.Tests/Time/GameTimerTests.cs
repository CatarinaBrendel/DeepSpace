using DeepSpace.Engine.Time;

namespace DeepSpace.Engine.Tests.Time;

public sealed class GameTimerTests
{
    [Fact]
    public void Advance_WithDefaultTimeScale_AdvancesAtTwentyFourTimesRealTime()
    {
        var timer = new GameTimer();

        var elapsedGameTime = timer.Advance(TimeSpan.FromSeconds(1));

        Assert.Equal(TimeSpan.FromSeconds(24), elapsedGameTime);
        Assert.Equal(TimeSpan.FromSeconds(24), timer.ElapsedGameTime);
    }

    [Fact]
    public void Advance_WithOneRealHour_AdvancesOneGameDay()
    {
        var timer = new GameTimer();

        timer.Advance(TimeSpan.FromHours(1));

        Assert.Equal(TimeSpan.FromDays(1), timer.ElapsedGameTime);
    }

    [Fact]
    public void Advance_AccumulatesGameTime()
    {
        var timer = new GameTimer();

        timer.Advance(TimeSpan.FromMinutes(1));
        timer.Advance(TimeSpan.FromMinutes(1));

        Assert.Equal(TimeSpan.FromMinutes(48), timer.ElapsedGameTime);
    }

    [Fact]
    public void Constructor_WithCustomTimeScale_UsesCustomScale()
    {
        var timer = new GameTimer(timeScale: 2);

        var elapsedGameTime = timer.Advance(TimeSpan.FromSeconds(10));

        Assert.Equal(TimeSpan.FromSeconds(20), elapsedGameTime);
    }

    [Fact]
    public void Constructor_WithInvalidTimeScale_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new GameTimer(timeScale: 0));
    }

    [Fact]
    public void Advance_WithNegativeElapsedTime_Throws()
    {
        var timer = new GameTimer();

        Assert.Throws<ArgumentOutOfRangeException>(() => timer.Advance(TimeSpan.FromSeconds(-1)));
    }
}
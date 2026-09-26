namespace DeepSpace.Engine.Time;

public sealed class GameTimer
{
    public const double DefaultTimeScale = 24;

    public double TimeScale { get; }

    public TimeSpan ElapsedGameTime { get; private set; }

    public GameTimer(double timeScale = DefaultTimeScale)
    {
        if (timeScale <= 0)
            throw new ArgumentOutOfRangeException(nameof(timeScale), "Time scale must be greater than zero.");

        TimeScale = timeScale;
    }

    public TimeSpan Advance(TimeSpan elapsedRealTime)
    {
        if (elapsedRealTime < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(elapsedRealTime), "Elapsed real time must not be negative.");

        var elapsedGameTime = elapsedRealTime * TimeScale;

        ElapsedGameTime += elapsedGameTime;

        return elapsedGameTime;
    }
}
namespace DeepSpace.Domain.Power;

public sealed class Generator
{
    public double OutputWatts { get; }

    public bool IsRunning { get; private set; }

    public Generator(double outputWatts, bool isRunning = false)
    {
        if (outputWatts <= 0)
            throw new ArgumentOutOfRangeException(nameof(outputWatts), "Generator output must be greater than zero.");

        OutputWatts = outputWatts;
        IsRunning = isRunning;
    }

    public void Start()
    {
        IsRunning = true;
    }

    public void Stop()
    {
        IsRunning = false;
    }
}
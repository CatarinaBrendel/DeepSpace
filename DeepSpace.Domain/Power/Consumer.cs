namespace DeepSpace.Domain.Power;

public sealed class Consumer
{
    public double PowerDrawWatts { get; }

    public bool IsEnabled { get; private set; }

    public Consumer(double powerDrawWatts, bool isEnabled = true)
    {
        if (powerDrawWatts <= 0)
            throw new ArgumentOutOfRangeException(nameof(powerDrawWatts), "Power draw must be greater than zero.");

        PowerDrawWatts = powerDrawWatts;
        IsEnabled = isEnabled;
    }

    public void Enable()
    {
        IsEnabled = true;
    }

    public void Disable()
    {
        IsEnabled = false;
    }
}
using DeepSpace.Engine;

namespace DeepSpace.Server.Game;

public sealed class GameSession
{
    private readonly Lock _lock = new();
    private readonly GameSimulation _simulation;

    public GameSession(GameSimulation simulation)
    {
        ArgumentNullException.ThrowIfNull(simulation);

        _simulation = simulation;
    }

    public T Read<T>(Func<GameSimulation, T> read)
    {
        ArgumentNullException.ThrowIfNull(read);

        lock (_lock)
            return read(_simulation);
    }

    public T Update<T>(Func<GameSimulation, T> update)
    {
        ArgumentNullException.ThrowIfNull(update);

        lock (_lock)
            return update(_simulation);
    }
}
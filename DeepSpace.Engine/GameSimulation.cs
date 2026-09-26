using DeepSpace.Domain;
using DeepSpace.Engine.Power;
using DeepSpace.Engine.Time;

namespace DeepSpace.Engine;

public sealed class GameSimulation
{
    private readonly GameTimer _timer;
    private readonly PowerSimulation _powerSimulation;

    public Spacecraft Spacecraft { get; }

    public TimeSpan ElapsedGameTime => _timer.ElapsedGameTime;

    public GameSimulation(Spacecraft spacecraft)
    {
        ArgumentNullException.ThrowIfNull(spacecraft);

        Spacecraft = spacecraft;

        _timer = new GameTimer();
        _powerSimulation = new PowerSimulation();
    }

    public PowerSimulationResult Advance(TimeSpan elapsedRealTime)
    {
        var elapsedGameTime = _timer.Advance(elapsedRealTime);

        return _powerSimulation.Step(Spacecraft, elapsedGameTime);
    }
}
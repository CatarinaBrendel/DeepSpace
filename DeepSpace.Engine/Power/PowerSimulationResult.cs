namespace DeepSpace.Engine.Power;

public sealed record PowerSimulationResult(
    double GeneratedWh,
    double DemandedWh,
    double BatteryStoredWh,
    double BatterySuppliedWh,
    double UnmetDemandWh);
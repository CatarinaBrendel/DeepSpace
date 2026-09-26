namespace DeepSpace.Server.Game;

public sealed record GameStateResponse(
    double ElapsedGameSeconds,
    BatteryStateResponse? Battery,
    IReadOnlyList<GeneratorStateResponse> Generators);

public sealed record BatteryStateResponse(
    double CapacityWh,
    double ChargeWh,
    double StateOfCharge);

public sealed record GeneratorStateResponse(
    string Id,
    double OutputWatts,
    bool IsRunning);
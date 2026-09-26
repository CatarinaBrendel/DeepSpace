namespace DeepSpace.Server.Game;

public sealed record GameStateResponse(
    double ElapsedGameSeconds,
    BatteryStateResponse? Battery);

public sealed record BatteryStateResponse(
    double CapacityWh,
    double ChargeWh,
    double StateOfCharge);
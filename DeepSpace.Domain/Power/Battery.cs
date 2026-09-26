namespace DeepSpace.Domain.Power;

public sealed class Battery
{
    public double CapacityWh { get; }

    public double ChargeWh { get; private set; }

    public double StateOfCharge => ChargeWh / CapacityWh;

    public Battery(double capacityWh, double chargeWh)
    {
        if (capacityWh <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacityWh), "Battery capacity must be greater than zero.");

        if (chargeWh < 0 || chargeWh > capacityWh)
            throw new ArgumentOutOfRangeException(nameof(chargeWh), "Battery charge must be between zero and capacity.");

        CapacityWh = capacityWh;
        ChargeWh = chargeWh;
    }

    public double Store(double energyWh)
    {
        if (energyWh < 0)
            throw new ArgumentOutOfRangeException(nameof(energyWh), "Energy must not be negative.");

        var storedWh = Math.Min(energyWh, CapacityWh - ChargeWh);

        ChargeWh += storedWh;

        return storedWh;
    }

    public double Supply(double energyWh)
    {
        if (energyWh < 0)
            throw new ArgumentOutOfRangeException(nameof(energyWh), "Energy must not be negative.");

        var suppliedWh = Math.Min(energyWh, ChargeWh);

        ChargeWh -= suppliedWh;

        return suppliedWh;
    }
}
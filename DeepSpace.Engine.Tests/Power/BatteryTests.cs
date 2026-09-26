using DeepSpace.Domain.Power;

namespace DeepSpace.Engine.Tests.Power;

public sealed class BatteryTests
{
    [Fact]
    public void Constructor_WithValidValues_CreatesBattery()
    {
        var battery = new Battery(capacityWh: 5_000, chargeWh: 3_500);

        Assert.Equal(5_000, battery.CapacityWh);
        Assert.Equal(3_500, battery.ChargeWh);
        Assert.Equal(0.7, battery.StateOfCharge);
    }

    [Fact]
    public void Constructor_WithZeroCapacity_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Battery(capacityWh: 0, chargeWh: 0));
    }

    [Fact]
    public void Constructor_WithNegativeCharge_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Battery(capacityWh: 5_000, chargeWh: -1));
    }

    [Fact]
    public void Constructor_WithChargeAboveCapacity_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Battery(capacityWh: 5_000, chargeWh: 5_001));
    }
}
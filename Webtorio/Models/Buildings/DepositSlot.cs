namespace Webtorio.Models.Buildings;

public class DepositSlot : Slot
{
    public int DepositId { get; init; }
    public Deposit? Deposit { get; init; }
    public ExtractiveBuilding? ExtractiveBuilding => Building as ExtractiveBuilding;
}
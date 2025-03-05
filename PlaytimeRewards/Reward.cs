namespace PlaytimeRewards;

public class Reward
{
    public int ItemID { get; set; } = 0;
    public int Amount { get; set; } = 0;
    public bool IsAmountRandom { get; set; } = false;
    public bool IsPreHardmode { get; set; } = false;
    public bool IsHardmode { get; set; } = false;

    public Reward(int itemID, int amount, bool isAmountRandom, bool isPreHardmode, bool isHardmode)
    {
        ItemID = itemID;
        Amount = amount;
        IsAmountRandom = isAmountRandom;
        IsPreHardmode = isPreHardmode;
        IsHardmode = isHardmode;
    }
}


public class ChestModel 
{
    public ChestModel(ChestScriptableObject chestSO)
    {
        Type = chestSO.Type;
        Coins = UnityEngine.Random.Range(chestSO.minCoins, chestSO.maxCoins);
        Gems = UnityEngine.Random.Range(chestSO.minGems, chestSO.maxGems);
        TimeToUnlock = chestSO.TimeToUnlockInSeconds;
    }

    public string Type { get; private set; }
    public int Coins { get; private set; }
    public int Gems { get; private set; }
    public int TimeToUnlock { get; private set; }

    public void SetType(string type) { Type = type; }
    public void SetCoins(int coins) { Coins = coins; }
    public void SetGems(int gems) { Gems = gems; }
    public void SetTimeToUnlock(int time) { TimeToUnlock = time; }
}



public class ChestModel 
{
    public ChestModel(ChestScriptableObject chestSO)
    {
        Type = chestSO.Type;
        Coins = UnityEngine.Random.Range(chestSO.minCoins, chestSO.maxCoins);
        Gems = UnityEngine.Random.Range(chestSO.minGems, chestSO.maxGems);
        TimeToUnlock = chestSO.TimeToUnlockInSeconds;
    }

    public string Type { get; }
    public int Coins { get;  }
    public int Gems { get;  }
    public int TimeToUnlock { get; }

}

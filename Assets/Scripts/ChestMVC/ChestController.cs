using System;
using UnityEngine;

[Serializable]
public class ChestController 
{
    public string Type;
    public int Coins;
    public int Gems;
    public int TimeToUnlock;
    public string Status;
    public int UnlockGems;
    DateTime startTimeStamp;
    public bool addedToQueue;
    public bool empty;
    public bool locked;
    private Sprite emptySprite;
    public bool startTimer;

    public ChestController(ChestModel chestModel,ChestView chestPrefab,Sprite chestSprite)
    {
        ChestModel = chestModel;
        ChestView = GameObject.Instantiate<ChestView>(chestPrefab);
        ChestView.chestController = this;
        emptySprite=chestSprite;
        Debug.Log("ChestView Created", ChestView);
    }

    public ChestModel ChestModel { get; }
    public ChestView ChestView { get; }

    internal void MakeChestEmpty()
    {
        Type= "Empty";
        Coins = 0;
        Gems = 0;
        empty = true;
        Status = "Empty";
        addedToQueue = false;
        UnlockGems = 0;
        addedToQueue = false;
        ChestView.currentSprite = emptySprite;
        ChestView.DisplayChestData();
    }

    internal void AddChestToController(ChestScriptableObject chestSO, Sprite chestSprite)
    {
        locked = true;
        empty = false;
        Type = chestSO.Type;
        Coins = UnityEngine.Random.Range(chestSO.minCoins, chestSO.maxCoins);
        Gems = UnityEngine.Random.Range(chestSO.minGems, chestSO.maxGems);
        TimeToUnlock = chestSO.TimeToUnlockInSeconds;
        Status = "Locked";
        UnlockGems = CountGemsToUnlock(TimeToUnlock);
        ChestView.currentSprite = chestSprite;
        ChestView.DisplayChestData();
    }

    private int CountGemsToUnlock(int timeToUnlock)
    {
        int noOfGems = 0;
        int unlockTimeInMin = timeToUnlock / 60;
        noOfGems = unlockTimeInMin / 10;
        return noOfGems + 1;
    }

    internal void UnlockChestUsingGems()
    {
        bool chestCanBeUnlocked = Player.GetInstance().RemoveFromPlayer(UnlockGems);
        if (chestCanBeUnlocked)
            ChestUnlocked();
        else
        {
            ChestService.GetInstance().DisplayMessageOnPopUp("Unsufficient Gems. Cannot Unlock Chest");
        }
    }

    private void ChestUnlocked()
    {
        Debug.Log("Chest Unlocked");
        startTimer = false;
        locked = false;
        Status = "Unlocked";
        TimeToUnlock = 0;
        UnlockGems = 0;
        ChestView.DisplayChestData();
        ChestService.GetInstance().UnlockNextChest(ChestView);
    }

    internal bool isEmpty()
    {
        return empty;
    }

    public void ChestClicked()
    {
        string message;
        if (empty)
        {
            message = "Chest Slot is Empty";
            ChestService.GetInstance().DisplayMessageOnPopUp(message);
            return;
        }
        if (!locked)
        {
            Player.GetInstance().AddToPlayer(Coins, Gems);
            message = "Added " + Coins + " coins and " + Gems + " gems";
            ChestService.GetInstance().DisplayMessageOnPopUp(message);
            MakeChestEmpty();
        }
        else
        {
            message = "Chest is Locked.";
            ChestService.GetInstance().DisplayPopUp(this,addedToQueue,message,UnlockGems);
        }
    }

    internal void StartTimer()
    {
        startTimeStamp = DateTime.Now;
        Debug.Log("StartTimeStamp=" + startTimeStamp);
        startTimer = true;
        ChestService.GetInstance().timerStarted = true;
    }

    internal void StartUnlockingChest()
    {
        DateTime curtimer = DateTime.Now;
        int timer = GetSubSeconds(startTimeStamp, curtimer);
        //Debug.Log(timer);
        int timeLeft = TimeToUnlock - timer;
        UnlockGems = CountGemsToUnlock(timeLeft);
        ChestView.DisplayTimerAndUnlockGems(timeLeft, UnlockGems);
        if (timer >= TimeToUnlock)
        {
            Debug.Log("Chest Unlocked at " + curtimer);
            ChestUnlocked();
        }
    }

    public int GetSubSeconds(DateTime startTimer, DateTime endTimer)
    {
        TimeSpan startSpan = new TimeSpan(startTimer.Ticks);

        TimeSpan nowSpan = new TimeSpan(endTimer.Ticks);

        TimeSpan subTimer = nowSpan.Subtract(startSpan).Duration();

        //Return the time difference (the return value is the number of seconds of the difference)
        return subTimer.Seconds;
    }
}

using UnityEngine;

public class ChestController 
{
    public string Status;
    public int UnlockGems;
    //DateTime startTimeStamp;
    public bool addedToQueue;
    public bool empty;
    public bool locked;
    private Sprite emptySprite;
    Timer timer;
    //public bool startTimer;

    public ChestController(ChestModel chestModel,ChestView chestPrefab,Sprite chestSprite)
    {
        ChestModel = chestModel;
        ChestView = GameObject.Instantiate<ChestView>(chestPrefab);
        ChestView.chestController = this;
        emptySprite=chestSprite;
        Debug.Log("ChestView Created", ChestView);
        timer = ChestView.GetComponent<Timer>();
    }

    public ChestModel ChestModel { get; }
    public ChestView ChestView { get; }

    internal void MakeChestEmpty()
    {
        ChestModel.SetType("Empty");
        ChestModel.SetCoins(0);
        ChestModel.SetGems(0);
        ChestModel.SetTimeToUnlock(0);
        empty = true;
        Status = "Empty";
        addedToQueue = false;
        UnlockGems = 0;
        ChestView.currentSprite = emptySprite;
        ChestView.DisplayChestData();
    }

    internal void AddChestToController(ChestScriptableObject chestSO, Sprite chestSprite)
    {
        locked = true;
        empty = false;
        ChestModel.SetType(chestSO.Type);
        ChestModel.SetCoins(UnityEngine.Random.Range(chestSO.minCoins, chestSO.maxCoins));
        ChestModel.SetGems(UnityEngine.Random.Range(chestSO.minGems, chestSO.maxGems));
        ChestModel.SetTimeToUnlock(chestSO.TimeToUnlockInSeconds);
        Status = "Locked";
        UnlockGems = CountGemsToUnlock(ChestModel.TimeToUnlock);
        ChestView.currentSprite = chestSprite;
        ChestView.DisplayChestData();
    }

    public int CountGemsToUnlock(int timeToUnlock)
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

    public void ChestUnlocked()
    {
        Debug.Log("Chest Unlocked");
        //startTimer = false;
        timer.enabled = false;
        locked = false;
        Status = "Unlocked";
        ChestModel.SetTimeToUnlock(0);
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
            Player.GetInstance().AddToPlayer(ChestModel.Coins,ChestModel.Gems);
            message = "Added " + ChestModel.Coins + " coins and " + ChestModel.Gems + " gems";
            ChestService.GetInstance().DisplayMessageOnPopUp(message);
            MakeChestEmpty();
        }
        else
        {
            message = "Chest is Locked.";
            ChestService.GetInstance().DisplayPopUp(this,addedToQueue,message,UnlockGems);
        }
    }

    public void StartTimer()
    {
        //Activate Timer Script
        timer.SetController(this);
        timer.enabled = true;
    }
   /* public void StartTimer()
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
        int timeLeft = ChestModel.TimeToUnlock - timer;
        UnlockGems = CountGemsToUnlock(timeLeft);
        ChestView.DisplayTimerAndUnlockGems(timeLeft, UnlockGems);
        if (timer >= ChestModel.TimeToUnlock)
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
    }*/
}


using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    DateTime startTimeStamp;
    ChestController unlockingChest;
    [SerializeField] int timer;
    void Start()
    {
        StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        StartUnlockingChest();
    }

    public void SetController(ChestController controller)
    {
        unlockingChest = controller;
    }

    public void StartTimer()
    {
        startTimeStamp = DateTime.Now;
        Debug.Log("StartTimeStamp=" + startTimeStamp);
        ChestService.GetInstance().timerStarted = true;
    }

    public void StartUnlockingChest()
    {
        DateTime curtimer = DateTime.Now;
        timer = GetSubSeconds(startTimeStamp, curtimer);
        Debug.Log(timer);
        int timeLeft = unlockingChest.ChestModel.TimeToUnlock - timer;
        unlockingChest.UnlockGems = unlockingChest.CountGemsToUnlock(timeLeft);
        unlockingChest.ChestView.DisplayTimerAndUnlockGems(timeLeft, unlockingChest.UnlockGems);
        if (timer >= unlockingChest.ChestModel.TimeToUnlock)
        {
            Debug.Log("Chest Unlocked at " + curtimer);
            unlockingChest.ChestUnlocked();
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

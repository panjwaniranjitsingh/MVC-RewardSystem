using System;
using System.Collections.Generic;
using UnityEngine;

public class ChestService : MonoSingleton<ChestService>
{
    public GameObject ChestSlotGroup;
    [SerializeField] int NoOfChestSlots;
    ChestController[] ChestSlots;
    [SerializeField] ChestScriptableObjectList chestSOL;
    [SerializeField] List<ChestView> unlockingQueue;
    [SerializeField] int AllowedChestToUnlock = 3;
    [SerializeField] Sprite[] chestSprites;
    public bool timerStarted = false;
    [SerializeField] ChestView chestPrefab;
    ChestView PopUpChest;
    // Start is called before the first frame update
    void Start()
    {
        ChestSlots = new ChestController[NoOfChestSlots];
        for (int i = 0; i < NoOfChestSlots; i++)
            ChestSlots[i] = CreateEmptyChestSlot();
    }

    private ChestController CreateEmptyChestSlot()
    {
        return CreateNewChestController(chestSOL.Chests[chestSOL.Chests.Length - 1], chestPrefab, chestSprites[chestSprites.Length - 1]);
    }

    private ChestController CreateNewChestController(ChestScriptableObject chestScriptableObject, ChestView chestPrefab,Sprite chestSprite)
    {
        ChestModel chestModel = new ChestModel(chestScriptableObject);
        ChestController chestController = new ChestController(chestModel, chestPrefab,chestSprite);
        return chestController;
    }

    internal void StartUnlockingFirstChest()
    {
        unlockingQueue.Add(PopUpChest);
        Debug.Log("Unlocking queue=" + unlockingQueue.Count);
        PopUpChest.chestController.addedToQueue = true;
        PopUpChest.chestController.StartTimer();
    }

    internal void UnlockChestUsingGemsSelected()
    {
        PopUpChest.chestController.UnlockChestUsingGems();
    }

    public void CreateRandomChest()
    {
        int randomChest = UnityEngine.Random.Range(0, chestSOL.Chests.Length-1);
        AddChestToSlot(randomChest);
    }

    internal void AddChestToUnlockingQueue()
    {
        if (timerStarted && unlockingQueue.Count == AllowedChestToUnlock)
        {
            Debug.Log("Unlocking Queue Limit Reached");
            DisplayMessageOnPopUp("Unlocking Queue Limit Reached");
        }
        else
        {
            Debug.Log("Chest added to Unlocking Queue.");
            DisplayMessageOnPopUp("Chest added to Unlocking Queue.");
            unlockingQueue.Add(PopUpChest);
            PopUpChest.chestController.addedToQueue = true;
        }
    }

    public void AddChestToSlot(int chestIndex)
    {
        //Add Chest
        int chestSlotAlreadyOccupied = 0;
        for (int i = 0; i < ChestSlots.Length; i++)
        {
            if (ChestSlots[i].isEmpty())
            {
                ChestSlots[i].AddChestToController(chestSOL.Chests[chestIndex],chestSprites[chestIndex]);
                DisplayMessageOnPopUp("Chest Added to Slot:" + ++i);
                i = ChestSlots.Length + 1;
            }
            else
                chestSlotAlreadyOccupied++;
        }
        if (chestSlotAlreadyOccupied == ChestSlots.Length)
        {
            //All chest slots are filled
            Debug.Log("Chest not added. All slots are occupied");
            DisplayMessageOnPopUp("Chest not added. All slots are occupied");
        }
    }

    public void UnlockNextChest(ChestView unlockedChestView)
    {
        unlockingQueue.Remove(unlockedChestView);
        if(unlockingQueue.Count>0)
            unlockingQueue[0].chestController.StartTimer();
        
    }

    public void DisplayMessageOnPopUp(string message)
    {
        PopUpManager.GetInstance().OnlyDisplay(message);
    }

    public void DisplayPopUp(ChestController callingChest,bool chestAddedToQueue, string message, int gemsToUnlock)
    {
        PopUpChest = callingChest.ChestView;
        PopUpManager.GetInstance().DisplayPopUp(chestAddedToQueue, message, gemsToUnlock);
    }
}

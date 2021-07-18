using System;
using UnityEngine;
using UnityEngine.UI;

public class ChestView : MonoBehaviour
{
    public ChestController chestController;
    [SerializeField] Text TimerText;
    [SerializeField] Text TypeText;
    [SerializeField] Text CoinsText;
    [SerializeField] Text GemsText;
    [SerializeField] Text StatusText;
    [SerializeField] Text UnlockGemsText;
    public Sprite currentSprite;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(ChestService.GetInstance().ChestSlotGroup.transform);
        chestController.MakeChestEmpty();
    }

    // Update is called once per frame
    void Update()
    {
        if (chestController.startTimer)
        {
           chestController.StartUnlockingChest();
        }
    }

    public void DisplayChestData()
    {
        TimerText.text = "Timer:" + chestController.TimeToUnlock.ToString();
        TypeText.text = "Type:" + chestController.Type;
        GemsText.text = "Gems:" + chestController.Gems.ToString();
        CoinsText.text = "Coins:" + chestController.Coins.ToString();
        StatusText.text = "Status:" + chestController.Status;
        UnlockGemsText.text = "GemsToUnlock:" + chestController.UnlockGems.ToString();
        gameObject.GetComponent<Image>().sprite = currentSprite;
    }

    public void ChestButtonClicked()
    {
        chestController.ChestClicked();
    }

    public void DisplayTimerAndUnlockGems(int timeLeft,int UnlockGems)
    {
        TimerText.text = "Timer:" + timeLeft.ToString();
        UnlockGemsText.text = "GemsToUnlock:" + UnlockGems.ToString();
    }
}

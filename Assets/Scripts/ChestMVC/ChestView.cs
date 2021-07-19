
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
  /*  void Update()
    {
        if (chestController.startTimer)
        {
           chestController.StartUnlockingChest();
        }
    }
*/
    public void DisplayChestData()
    {
        TimerText.text = "Timer:" + ConvertTimerToReadable(chestController.ChestModel.TimeToUnlock);
        TypeText.text = "Type:" + chestController.ChestModel.Type;
        GemsText.text = "Gems:" + chestController.ChestModel.Gems.ToString();
        CoinsText.text = "Coins:" + chestController.ChestModel.Coins.ToString();
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
        TimerText.text = "Timer:" + ConvertTimerToReadable(timeLeft);
        UnlockGemsText.text = "GemsToUnlock:" + UnlockGems.ToString();
    }

    public string ConvertTimerToReadable(int timeInSec)
    {
        //Debug.Log("TimeInSec=" + timeInSec);
        string Time=timeInSec.ToString();
        if (timeInSec >= 60)
        {
            int min = timeInSec / 60;
            if (min >= 60)
                min = min % 60;
            int sec = timeInSec % 60;
            int hour = timeInSec / 3600;
            if(hour>0)
                Time =hour.ToString()+ "hr" + min.ToString() + "min" + sec.ToString();
            else
                Time = min.ToString() + "min" + sec.ToString();
        }
        return Time+"sec";
    }
}

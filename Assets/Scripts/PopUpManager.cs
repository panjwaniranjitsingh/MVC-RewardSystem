using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoSingleton<PopUpManager>
{
    [SerializeField] GameObject PopUpScreen;
    [SerializeField] Text Message;
    [SerializeField] Button firstButton;
    [SerializeField] Button secondButton;
    [SerializeField] Text firstButtonText;
    [SerializeField] Text secondButtonText;
    Coroutine PopUpCoroutine;
    public void DisplayPopUp(bool chestAddedToQueue, string message, int gemsToUnlock)
    {
        PopUpScreen.SetActive(true);
        if (PopUpCoroutine!=null)
            StopCoroutine(PopUpCoroutine);
        if(!ChestService.GetInstance().timerStarted)
            firstButtonText.text = "Start CountDown";
        else
            firstButtonText.text = "Add Chest to Unlocking Queue";

        secondButtonText.text = "Unlock using Gems:" + gemsToUnlock.ToString();
        Message.text = message;
        if(chestAddedToQueue)
            firstButton.transform.gameObject.SetActive(false);
        else
            firstButton.transform.gameObject.SetActive(true);
        secondButton.transform.gameObject.SetActive(true);
    }

    public void FirstButtonSelected()
    {
        PopUpScreen.SetActive(false);
        if (!ChestService.GetInstance().timerStarted)
        {
            Debug.Log("First Chest to be added to Unlocking Queue.");
            ChestService.GetInstance().StartUnlockingFirstChest();
        }
        else
            ChestService.GetInstance().AddChestToUnlockingQueue();
    }

    public void UnlockChestSelected()
    {
        PopUpScreen.SetActive(false);
        ChestService.GetInstance().UnlockChestUsingGemsSelected();
    }

    public void OnlyDisplay(string message)
    {
        PopUpScreen.SetActive(true);
        Message.text = message;
        firstButton.transform.gameObject.SetActive(false);
        secondButton.transform.gameObject.SetActive(false);
        PopUpCoroutine = StartCoroutine(DisablePopUp());
    }

    IEnumerator DisablePopUp()
    {
        yield return new WaitForSeconds(2f);
        PopUpScreen.SetActive(false);
    }
}

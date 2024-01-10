using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using SoundSystem;

public class PlayerKillController : Singleton<PlayerKillController>
{
    //The UI button with the Overlay color
    [SerializeField] Button KillButton;
    [SerializeField] Image KillButtonImage;
    [SerializeField] TMP_Text KillButtonCoolDownText;

    [SerializeField] float KillCoolDown = 10;
    bool canKill = true;

    PlayerKillDetector playerKillDetector;

    void Awake()
    {
        KillButtonCoolDownText.text = "";
        KillCoolDown = PlayerPrefs.GetInt("coolDown");
        KillButtonImage.fillAmount = 1;
        DisableKilling();
        StartCoroutine(ResetKill());
    }
    public IEnumerator ResetKill()
    {
        //Reset the Variables
        canKill = false;
        KillButton.interactable = false;
        KillButtonCoolDownText.text = "" + KillCoolDown;
        KillButtonImage.fillAmount = 0;

        //Start the Timer
        float TimeLeft = KillCoolDown;
        while (TimeLeft != 0)
        {
            yield return new WaitForSeconds(1);
            TimeLeft--;

            //Change the Time Text and the Image Fill Amount
            KillButtonCoolDownText.text = "" + TimeLeft;
            KillButtonImage.fillAmount = 1 - TimeLeft / KillCoolDown;
            if (GameManager.Instance.isDetectedBodyDead)
            {
                TimeLeft = 0;
            }
        }

        //if the current Player in range is Alive

        if (playerKillDetector != null)
            KillButton.interactable = true;


        //Set variables for the next kill
        KillButtonCoolDownText.text = "";
        KillButtonImage.fillAmount = 1;
        canKill = true;
    }

    //Called when the kill button is pressed
    public void Kill()
    {
        //Music
        SoundManager.Play("Kill");
        transform.position = playerKillDetector.gameObject.transform.position;
        playerKillDetector.Killed();

        //Send notification for the player to be killed


        //Start the CoolDown
        StartCoroutine(ResetKill());
    }

    #region Triggers

    //Called when a player enter the player collider
    public void EnableKilling(PlayerKillDetector playerKillDetector)
    {
        this.playerKillDetector = playerKillDetector;

        //this.playerKillDetector.Add(playerKillDetector);


        //if the cooldown is 0
        if (canKill)
        {
            KillButton.interactable = true;
        }


    }

    //Called when a player exit the player collider
    public void DisableKilling()
    {
        playerKillDetector = null;
        KillButton.interactable = false;
    }

    #endregion
}

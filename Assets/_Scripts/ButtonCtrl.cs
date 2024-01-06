using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCtrl : MonoBehaviour
{
    [SerializeField] private Button sabotageBtn;
    [SerializeField] private GameObject sabotageImg;
    [SerializeField] private bool isSabotageActive = false;

    [SerializeField] private Button lightBtn;
    [SerializeField] private Button destroyBtn;
    [SerializeField] Image lightImg;
    [SerializeField] Image destroyImg;
    [SerializeField] float coolDown;
    private void Start()
    {
        sabotageBtn.onClick.AddListener(() => SabotageButton());
        lightBtn.onClick.AddListener(() => StopEnemy());
        destroyBtn.onClick.AddListener(() => StopEnemy());
    }
    void SabotageButton()
    {
        isSabotageActive = !isSabotageActive;
        if (isSabotageActive)
        {
            sabotageImg.SetActive(true);
        }
        else
        {
            sabotageImg.SetActive(false);
        }
    }
    void StopEnemy()
    {
        StartCoroutine(ClickButton());
    }
    IEnumerator ClickButton()
    {
        lightBtn.interactable = false;
        lightImg.fillAmount = 0;
        destroyBtn.interactable = false;
        destroyImg.fillAmount = 0;
        float TimeLeft = coolDown;
        while (TimeLeft != 0)
        {
            yield return new WaitForSeconds(1);
            TimeLeft--;
            lightImg.fillAmount = 1 - TimeLeft / coolDown;
            destroyImg.fillAmount = 1 - TimeLeft / coolDown;
        }
        lightImg.fillAmount = 1;
        lightBtn.interactable = true;

        destroyImg.fillAmount = 1;
        destroyBtn.interactable = true;
    }
}

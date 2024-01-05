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
        Debug.Log("aaaa");
    }
}

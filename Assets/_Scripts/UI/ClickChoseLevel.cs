using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using SoundSystem;

public class ClickChoseLevel : MonoBehaviour
{
    //[SerializeField] private GameObject[] level;
    [SerializeField] private TMP_Text mapTxt;
    [SerializeField] private int maxMap;
    [SerializeField] private int index = 0;
    [SerializeField] private Button nextBtn;
    [SerializeField] private Button backBtn;
    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        PlayerPrefs.SetInt("map", index + 1);
        nextBtn.onClick.AddListener(() => NextButton());
        backBtn.onClick.AddListener(() => ForwardButton());
    }

    void NextButton()
    {
        //Music
        SoundManager.Play("Click");
        if (index >= maxMap - 1)
        {
            index = 0;
        }
        else
        {
            index++;
        }
        mapTxt.text = (index + 1).ToString();
        PlayerPrefs.SetInt("map", index + 1);
    }
    void ForwardButton()
    {
        //Music
        SoundManager.Play("Click");
        if (index <= 0)
        {
            index = maxMap - 1;
        }
        else
        {
            index--;
        }
        mapTxt.text = (index + 1).ToString();
        PlayerPrefs.SetInt("map", index + 1);
    }
}

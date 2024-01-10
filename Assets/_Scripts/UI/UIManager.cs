using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using SoundSystem;
using Unity.VisualScripting;
using System.Runtime.InteropServices;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TMP_Text nickName;
    private string nameString;

    [SerializeField] private Slider attackRange;
    [SerializeField] private TMP_Text attackRangeTxt;
    private float attackRangeNumber;
    [SerializeField] private float minValueRange = 10;
    [SerializeField] private float maxValueRange = 900;

    [SerializeField] private Slider coolDown;
    [SerializeField] private TMP_Text coolDownTxt;
    private float coolDownNumber;
    [SerializeField] private float minValueCoolDown = 10;
    [SerializeField] private float maxValueCoolDown = 900;

    [SerializeField] private Slider speed;
    [SerializeField] private TMP_Text speedTxt;
    private float speedNumber;
    [SerializeField] private float minValueSpeed = 10;
    [SerializeField] private float maxValueSpeed = 900;
    private void Awake()
    {
        PlayerPrefs.SetString("name", "player");
    }

    private void Start()
    {

        SetDefault();
        //Audio
        SoundManager.Play("Background");
    }

    void SetDefault()
    {
        PlayerPrefs.SetFloat("attackRange", 1);
        PlayerPrefs.SetInt("coolDown", 6);
        PlayerPrefs.SetInt("speed", 4);
        attackRange.value = 1 / maxValueRange;
        attackRangeTxt.text = "100";
        coolDown.value = 6 / maxValueCoolDown;
        coolDownTxt.text = "6";
        speed.value = 4 / maxValueSpeed;
        speedTxt.text = "4";

    }
    void NickName()
    {
        PlayerPrefs.SetString("name", $"{nickName.text}");
    }

    public void StartBtn()
    {
        //Music
        SoundManager.Play("Click");
        NickName();
        /*SliderEventAttackRange();
        SliderEventCoolDown();
        SliderEventSpeed();*/
        nameString = PlayerPrefs.GetString("name");
        PlayerPrefs.SetFloat("attackRange", attackRangeNumber);
        PlayerPrefs.SetInt("coolDown", (int)coolDownNumber);
        PlayerPrefs.SetInt("speed", (int)speedNumber);
        ChooseMap(PlayerPrefs.GetInt("map"));
    }
    public void SliderEventCoolDown()
    {
        //SoundManager.Play("Click");
        coolDownNumber = Mathf.RoundToInt(coolDown.value * (maxValueCoolDown - minValueCoolDown) + minValueCoolDown);
        coolDownTxt.text = coolDownNumber.ToString();
    }
    public void SliderEventAttackRange()
    {
        attackRangeNumber = attackRange.value * (maxValueRange - minValueRange) + minValueRange;
        attackRangeTxt.text = Mathf.RoundToInt(attackRangeNumber * 100).ToString();
    }
    public void SliderEventSpeed()
    {
        speedNumber = Mathf.RoundToInt(speed.value * (maxValueSpeed - minValueSpeed) + minValueSpeed);
        speedTxt.text = speedNumber.ToString();
    }
    void ChooseMap(int index)
    {
        print("id"+ index);
        int sceneLength = SceneManager.sceneCountInBuildSettings;
/*        if (index + 3 > sceneLength) return;*/
        if (index == 3)
        {
            print("testlink");
            Link();
        }
        else
        {
            SceneManager.LoadScene(index + 1);
            //SendAds("start");
            LogEvent(FirebaseEventType.EVENT_STARTGAME);
        }

    }
    void Link()
    {
        string url = GetLinkApk();
        Debug.Log(url);
        Application.OpenURL(url);
    }

    [DllImport("__Internal")]
    private static extern string GetLinkApk();
    //Ads
    [DllImport("__Internal")]
    private static extern void SendAds(string type);
    [DllImport("__Internal")]
    private static extern void LogEvent(string type);



}

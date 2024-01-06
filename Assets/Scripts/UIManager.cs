using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


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
        SliderEventAttackRange();
        SliderEventCoolDown();
        SliderEventSpeed();
    }
    void NickName()
    {
        PlayerPrefs.SetString("name", $"{ nickName.text}");
    }

    public void StartBtn()
    {
        NickName();
        SliderEventAttackRange();
        SliderEventCoolDown();
        SliderEventSpeed();
        nameString = PlayerPrefs.GetString("name");
        PlayerPrefs.SetInt("attackRange", (int)attackRangeNumber);
        PlayerPrefs.SetInt("coolDown", (int)coolDownNumber);
        PlayerPrefs.SetInt("speed", (int)speedNumber);
    }
    public void SliderEventCoolDown()
    {
        coolDownNumber = Mathf.RoundToInt(coolDown.value * (maxValueCoolDown - minValueCoolDown) + minValueCoolDown);
        coolDownTxt.text = coolDownNumber.ToString();
    }
    public void SliderEventAttackRange()
    {
        attackRangeNumber = Mathf.RoundToInt(attackRange.value * (maxValueRange - minValueRange) + minValueRange);
        attackRangeTxt.text = attackRangeNumber.ToString();
    }
    public void SliderEventSpeed()
    {
        speedNumber = Mathf.RoundToInt(speed.value * (maxValueSpeed - minValueSpeed) + minValueSpeed);
        speedTxt.text = speedNumber.ToString();
    }
}

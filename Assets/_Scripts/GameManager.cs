using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    [SerializeField] public Transform[] AllTranform;
    [SerializeField] public Slider silerTask;
    [SerializeField] public bool isDetectedBodyDead;
    [SerializeField] public GameObject bodyDead;
    [SerializeField] private PlayerMovement playerMovement;



    public List<MoveAI> AIs;
    //UIVote
    [SerializeField] private GameObject emerrgencyGO;
    [SerializeField] private GameObject voteGO;
    [SerializeField] private GameObject loseUIGO;
    [SerializeField] private Text textKick;
    //Bot
    public bool isLose = false;
    private void Start()
    {
        ResetRound();
    }

    public void ResetRound()
    {
        //RESET
        isDetectedBodyDead = false;
        playerMovement.ResetTransform();
        foreach (var i in AIs)
        {
            i.gameObject.transform.position = i.FirstTranfrom;
            /*            print("1" + i.gameObject.transform.position);
                        print("2" + i.FirstTranfrom.position);*/
        }
    }
    public void Vote()
    {
        StartCoroutine(ReturnGame());
    }
    IEnumerator ReturnGame()
    {
        //Emergency
        emerrgencyGO.SetActive(true);
        //Vote Kick
        yield return new WaitForSeconds(3f);
        emerrgencyGO.SetActive(false);
        voteGO.SetActive(true);
        StartCoroutine(DoneVote());

    }
    IEnumerator DoneVote()
    {
        //Done
        yield return new WaitForSeconds(5f);


        if (isLose)
        {
            textKick.text = PlayerPrefs.GetString("name") + "was ejected";
            LoseUI();
        }
        else
        {
            ResetRound();
            voteGO.SetActive(false);
        }

    }
    void LoseUI()
    {
        //TextLose
        loseUIGO.SetActive(true);
    }
    public void RemoveAI(MoveAI ai)
    {
        var index = AIs.IndexOf(ai);
        AIs.RemoveAt(index);
    }
    //BUTTON
    public void GoMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}

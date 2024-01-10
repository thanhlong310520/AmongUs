using DG.Tweening;
using SoundSystem;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
    [SerializeField] public List<Transform> listBodyDead;
    [SerializeField] public Slider silerTask;
    [SerializeField] public bool isDetectedBodyDead;
    [SerializeField] public GameObject bodyDead;
    [SerializeField] private PlayerMovement playerMovement;



    public List<MoveAI> AIs;
    //UIVote
    [SerializeField] private GameObject emerrgencyGO;
    [SerializeField] private GameObject voteGO;
    [SerializeField] private GameObject loseUIGO;
    [SerializeField] private GameObject winUIGO;
    [SerializeField] private Text textKick;
    [SerializeField] private Text textImposter;
    [SerializeField] private Text textImposterWin;
    [SerializeField] private GameObject imposterSprite;
    [SerializeField] private RectTransform target;
    [SerializeField] Sprite[] spriteColor;
    //Bot
    public bool isLose = false;
    public bool isVote = false;
    Vector3 firstTransformImposterSprite;
    private void Start()
    {
        firstTransformImposterSprite = imposterSprite.GetComponent<RectTransform>().position;
        silerTask.value = 0;
        listBodyDead = new List<Transform>();
        ResetRound();
    }

    public void ResetRound()
    {
        //RESET
        imposterSprite.SetActive(false);
        isDetectedBodyDead = false;
        playerMovement.ResetTransform();
        foreach (var i in AIs)
        {
            if (i != null)
            {
                i.transform.position = i.FirstTranfrom;
            }
        }
        RunAI();

        imposterSprite.GetComponent<RectTransform>().position = firstTransformImposterSprite;
        imposterSprite.transform.rotation = Quaternion.Euler(0, 0, 0);
        StartCoroutine(PlayerKillController.Instance.ResetKill());
    }

    public void StopAI()
    {
        foreach (var i in AIs)
        {
            if (i != null)
            {
                i.isRunning = false;
                i.Stop();
            }
        }
    }
    public void RunAI()
    {
        foreach (var i in AIs)
        {
            if (i != null)
            {
                i.isRunning = true;
                i.Move();
            }
        }
    }
    public void Vote()
    {
        SoundManager.Stop("Run");
        isVote = true;
        StartCoroutine(ReturnGame());
    }
    IEnumerator ReturnGame()
    {
        //Emergency
        //Music
        SoundManager.Play("Report");
        emerrgencyGO.SetActive(true);
        //Vote Kick
        yield return new WaitForSeconds(3f);
        VoteSytem();
        emerrgencyGO.SetActive(false);
        voteGO.SetActive(true);
        MoveSpriteInVote();
        StartCoroutine(DoneVote());

    }
    IEnumerator DoneVote()
    {
        yield return new WaitForSeconds(5f);

        if (isLose)
        {
            /*            imposterSprite.SetActive(true);
                        textKick.text = PlayerPrefs.GetString("name") + "was ejected";*/

            //imposterSprite.GetComponent<Image>().sprite = spriteColor[10];
            //imposterSprite.SetActive(true);

            GameOver();
        }
        else
        {
            //done
            ResetRound();
            voteGO.SetActive(false);
            CheckWin();

        }

    }
    void VoteSytem()
    {
        if (isLose)
        {
            imposterSprite.SetActive(true);
            textKick.text = PlayerPrefs.GetString("name") + " was ejected";
            imposterSprite.GetComponent<Image>().sprite = spriteColor[10];
        }
        else
        {
            if (Random.value * 100 < 50)
            {
                //nokick  
                textKick.text = "No one was ejected";
                imposterSprite.SetActive(false);
            }
            else
            {
                //kick
                int idKick = Random.Range(0, AIs.Count);
                string nameKick = AIs[idKick].NameAI;
                imposterSprite.GetComponent<Image>().sprite = spriteColor[idKick];
                imposterSprite.SetActive(true);
                textKick.text = nameKick + " was ejected";
                Destroy(AIs[idKick].gameObject);
                AIs.RemoveAt(idKick);

            }



        }

    }
    void CheckWin()
    {
        print("AI count : " + AIs.Count);
        if (AIs.Count <= 1)
        {
            Win();
        }
        isVote = false;
    }
    void Win()
    {
        //Music
        SoundManager.Play("Victory");
        textImposterWin.text = PlayerPrefs.GetString("name");
        winUIGO.SetActive(true);
    }
    public void GameOver()
    {
        //Music
        SoundManager.Play("Defeat");


        textImposter.text = PlayerPrefs.GetString("name");
        loseUIGO.SetActive(true);
    }
    public void ClearListBodyDead()
    {
        foreach (var item in listBodyDead)
        {
            Destroy(item.gameObject);
        }
        listBodyDead.Clear();
    }

    public void RemoveAI(MoveAI ai)
    {
        var index = AIs.IndexOf(ai);
        AIs.RemoveAt(index);
        CheckWin();
    }

    //Dotween
    void MoveSpriteInVote()
    {
        imposterSprite.GetComponent<RectTransform>().DOMove(target.position, 4);
        imposterSprite.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, 180), 4);
    }
    //BUTTON
    public void GoMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}

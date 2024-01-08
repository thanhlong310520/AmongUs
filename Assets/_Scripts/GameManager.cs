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
    [SerializeField] private GameObject imposterSprite;
    [SerializeField] Sprite[] spriteColor;
    //Bot
    public bool isLose = false;
    private void Start()
    {
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
        StartCoroutine(ReturnGame());
    }
    IEnumerator ReturnGame()
    {
        //Emergency
        emerrgencyGO.SetActive(true);
        //Vote Kick
        yield return new WaitForSeconds(3f);
        VoteSytem();
        emerrgencyGO.SetActive(false);
        voteGO.SetActive(true);
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
                CheckWin();
            }



        }

    }
    void CheckWin()
    {
        if(AIs.Count <= 1)
        {
            Win();
        }
    }
    void Win()
    {
        winUIGO.SetActive(true);
    }
    public void GameOver()
    {
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
        CheckWin() ;
    }
    //BUTTON
    public void GoMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}

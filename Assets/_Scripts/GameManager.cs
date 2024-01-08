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
    [SerializeField] private GameObject imposterSprite;
    [SerializeField] Sprite[] spriteColor;
    //Bot
    public bool isLose = false;
    private void Start()
    {
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
                i.gameObject.transform.position = i.FirstTranfrom;
            }

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

            loseUIGO.SetActive(true);
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
                int idKick = Random.Range(0, 9);
                string nameKick = AIs[idKick].NameAI;
                imposterSprite.GetComponent<Image>().sprite = spriteColor[idKick];
                imposterSprite.SetActive(true);
                textKick.text = nameKick + " was ejected";
                Destroy(AIs[idKick].gameObject);
            }



        }

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

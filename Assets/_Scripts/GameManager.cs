using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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

    [SerializeField] private GameObject voteGO;
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
        voteGO.SetActive(true);

        StartCoroutine(ReturnGame());
    }
    IEnumerator ReturnGame()
    {
        yield return new WaitForSeconds(5f);
        ResetRound();
        voteGO.SetActive(false);


    }
    public void RemoveAI(MoveAI ai)
    {
        var index = AIs.IndexOf(ai);
        AIs.RemoveAt(index);
    }

}

using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MoveAI : MonoBehaviour
{
    public ColorBot colorBot;
    public int IDBot;
    Transform target;
    NavMeshAgent agent;
    SkeletonAnimation anim;
    public bool isRunning = true;
    bool isRight = true;
    private string[] aiNames = {
"Player1", "GamerX", "ProGamer123", "GameMaster", "EpicPlayer",
    "SpeedyGamer", "NoobSlayer", "PixelWarrior", "StarGamer", "VictoryChaser",
    "GameAddict", "TheGamingNinja", "PlayfulPanda", "DragonGamer", "OnlineWarrior",
    "GameFreak", "CyberHero", "ControllerKing", "BattleLegend", "QuestHunter",
    "GameWizard", "VirtualHero", "GamingGuru", "UltraGamer", "JoystickJunkie",
    "ActionHero", "GameOverlord", "StealthGamer", "ConsoleMaster", "VictorySeeker",
    "GameJunkie", "PixelHunter", "Gametime", "Gamezilla", "MasterBlaster",
    "GamingProdigy", "DungeonCrawler", "GameGladiator", "GamerDude", "ShadowGamer",
    "GameOn", "VirtualWarrior", "QuestMaster", "GameGenius", "PlayerTwo",
    "ArcadeKing", "LegendOfGames", "GameSlinger", "TacticalGamer", "GameBender",
    "FantasyGamer", "GameHunter", "TheGamingBeast", "PowerPlayer", "GameFury",
    "GameChampion", "PixelPirate", "GameWarlock", "UltimateGamer", "ProPlayer",
    "GameSorcerer", "GamerGirl", "ControllerQueen", "BattleMistress", "PixelPrincess",
    "VictoryVixen", "GameGoddess", "JoystickQueen", "ActionAngel", "GameEmpress",
    "GamingQueen", "DungeonDiva", "GameMistress", "PixelHeroine", "QuestPrincess",
    "GamePixie", "VirtualPrincess", "GamingVixen", "UltraGamerGirl", "GameWarriorWoman",
    "GameLover", "ConsoleQueen", "VictoryVirtuosa", "GameDuchess", "GameExplorer",
    "TheGamingLady", "PlayerThree", "ArcadeQueen", "GameSorceress", "GamerGal",
    "TacticalGamerGirl", "GameBenderella", "FantasyGamerGirl", "GameHuntress", "TheGamingBeauty",
    "PowerPlayerGirl", "GameFuryGirl", "GameChampioness", "PixelPirateLady", "GameWarlockess",
    "UltimateGamerGirl", "ProGamerGirl", "GameSorceress", "GamerGoddess", "ControllerGoddess",
    "BattleMistress", "PixelPrincess", "VictoryVixen", "GameGoddess", "JoystickQueen",
    "ActionAngel", "GameEmpress", "GamingQueen", "DungeonDiva", "GameMistress",
    "PixelHeroine", "QuestPrincess", "GamePixie", "VirtualPrincess", "GamingVixen",
    "UltraGamerGirl", "GameWarriorWoman", "GameLover", "ConsoleQueen", "VictoryVirtuosa",
    "GameDuchess", "GameExplorer", "TheGamingLady", "PlayerFour", "ArcadeQueen",
    "GameSorceress", "GamerGal", "TacticalGamerGirl", "GameBenderella", "FantasyGamerGirl",
    "GameHuntress", "TheGamingBeauty", "PowerPlayerGirl", "GameFuryGirl", "GameChampioness",
    "PixelPirateLady", "GameWarlockess", "UltimateGamerGirl", "ProGamerGirl", "GameSorceress",
    "GamerGoddess", "ControllerGoddess", "BattleMistress", "PixelPrincess", "VictoryVixen",
    "GameGoddess", "JoystickQueen", "ActionAngel", "GameEmpress", "GamingQueen",
    "DungeonDiva", "GameMistress", "PixelHeroine", "QuestPrincess", "GamePixie",
    "VirtualPrincess", "GamingVixen", "UltraGamerGirl", "GameWarriorWoman", "GameLover",
    "ConsoleQueen", "VictoryVirtuosa", "GameDuchess", "GameExplorer", "TheGamingLady",
    "PlayerFive", "ArcadeQueen", "GameSorceress", "GamerGal", "TacticalGamerGirl",
    "GameBenderella", "FantasyGamerGirl", "GameHuntress", "TheGamingBeauty", "PowerPlayerGirl"
    };
    public Vector3 FirstTranfrom;
    public string NameAI;
    [SerializeField] Text NameAIText;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<SkeletonAnimation>();
        FirstTranfrom = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0);
        //NAME AI
        int randomNum = Random.Range(0, aiNames.Length);
        NameAI = aiNames[randomNum];
        NameAIText.text = NameAI;
        CheckId();
    }
    private void Start()
    {

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        anim.initialSkinName = colorBot.ToString();
        anim.Initialize(true);

        //print(colorBot.ToString()); 
        ChangerTarget();
        //GameManager.Instance.AIs.Add(this);
        Move();
    }

    private void Update()
    {
        if (agent.velocity.x > 0 && !isRight)
        {
            Flip();
        }
        if (agent.velocity.x < 0 && isRight)
        {
            Flip();
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Task")
        {
            if (target == collision.transform)
            {
                Stop();
                StartCoroutine(UpdateTask());
            }
        }
    }

    IEnumerator UpdateTask()
    {
        ChangerTarget();
        yield return new WaitForSeconds(5f);
        if (isRunning)
        {
            if (GameManager.Instance.silerTask.value < 1)
            {
                GameManager.Instance.silerTask.value += 0.01f;
            }
            else
            {
                //lose
                GameManager.Instance.silerTask.value = 1f;
                GameManager.Instance.GameOver();

            }
            Move();
            yield return new WaitForSeconds(1f);
        }

    }

    void CheckId()
    {
        if (colorBot == ColorBot.black)
        {
            IDBot = 0;
        }
        else if (colorBot == ColorBot.blue)
        {

            IDBot = 1;
        }
        else if (colorBot == ColorBot.brown)
        {

            IDBot = 2;
        }
        else if (colorBot == ColorBot.crystal)
        {

            IDBot = 3;
        }

        else if (colorBot == ColorBot.green)
        {

            IDBot = 4;
        }
        else if (colorBot == ColorBot.greenjungle)
        {

            IDBot = 5;
        }
        else if (colorBot == ColorBot.orange)
        {

            IDBot = 6;
        }
        else if (colorBot == ColorBot.pink)
        {

            IDBot = 7;
        }
        else if (colorBot == ColorBot.purple)
        {

            IDBot = 8;
        }
        else
        {

            IDBot = 9;
        }
    }
    private void ChangerTarget()
    {
        isRunning = true;

        while (true)
        {
            int i = Random.Range(0, GameManager.Instance.AllTranform.Length);
            if (target != GameManager.Instance.AllTranform[i])
            {
                target = GameManager.Instance.AllTranform[i];
                break;
            }

        }
        //botAnim.SetBool("isRun", true);

    }
    public void Move()
    {
        if (target == null) return;
        agent.isStopped = false;
        agent.SetDestination(target.position);

        anim.AnimationName = "run";
        anim.loop = true;
        anim.Initialize(true);
    }
    public void Stop()
    {
        agent.velocity = Vector2.zero;
        agent.isStopped = true;
        anim.AnimationName = "stopandlose";
        anim.Initialize(true);
    }
    void Flip()
    {
        if (isRight)
        {
            isRight = false;
            anim.initialFlipX = false;
            anim.Initialize(true);
        }
        else
        {
            isRight = true;
            anim.initialFlipX = true;
            anim.Initialize(true);
        }
    }
}
public enum ColorBot
{
    black,
    blue,
    brown,
    crystal,
    green,
    greenjungle,
    orange,
    pink,
    purple,
    red,
    white,
    yellow
}
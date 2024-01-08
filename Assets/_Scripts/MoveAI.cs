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
    Transform target;
    NavMeshAgent agent;
    SkeletonAnimation anim;
    bool isRunning = true;
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
        anim = GetComponent<SkeletonAnimation>();
        agent = GetComponent<NavMeshAgent>();
        FirstTranfrom = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0);
        //NAME AI
        int randomNum = Random.Range(0, aiNames.Length);
        NameAI = aiNames[randomNum];
        NameAIText.text = NameAI;
    }
    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        anim.initialSkinName = colorBot.ToString();
        anim.Initialize(true);

        print(colorBot.ToString()); 
        ChangerTarget();
        GameManager.Instance.AIs.Add(this);
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
            Stop();
            StartCoroutine(UpdateTask());
        }
    }

    IEnumerator UpdateTask()
    {
        yield return new WaitForSeconds(5f);
        if (isRunning)
        {
            ChangerTarget();
            if (GameManager.Instance.silerTask.value < 1)
            {
                GameManager.Instance.silerTask.value += 0.01f;
            }
            else
            {
                //lose
                GameManager.Instance.silerTask.value = 1f;
            }
            Move();
        }

    }


    private void ChangerTarget()
    {
        isRunning = true;

        //botAnim.SetBool("isRun", true);
        int i = Random.Range(0, 10);
        target = GameManager.Instance.AllTranform[i];
    }
    void Move()
    {
        agent.isStopped = false;
        agent.SetDestination(target.position);

        anim.AnimationName = "run";
        anim.loop = true;
        anim.Initialize(true);
    }
    void Stop()
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
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
        ChangerTarget();
        GameManager.Instance.AIs.Add(this);
    }

    private void Update()
    {
        if (!isRunning || GameManager.Instance.isDetectedBodyDead) return;

        if ((target.position.x - this.transform.position.x) < 0.1f && (target.position.y - this.transform.position.y) < 0.1f)
        {
            //idle
            isRunning = false;
            anim.AnimationName = "stopandlose";
            anim.Initialize(true);
            //botAnim.SetBool("isRun", false);
            StartCoroutine(UpdateTask());
        }
        else
        {
            //move
            agent.SetDestination(target.position);
            //flip
            if ((target.position.x - this.transform.position.x) < 0f)
            {
                //botSprite.flipX = true;
                anim.initialFlipX = false;
                anim.Initialize(true);
            }
            else
            {
                //botSprite.flipX = false;
                anim.initialFlipX = true;
                anim.Initialize(true);
            }
        }

    }
    IEnumerator UpdateTask()
    {
        yield return new WaitForSeconds(5f);

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

    }
    private void ChangerTarget()
    {
        isRunning = true;
        anim.AnimationName = "run";
        anim.loop = true;
        anim.Initialize(true);

        //botAnim.SetBool("isRun", true);
        int i = Random.Range(0, 10);
        target = GameManager.Instance.AllTranform[i];
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
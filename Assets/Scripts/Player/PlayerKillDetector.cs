using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKillDetector : MonoBehaviour
{
    public BotBodyDetection botBodyDetection;
    public void Killed()
    {
        var listBotAroud = botBodyDetection.CheckObjAround(botBodyDetection.botLayer);
        
        if (listBotAroud.Count > 0)
        {
            print("kill player");
        }
        SpawnBodyDead();
        Destroy(gameObject);

    }
    void SpawnBodyDead()
    {
        GameObject body = Instantiate(GameManager.Instance.bodyDead,transform.position,Quaternion.identity);
    }
}

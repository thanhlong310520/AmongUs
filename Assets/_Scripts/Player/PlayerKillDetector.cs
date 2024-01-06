using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKillDetector : MonoBehaviour
{
    public BotBodyDetection botBodyDetection;
    public void Killed()
    {
        GameManager.Instance.RemoveAI(GetComponent<MoveAI>());
        SpawnBodyDead();
        Destroy(gameObject);


    }
    void SpawnBodyDead()
    {
        GameObject body = Instantiate(GameManager.Instance.bodyDead,transform.position,Quaternion.identity);
    }
}
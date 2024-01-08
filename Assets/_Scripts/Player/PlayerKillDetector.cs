using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKillDetector : MonoBehaviour
{
    public BotBodyDetection botBodyDetection;
    public CircleCollider2D circleColi;
    private void OnValidate()
    {
        circleColi = GetComponent<CircleCollider2D>();
    }
    private void Awake()
    {
        circleColi.radius = PlayerPrefs.GetFloat("attackRange");
    }
    public void Killed()
    {
        GameManager.Instance.RemoveAI(GetComponent<MoveAI>());
        SpawnBodyDead();
        Destroy(gameObject);
    }
    void SpawnBodyDead()
    {
        GameObject body = Instantiate(GameManager.Instance.bodyDead,transform.position,Quaternion.identity);
        GameManager.Instance.listBodyDead.Add(body.transform);
    }
}
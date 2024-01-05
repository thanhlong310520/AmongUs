using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBodyDetection : MonoBehaviour
{
    public BotCtrl botCtrl;
    public float radius = 1f;
    public float timeCheck = 0.5f;
    public LayerMask botLayer;
    public LayerMask barrierLayer;
    // Start is called before the first frame update
    void Start()
    {
        StartFindBotAround();
    }

    void StartFindBotAround()
    {
        StartCoroutine(GetBotAround());
    }

    IEnumerator GetBotAround()
    {
        while (true)
        {
            if (botCtrl.isDead) break;

            yield return new WaitForSeconds(timeCheck);

            bool isHandle = false;
            foreach (Transform t in CheckObjAround(botLayer))
            {
                Debug.Log(t.name);
                if (CheckDead(t.GetComponent<BotCtrl>()))
                {
                    botCtrl.HandleAfterDetectBody();
                    isHandle = true;
                    break;
                }
            }
            if (isHandle) break;
        }
    }

    public List<Transform> CheckObjAround(LayerMask targetLayer)
    {
        List<Transform> list = new List<Transform>();
        var botDetect = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);
        foreach (var item in botDetect)
        {
            if (!CheckBlock(item.transform))
            {
                list.Add(item.transform);
            }
        }
        return list;
    }

    bool CheckBlock(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        if (!Physics2D.Raycast(transform.position, direction, Vector3.Distance(transform.position, target.position), barrierLayer))
        {
            return false;
        }
        return true;
    }
    bool CheckDead(BotCtrl targetBot)
    {
        if (targetBot.isDead) return true;        
        return false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

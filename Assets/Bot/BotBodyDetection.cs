﻿using System;
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
    public LayerMask bodyDeadLayer;

    bool isDetected = false;
    

    private void Update()
    {
        if (isDetected) return;

        var listBodyDead = CheckObjAround(bodyDeadLayer);  
        if(listBodyDead.Count > 0)
        {
            isDetected = true;
            Debug.Log("phat hien xac");
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
    public void HandleAfterDetectBody()
    {
        Debug.Log("xu ly sau khi phat hien xac");
        FindPlayer();
    }

    void FindPlayer()
    {
        var listPlayer = CheckObjAround(LayerMask.GetMask("Player"));
        if (listPlayer.Count > 0)
        {
            Debug.Log("Phat hien Player");
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

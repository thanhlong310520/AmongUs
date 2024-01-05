using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BotCtrl : MonoBehaviour
{
    public bool isDead = false;
    public BotBodyDetection botBodyDetection;

    public void HandleAfterDetectBody()
    {
        Debug.Log("xu ly sau khi phat hien xac");
        FindPlayer();
    }

    void FindPlayer()
    {
        var listPlayer =  botBodyDetection.CheckObjAround(LayerMask.GetMask("Player"));
        if (listPlayer.Count > 0)
        {
            Debug.Log("Phat hien Player");
        }
    }
}

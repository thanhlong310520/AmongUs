using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BotCtrl : MonoBehaviour
{
    public BotBodyDetection botBodyDetection;

    public void IsKill()
    {
        var listBotAroud = botBodyDetection.CheckObjAround(botBodyDetection.botLayer);
        if(listBotAroud.Count > 0)
        {
            print("kill player");
        }
    }
}

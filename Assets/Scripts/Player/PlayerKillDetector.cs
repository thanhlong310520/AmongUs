using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKillDetector : MonoBehaviour
{
    public bool isAlive = true;
    public Sprite killedsprite;

    public void Killed()
    {
        isAlive = false;
        //GetComponent<Animator>().SetTrigger("Dead");
        GetComponent<CircleCollider2D>().enabled = false;
        //GetComponent<AudioSource>().Play();
        GetComponent<SpriteRenderer>().sprite = killedsprite;
    }
}

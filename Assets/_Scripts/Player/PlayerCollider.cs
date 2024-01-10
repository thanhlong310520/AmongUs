using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField] LayerMask barrierLayer;
    PlayerKillController playerKillController;
    PlayerMovement playerMovement;
    private void Awake()
    {
        playerKillController = GetComponent<PlayerKillController>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Vent")
        {
            //If the Imposter is activated skip
            //This is used to move the imposter around without activating the triggers
            if (playerMovement.IsInVent())
            {
                collision.gameObject.GetComponent<Vent>().EnableVent(playerMovement);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            if (!CheckBlock(collision.transform.transform)) {
                //Debug.Log(collision.name);
                playerKillController.EnableKilling(collision.GetComponent<PlayerKillDetector>());
            }
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Vent")
        {
            //If the Imposter is activated skip
            //to not allow killing from the vent
            if (playerMovement.IsInVent())
                collision.gameObject.GetComponent<Vent>().DisableVent();
        }
        if (collision.gameObject.tag == "enemy")
        {
            playerKillController.DisableKilling();
        }
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
}

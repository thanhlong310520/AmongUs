using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
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
            playerKillController.EnableKilling(collision.GetComponent<PlayerKillDetector>());
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
}

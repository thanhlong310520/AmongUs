using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    PlayerKillController playerKillController;
    private void Awake()
    {
        playerKillController = GetComponent<PlayerKillController>();
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
        if (collision.gameObject.tag == "enemy")
        {
            playerKillController.DisableKilling();
        }
    }
}

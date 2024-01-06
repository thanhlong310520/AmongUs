using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveVector;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private SpriteRenderer sprite;
    bool isFacingRight = false;
    [SerializeField] bool isMove = true;

    //Other Classes
    VentsSystem ventsSystem;

    private void Awake()
    {
        isFacingRight = false;
    }
    public void InputPlayer(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
    }
    private void FixedUpdate()
    {
        if (isMove)
        {
            moveVector.Normalize();
            transform.Translate(moveSpeed * moveVector * Time.deltaTime);
            if (moveVector.x > 0 && isFacingRight)
            {
                Flip();
            }
            if (moveVector.x < 0 && !isFacingRight)
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        //transform.Rotate(0, 180, 0);
        if (moveVector.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    #region Vent Movement Control
    public void EnterVent(VentsSystem ventsSystem)
    {
        this.ventsSystem = ventsSystem;
        VentEntered();
        //Animation and sounds
        //playerAnimator.SetTrigger("Vent");
        //playerAudioController.StopWalking();
        //playerAudioController.PlayVent();
    }

    public void VentEntered()
    {
        DisablePlayer();

        ventsSystem.PlayerInVent();
        rigid.simulated = false;
        isMove = false;
    }

    public bool IsInVent()
    {
        return GetComponent<Rigidbody2D>().simulated;
    }

    public void VentExited()
    {
        EnablePlayer();

        //sounds
        //playerAudioController.PlayVent();
    }
    void DisablePlayer()
    {
        Color c = sprite.color;
        c.a = 0;
        sprite.color = c;
        rigid.simulated = false;
        isMove = false;
    }
    void EnablePlayer()
    {
        Color c = sprite.color;
        c.a = 1;
        sprite.color = c;
        rigid.simulated = true;
        isMove = true;
    }
    #endregion
}

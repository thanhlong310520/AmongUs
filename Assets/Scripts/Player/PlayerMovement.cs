using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveVector;
    [SerializeField] private float moveSpeed;
    /*[SerializeField] private Rigidbody2D rigid;
    [SerializeField] private PlayerInput playerInput;*/
    bool isFacingRight = true;
    [SerializeField] bool isMove = true;
    private void Awake()
    {
        isFacingRight = false;
    }
    public void InputPlayer(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
    }
    private void Update()
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
}

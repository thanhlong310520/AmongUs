using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Text PlayerText;
    Vector2 moveVector;
    Vector2 moveVectorJoystick;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rigid;
    bool isFacingRight = false;
    [SerializeField] bool isMove = true;
    [SerializeField] SkeletonAnimation skeleton;
    [SerializeField] Vector2 firstPos;

    bool isUseVent;

    //Other Classes
    VentsSystem ventsSystem;


    private void Awake()
    {
        moveSpeed = PlayerPrefs.GetInt("speed");
        isFacingRight = true;
        firstPos = transform.position;
        PlayerText.text = PlayerPrefs.GetString("name");
    }
    private void Start()
    {
        isUseVent = false;
        //firstPos = transform.position;
    }
    public void InputPlayer(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
    }
    private void FixedUpdate()
    {
        if (isMove)
        {
            moveVectorJoystick = new Vector2(UltimateJoystick.GetHorizontalAxis("joystick"), UltimateJoystick.GetVerticalAxis("joystick")) * moveSpeed;
            if (Input.GetButton("Horizontal"))
            {
                moveVectorJoystick.x = Input.GetAxisRaw("Horizontal");
            }
            if (Input.GetButton("Vertical"))
            {
                moveVectorJoystick.y = Input.GetAxisRaw("Vertical");
            }
            /*moveVector.Normalize();
            rigid.velocity = moveSpeed * moveVector;*/

            //rigid.velocity = moveSpeed * moveVectorJoystick;
            moveVectorJoystick.Normalize();
            rigid.velocity = moveVectorJoystick * moveSpeed;
            //transform.Translate(moveSpeed * moveVectorJoystick * Time.deltaTime);
            if (moveVectorJoystick.x > 0 && isFacingRight)
            {
                Flip();
            }
            if (moveVectorJoystick.x < 0 && !isFacingRight)
            {
                Flip();
            }
            /*if (moveVector != Vector2.zero)
            {
                skeleton.AnimationName = "run";
            }
            else skeleton.AnimationName = "stopandlose";*/
            StartCoroutine(DelayAnim());
        }
    }
    IEnumerator DelayAnim()
    {
        yield return new WaitForSeconds(0.1f);
        if (moveVectorJoystick != Vector2.zero)
        {
            skeleton.AnimationName = "run";
        }
        else skeleton.AnimationName = "stopandlose";
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        if (moveVectorJoystick.x < 0)
        {
            skeleton.initialFlipX = false;
        }
        else
        {
            skeleton.initialFlipX = true;
        }
        skeleton.Initialize(true);
    }
    public void ResetTransform()
    {
        transform.position = firstPos;
        if (isUseVent)
        {
            EnablePlayer();
            ventsSystem.arrowsManager.ResetArrows();
        }
    }
    #region Vent Movement Control
    public void EnterVent(VentsSystem ventsSystem)
    {
        this.ventsSystem = ventsSystem;
        VentEntered();
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

    }
    void DisablePlayer()
    {
        isUseVent = true;
        skeleton.initialSkinName = "default";
        skeleton.Initialize(true);
        rigid.simulated = false;
        isMove = false;
    }
    void EnablePlayer()
    {
        isUseVent = false;
        skeleton.initialSkinName = "red";
        skeleton.Initialize(true);
        rigid.simulated = true;
        isMove = true;
    }
    #endregion
}

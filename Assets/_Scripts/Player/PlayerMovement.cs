using SoundSystem;
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
    bool isSound = false;

    //Other Classes
    VentsSystem ventsSystem;


    private void Awake()
    {
        moveSpeed = PlayerPrefs.GetInt("speed");
        isFacingRight = true;
        firstPos = transform.position;
        PlayerText.text = PlayerPrefs.GetString("name");
        SoundManager.Play("Run");
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
        if (isMove && !GameManager.Instance.isLose && !GameManager.Instance.isVote)
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
            moveVectorJoystick.Normalize();
            rigid.velocity = moveVectorJoystick * moveSpeed;
            if (moveVectorJoystick.x > 0 && isFacingRight)
            {
                Flip();
            }
            if (moveVectorJoystick.x < 0 && !isFacingRight)
            {
                Flip();
            }
            //Music
            StartCoroutine(DelayAnim());
        }
        else
        {
            SoundManager.Stop("Run");
        }
    }
    IEnumerator DelayAnim()
    {
        yield return new WaitForSeconds(0.1f);

        if (moveVectorJoystick != Vector2.zero)
        {
            skeleton.AnimationName = "run";

            if (!isSound && !GameManager.Instance.isLose)
            {
                isSound = true;
                SoundManager.PlayContinue("Run");
            }



        }
        else
        {
            SoundManager.Pause("Run");

            skeleton.AnimationName = "stopandlose";
            if (isSound && !GameManager.Instance.isLose)
            {
                isSound = false;
                SoundManager.PlayContinue("Run");
            }
        }
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

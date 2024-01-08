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
            moveVector.Normalize();
            rigid.velocity = moveSpeed * moveVector;
            //transform.Translate(moveSpeed * moveVector * Time.deltaTime);
            if (moveVector.x > 0 && isFacingRight)
            {
                Flip();
            }
            if (moveVector.x < 0 && !isFacingRight)
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
        if (moveVector != Vector2.zero)
        {
            skeleton.AnimationName = "run";
        }
        else skeleton.AnimationName = "stopandlose";
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        //transform.Rotate(0, 180, 0);
        if (moveVector.x < 0)
        {
            skeleton.initialFlipX = false;
            //sprite.flipX = true;
            //transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            skeleton.initialFlipX = true;
            //sprite.flipX = false;
            //transform.localScale = new Vector3(1, 1, 1);
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

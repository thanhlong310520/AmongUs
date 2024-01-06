using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAI : MonoBehaviour
{
    Transform target;
    NavMeshAgent agent;
    Animator botAnim;
    SpriteRenderer botSprite;
    bool isRunning = true;

    public Vector3 FirstTranfrom;

    private void Awake()
    {
        botSprite = GetComponent<SpriteRenderer>();
        botAnim = GetComponent<Animator>();
        botAnim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        FirstTranfrom = new Vector3( this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0);
    }
    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        ChangerTarget();
        GameManager.Instance.AIs.Add(this);
    }

    private void Update()
    {
        if (!isRunning || GameManager.Instance.isDetectedBodyDead) return;

        if ((target.position.x - this.transform.position.x) < 0.1f && (target.position.y - this.transform.position.y) < 0.1f)
        {
            isRunning = false;
            botAnim.SetBool("isRun", false);
            StartCoroutine(UpdateTask());
        }
        else
        {
            //move
            agent.SetDestination(target.position);
            //flip
            if ((target.position.x - this.transform.position.x) < 0f)
            {
                botSprite.flipX = true;
            }
            else
            {
                botSprite.flipX = false;
            }
        }

    }
    IEnumerator UpdateTask()
    {
        yield return new WaitForSeconds(5f);

        ChangerTarget();
        if (GameManager.Instance.silerTask.value < 1)
        {
            GameManager.Instance.silerTask.value += 0.01f;
        }
        else
        {
            //lose
            GameManager.Instance.silerTask.value = 1f;
        }

    }
    private void ChangerTarget()
    {
        isRunning = true;
        botAnim.SetBool("isRun", true);
        int i = Random.Range(0, 10);
        target = GameManager.Instance.AllTranform[i];
    }




}
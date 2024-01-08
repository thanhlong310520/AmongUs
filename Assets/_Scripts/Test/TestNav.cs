using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNav : MonoBehaviour
{
    public Transform[] listTarget;
    Transform target;
    NavMeshAgent agent;
    Animator botAnim;
    SpriteRenderer botSprite;
    bool isRunning = true;
    bool isRight = true;

    public Vector3 FirstTranfrom;

    private void Awake()
    {
        botSprite = GetComponent<SpriteRenderer>();
        botAnim = GetComponent<Animator>();
        botAnim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        FirstTranfrom = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0);
    }
    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        ChangerTarget();
        Move();

    }

    // Update is called once per frame
    void Update()
    {
        if (agent.velocity.x > 0 && !isRight)
        {
            Flip();
        }
        if (agent.velocity.x < 0 && isRight)
        {
            Flip();
        }

        if (Input.GetMouseButtonDown(0))
        {
            isRunning = false;
            transform.position = FirstTranfrom;
            Stop(); 
        }
        if (Input.GetMouseButtonDown(1))
        {
            isRunning=true;
            Move(); 
        }
    }
    private void ChangerTarget()
    {
        isRunning = true;
        botAnim.SetBool("isRun", true);
        while(true)
        {
            int i = Random.Range(0, 10);
            if (listTarget[i] != target) 
            {
                target = listTarget[i];
                break;
            }
            
        }
    }
    void Flip()
    {
        if (isRight)
        {
            isRight = false;
            botSprite.flipX = true;
        }
        else
        {
            isRight = true;
            botSprite.flipX = false;
        }
    }

    IEnumerator UpdateTask()
    {
        yield return new WaitForSeconds(2f);
        if(isRunning)
        {
            ChangerTarget();
            Move();
        }
        //if (GameManager.Instance.silerTask.value < 1)
        //{
        //    GameManager.Instance.silerTask.value += 0.01f;
        //}
        //else
        //{
        //    //lose
        //    GameManager.Instance.silerTask.value = 1f;
        //}

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "player")
        {
            Stop();
            StartCoroutine(UpdateTask());
        }
    }

    void Move()
    {
        agent.isStopped = false;
        agent.SetDestination(target.position);
        botAnim.SetBool("isRun", true);
    }
    void Stop()
    {
        agent.velocity = Vector2.zero;
        agent.isStopped = true;
        botAnim.SetBool("isRun", false);
    }

}

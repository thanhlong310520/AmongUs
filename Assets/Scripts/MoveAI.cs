using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAI : MonoBehaviour
{
    [SerializeField] Transform target;
    NavMeshAgent agent;
    Animator botAnim;
    bool isRunning = true;
    private void Awake()
    {
        ChangerTarget();
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        if (!isRunning) return;
        if ((target.position.x - this.transform.position.x) < 0.1f && (target.position.y - this.transform.position.y) < 0.1f)
        {
            isRunning = false;
            StartCoroutine(UpdateTask());
        }
        else
        {
            agent.SetDestination(target.position);
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
        int i = Random.Range(0, 10);
        target = GameManager.Instance.AllTranform[i];
    }




}

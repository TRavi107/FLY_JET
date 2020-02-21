using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NinjaBoyController:MonoBehaviour, IPooledObj
{
    public State currentState;
    public State remainState;
    public Transform eyePos;
    public Transform throwPos;
    public float speed;
    public Transform destination;
    public List<Transform> Limit;
    public int roamIndex;
    public Animator myanimator;
    public bool looking;
    public bool continueChase;
    public float now,lastThrown,throwInterval;

    void IStart()
    {
        roamIndex = 0;
        Limit = new List<Transform>
        {
            GetComponentInParent<FJ.PlatController>().myEdge_left,
            GetComponentInParent<FJ.PlatController>().myEdge_right
        };
        Limit[0].position = new Vector2(Limit[0].position.x, transform.position.y);
        Limit[1].position = new Vector2(Limit[1].position.x, transform.position.y);
        destination = Limit[Random.Range(0, Limit.Count)];
    }

    private void Update()
    {
        now = Time.time;
        currentState.Execute(this);
    }

    public void Chase()
    {
        StopCoroutine("LookAround");
        looking = false;
        if (continueChase)
        {
            if (Vector2.Distance(transform.position, destination.position) >= 1)
            {
                myanimator.SetBool("run", true);
                this.transform.position = Vector2.MoveTowards(this.transform.position, destination.position, speed * Time.deltaTime);
                if (transform.position.x > destination.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);

                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            else
            {
                myanimator.SetBool("run", false);
                myanimator.SetBool("attack", true);
            }
        }
    }

    public void Patrol()
    {
        if (Vector2.Distance(transform.position, destination.position) >= 0.000001)
        {
            myanimator.SetBool("run", true);
            this.transform.position = Vector2.MoveTowards(this.transform.position, destination.position, speed * Time.deltaTime);
            if (transform.position.x > destination.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);

            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

        else
        {
            myanimator.SetBool("run", false);
            if (!looking)
            {
                looking = true;
                StartCoroutine("LookAround");
            }
        }
    }
    IEnumerator LookAround()
    {
        yield return new WaitForSeconds(2);
        int lookCount = Random.Range(1, 2);
        for (int i = 0; i <= lookCount; i++)
        {
            transform.Rotate(0, 180, 0);
            yield return new WaitForSeconds(2);
        }
        roamIndex = (roamIndex + 1) % Limit.Count;
        destination = Limit[roamIndex];
        looking = false;
    }

    public void ChangeState(State nextState)
    {
        if (nextState != remainState)
        {
            ////currentState.OnStateEnd(this);
            currentState = nextState;
            //currentState.OnStateStart(this);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Hero"))
        {
            //StartCoroutine("ShrinkAnimation");
        }
    }
    IEnumerator ShrinkAnimation()
    {
        currentState = remainState;
        while (transform.localScale.y >= 0.2)
        {
            transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y-Time.deltaTime/4);
            yield return null;
        }
        this.gameObject.SetActive(false);
        FJ.GameManager.singleton.AddScore(20);
    }

    public void PooledObjStart()
    {
        IStart();
    }
}

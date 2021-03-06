using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Patrol : MonoBehaviour
{
    public float speed;
    private float waitTime;
    public float startWaitTime;
    public float smooth = 1f;
    public bool arrived = true;

    public GameObject FrontIdle;
    public GameObject BackIdle;
    public GameObject FrontWalking;
    public GameObject BackWalking;

    private Quaternion targetRotation;

    public Transform[] moveSpots;
    private int randomSpot;

    void Start()
    {
        waitTime = startWaitTime;
        randomSpot = Random.Range(0, moveSpots.Length);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);
        Moving();
        if (Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
        {
            Idle();
            if (waitTime <= 0)
            {
                Turn();
                randomSpot = Random.Range(0, moveSpots.Length);
                waitTime = startWaitTime;
                arrived = true;
            }
            else
            {
                waitTime -= Time.deltaTime;
                arrived = false;

            }
        }

        Vector3 dir = moveSpots[randomSpot].position - transform.position;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);


        if (Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                if (randomSpot < moveSpots.Length - 1)
                {
                    randomSpot++;
                    waitTime = startWaitTime;
                }


                else
                {
                    randomSpot = 0;
                }
            }

            else
            {
                waitTime -= Time.deltaTime;
            }
        }

    }

    public void Turn()
    {
        targetRotation = Quaternion.LookRotation(-transform.forward, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smooth * Time.deltaTime);
    }

    void Moving()
    {
        FrontIdle.SetActive(false);
        BackIdle.SetActive(false);
        FrontWalking.SetActive(true);
        BackWalking.SetActive(true);
    }

    void Idle()
    {
        FrontIdle.SetActive(true);
        BackIdle.SetActive(true);
        FrontWalking.SetActive(false);
        BackWalking.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePingPong : MonoBehaviour
{
    public float MoveSpeed = 0.5f;
    public bool canWait;
    public float waitTime;
    private Vector3 InitialScale;
    private Vector3 TargetScale;
    private bool isUp, canScale;

    // Start is called before the first frame update
    void Start()
    {
        canScale = true;
        InitialScale = TargetScale= transform.localScale;
        TargetScale *= 0.9f; 
    }
    IEnumerator ScaleWait()
    {
        yield return new WaitForSecondsRealtime(waitTime);
        canScale = true;
        canWait = !canWait;
    }

    // Update is called once per frame
    void Update()
    {
        if (canScale)
        {
            if (!isUp)
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, TargetScale, MoveSpeed * Time.deltaTime);
                if (transform.localScale == TargetScale)
                {
                    isUp = true;
                }
            }
            else
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, InitialScale, MoveSpeed * Time.deltaTime);
                if (transform.localScale == InitialScale)
                {
                    isUp = false;
                    if (canWait)
                    {
                        canScale = false;
                    }                  
                }
            }
        }
        else if (canWait)
        {
            canWait = !canWait;
            StartCoroutine(ScaleWait());
        }

    }
}

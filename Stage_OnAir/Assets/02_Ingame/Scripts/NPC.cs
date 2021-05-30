using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public int Code = 0;
    public int Distance = 0; // -1  = 왼쪽, 1 = 오른쪽
    public float workTime = 0f;
    public float waitTime = 0f;
    public float workDistance = 0f;

    public Animator Anim;
    public IEnumerator Crt;

    void Update()
    {
        if(Distance == 0)
        {
            Distance = Random.Range(-1, 2);
            workTime = Random.Range(3, 6);
            waitTime = Random.Range(5, 8);
            workDistance= Random.Range(100, 801);

            Crt = Wait(waitTime);

            if (transform.localPosition.x + (workDistance * Distance) > 800)
                workDistance = 800 - transform.localPosition.x;
            if (transform.localPosition.x + (workDistance * Distance) < -800)
                workDistance = 800 - (transform.localPosition.x * -1);

            transform.localRotation = Quaternion.Euler(0, (Distance == 1)? 180 : 0, 0);
            if (Distance != 0)
            {
                Anim.SetBool("IsWork", true);
                transform.DOLocalMoveX(transform.localPosition.x + (workDistance * Distance), workTime)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => { StartCoroutine(Crt); });
            }
        }
    }
    IEnumerator Wait(float time)
    {
        Anim.SetBool("IsWork", false);
        yield return new WaitForSeconds(time);
        Distance = 0;
    }

    public void Stop()
    {
        StopCoroutine(Crt);
    }
}

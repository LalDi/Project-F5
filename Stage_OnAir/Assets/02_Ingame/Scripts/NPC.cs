using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NPC : MonoBehaviour
{
    public int Distance = 0; // -1  = 왼쪽, 1 = 오른쪽
    public float workTime = 0f;
    public float waitTime = 0f;
    public float workDistance = 0f;


    void Update()
    {
        if(Distance == 0)
        {
            Distance = Random.Range(-1, 2);
            workTime = Random.Range(3, 6);
            waitTime = Random.Range(5, 8);
            workDistance= Random.Range(100, 801);

            if (transform.localPosition.x + (workDistance * Distance) > 800)
                workDistance = 800 - transform.localPosition.x;
            if (transform.localPosition.x + (workDistance * Distance) < -800)
                workDistance = 800 - (transform.localPosition.x * -1);

            transform.localRotation = Quaternion.Euler(0, (Distance == 1)? 180 : 0, 0);
            if (Distance != 0)
            {
                transform.DOLocalMoveX(transform.localPosition.x + (workDistance * Distance), workTime)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => { StartCoroutine(Wait(waitTime)); });
            }
        }
    }
    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        Distance = 0;
    }
}

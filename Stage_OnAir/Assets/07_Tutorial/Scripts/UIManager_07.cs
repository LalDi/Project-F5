using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager_07 : MonoBehaviour
{
    //Main Menu
    public bool IsOpen = false;
    public GameObject MainBT;
    public GameObject[] Icons = new GameObject[3];

    public GameObject Stat_UI;

    public void Setting_Anim()
    {
        SoundManager.Instance.PlaySound("Pop_6");
        if (IsOpen)
        {
            MainBT.transform.DORotate(Vector3.forward * 360f, 0.5f).SetEase(Ease.OutBack);
            for (int i = 0; i < Icons.Length; i++)
            {
                Icons[i].transform.DOMoveY(transform.position.y, 0.5f).SetEase(Ease.OutBack);
                Icons[i].GetComponent<Image>().DOFade(0, 0.2f);
            }
        }
        else
        {
            MainBT.transform.DORotate(Vector3.forward * 180f, 0.5f).SetEase(Ease.OutBack);
            for (int i = 0; i < Icons.Length; i++)
            {
                Icons[i].transform.DOMoveY(transform.position.y + (-300 * (i + 1)), 0.5f).SetEase(Ease.OutBack);
                Icons[i].GetComponent<Image>().DOFade(1, 0.2f);
            }
        }

        IsOpen = !IsOpen;
    }

    public void Stat_UI_Anim()
    {
        SoundManager.Instance.PlaySound("Pop_6");
        if (Stat_UI.transform.position.x >= -700)
            Stat_UI.transform.DOLocalMoveX(-720f, 0.3f).SetEase(Ease.OutBack);
        else
            Stat_UI.transform.DOLocalMoveX(-290f, 0.3f).SetEase(Ease.OutBack);
    }

}

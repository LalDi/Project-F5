using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingMenu : MonoBehaviour
{
    public bool IsOpen = false;
    public GameObject MainBT;
    public GameObject[] Icons = new GameObject[3];
    
    public void Anim()
    {
        if (IsOpen) {
            MainBT.transform.DORotate(Vector3.forward * 360f, 0.5f).SetEase(Ease.OutBack);
            for (int i = 0; i < Icons.Length; i++)
            {
                Icons[i].transform.DOMoveY(transform.position.y, 0.5f).SetEase(Ease.OutBack);
                Icons[i].GetComponent<Image>().DOFade(0, 0.2f);
                Icons[i].GetComponent<Button>().enabled = false;
            }
        }
        else {
            MainBT.transform.DORotate(Vector3.forward * 180f, 0.5f).SetEase(Ease.OutBack);
            for (int i = 0; i < Icons.Length; i++)
            {
                Icons[i].transform.DOMoveY(transform.position.y + (-300 * (i + 1)), 0.5f).SetEase(Ease.OutBack);
                Icons[i].GetComponent<Image>().DOFade(1, 0.2f);
                Icons[i].GetComponent<Button>().enabled = true;
            }
        }

        IsOpen = !IsOpen;
    }
}
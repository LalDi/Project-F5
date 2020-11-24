using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingMenu : MonoBehaviour
{
    public bool IsOpen = false;
    public GameObject[] Icons = new GameObject[3];
    void Start()
    {
        for (int i = 0; i < Icons.Length; i++) {
            Icons[i] = transform.GetChild(i).gameObject;
            Icons[i].GetComponent<Button>().enabled = false;
        }
    }

    public void Anim()
    {
        if (IsOpen) CloseAnim();
        else OpenAnim();

        IsOpen = !IsOpen;
    }

    void OpenAnim()
    {
        for (int i = 0; i < Icons.Length; i++)
        {
            Icons[i].transform.DOMoveY(transform.position.x + 300 * (i-1), 0.5f).SetEase(Ease.OutBack);
            Icons[i].GetComponent<Image>().DOFade(1, 0.5f);
            Icons[i].GetComponent<Button>().enabled = true;
        }
    }
    void CloseAnim()
    {
        for (int i = 0; i < Icons.Length; i++)
        {
            Icons[i].transform.DOMoveY(1300, 0.5f).SetEase(Ease.OutBack);
            Icons[i].GetComponent<Image>().DOFade(0, 0.2f);
            Icons[i].GetComponent<Button>().enabled = false;
        }
    }
}

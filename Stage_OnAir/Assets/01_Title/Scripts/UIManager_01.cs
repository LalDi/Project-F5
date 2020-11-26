using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager_01 : MonoBehaviour
{
    public GameObject ttsText;

    public GameObject Popup_Black;
    public GameObject Popup_LogIn;
    public GameObject Popup_SignUp;

    TouchScreenKeyboard Keyboard;

    void Start()
    {
        //ttsText.transform.DOScaleX(1.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.OutQuad);
        Popup_Black.SetActive(false);
        Popup_LogIn.SetActive(false);
        Popup_SignUp.SetActive(false);
    }

    public void To_Ingame()
    {
        LoadManager.LoaderCallback();
        LoadManager.Load(LoadManager.Scene.Ingame);
    }

    public void OpenPopup_LogIn()
    {
        ClosePopup();
        Popup_Black.SetActive(true);
        Popup_LogIn.SetActive(true);
    }
    public void OpenPopup_SignUp()
    {
        ClosePopup();
        Popup_Black.SetActive(true);
        Popup_SignUp.SetActive(true);
    }
    public void ClosePopup()
    {
        Popup_Black.SetActive(false);
        Popup_LogIn.SetActive(false);
        Popup_SignUp.SetActive(false);
    }
}

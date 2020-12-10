using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class UIManager_01 : MonoBehaviour
{
    public GameObject Popup_Black;
    public GameObject Popup_LogIn;
    public GameObject Popup_SignUp;

    void Start()
    {
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

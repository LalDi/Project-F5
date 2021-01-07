using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        SoundManager.Instance.PlaySound("Prize_Wheel_Spin_2_Reward");
        Backend.Chart.GetAllChartAndSave(true);
        ScenarioData.Instance.SetScenarioData();
        ActorData.Instance.SetActorsData();
        Items.Instance.SetStaffData();

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
        SoundManager.Instance.PlaySound("Pop_6");
        Popup_Black.SetActive(false);
        Popup_LogIn.SetActive(false);
        Popup_SignUp.SetActive(false);
    }
}

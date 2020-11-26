using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_03 : MonoBehaviour
{
    public GameObject Popup_Balck;
    public GameObject Popup_Result;

    void Start()
    {
        Popup_Balck.SetActive(false);
        Popup_Result.SetActive(false);
    }
    public void To_Ingame()
    {
        LoadManager.LoaderCallback();
        LoadManager.Load(LoadManager.Scene.Ingame);
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_Quit : MonoBehaviour
{
    [SerializeField] private GameObject Popup_Quit;

    private bool isOnPopup;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Popup_Quit.SetActive(!isOnPopup);
            isOnPopup = !isOnPopup;
        }
    }

    public void QuitApplication()
    {
        Application.CancelQuit();

#if !UNITY_EDTIOR
        System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
    }

    public void QuitPopup()
    {
        Popup_Quit.SetActive(false);
        isOnPopup = false;
    }
}

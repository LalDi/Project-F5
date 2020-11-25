using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager_06 : MonoBehaviour
{
    public GameObject Popup_Illust;

    void Start()
    {
        Popup_Illust.SetActive(false);
    }

    public void Push_IllustBT()
    {
        Popup_Illust.GetComponent<Image>().sprite =
            EventSystem.current.currentSelectedGameObject.transform.GetChild(1).GetComponent<Image>().sprite;
        Popup_Illust.SetActive(true);
    }
    public void Exit_IllustPopup()
    {
        Popup_Illust.SetActive(false);
    }
}

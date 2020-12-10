using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager_06 : MonoBehaviour
{
    public GameObject Popup_Black;
    public GameObject Popup_Illust;

    public GameObject Scroll;

    public Sprite Null;
    public List<Sprite> Illusts = new List<Sprite>();

    void Start()
    {
        Scroll = GameObject.Find("Scroll Rect Image").gameObject;
        for (int i = 0; i < ScenarioData.Instance.ScenarioList.Count; i++)
        {
            if (GameManager.Instance.ScenarioIllust[i])
                Scroll.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite =
                    Illusts[i];
            else
                Scroll.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite =
                    Null;
            Scroll.transform.GetChild(i).GetChild(0).GetComponent<Text>().text =
                ScenarioData.Instance.ScenarioList[i].Name;
            Scroll.transform.GetChild(i).gameObject.SetActive(true);
        }
        Scroll.GetComponent<RectTransform>().sizeDelta =
            new Vector2(911.0076f, ScenarioData.Instance.ScenarioList.Count * 610 + 100);

        Popup_Black.SetActive(false);
        Popup_Illust.SetActive(false);
    }

    public void Push_IllustBT()
    {
        Popup_Illust.GetComponent<Image>().sprite =
            EventSystem.current.currentSelectedGameObject.transform.GetChild(1).GetComponent<Image>().sprite;
        Popup_Black.SetActive(true);
        Popup_Illust.SetActive(true);
    }
    public void Exit_IllustPopup()
    {
        Popup_Black.SetActive(false);
        Popup_Illust.SetActive(false);
    }
    public void To_Ingame()
    {
        LoadManager.LoaderCallback();
        LoadManager.Load(LoadManager.Scene.Ingame);
    }
}

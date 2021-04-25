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
        for (int i = 0; i < ScenarioData.Instance.ScenarioList.Count; i++)
        {
            GameObject item = ObjManager.SpawnPool("Illust", Vector3.zero, Quaternion.Euler(0, 0, 0));

            if (GameManager.Instance.ScenarioIllust[i])
                item.transform.GetChild(1).GetComponent<Image>().sprite = Illusts[i];
            else
                item.transform.GetChild(1).GetComponent<Image>().sprite = Null;
            item.transform.GetChild(0).GetComponent<Text>().text =
                ScenarioData.Instance.ScenarioList[i].Name;

            int j = i;
            item.transform.GetComponent<Button>().onClick.AddListener(() => Push_IllustBT(j));
        }
        Scroll.GetComponent<RectTransform>().sizeDelta =
            new Vector2(911.0076f, ScenarioData.Instance.ScenarioList.Count * 610 + 100);

        Popup_Black.SetActive(false);
        Popup_Illust.SetActive(false);
    }

    public void Push_IllustBT(int num)
    {
        SoundManager.Instance.PlaySound("Pop_6");
        Popup_Illust.GetComponent<Image>().sprite = Illusts[num];
        Popup_Black.SetActive(true);
        Popup_Illust.SetActive(true);
    }
    public void Exit_IllustPopup()
    {
        SoundManager.Instance.PlaySound("Pop_3");
        Popup_Black.SetActive(false);
        Popup_Illust.SetActive(false);
    }
    public void To_Ingame()
    {
        SoundManager.Instance.PlaySound("Pop_6");
        LoadManager.LoaderCallback();
        LoadManager.Load(LoadManager.Scene.Ingame);
    }
}

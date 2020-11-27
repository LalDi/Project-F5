using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_03 : MonoBehaviour
{
    public GameObject ScenarioPrefab;

    public GameObject Popup_Black;
    public GameObject Popup_Scenario_Select;
    public GameObject Popup_Warning;
    public GameObject Popup_Buy_Checking;

    public GameObject Scroll;

    void Start()
    {
        Popup_Black.SetActive(false);
        Popup_Scenario_Select.SetActive(false);
        Popup_Warning.SetActive(false);
        Popup_Buy_Checking.SetActive(false);
    }
    void Update()
    {
        if (ScenarioData.Instance.ScenarioList.Count == 0)
        {
            ScenarioData.Instance.SetScenarioData();
            for (int i = 0; i < ScenarioData.Instance.ScenarioList.Count; i++)
                {
                    Scroll.transform.GetChild(i).parent = Scroll.transform.GetChild(0);
                    Scroll.transform.GetChild(i).GetChild(0).GetComponent<Text>().text =
                        ScenarioData.Instance.ScenarioList[i].Name;
                    Scroll.transform.GetChild(i).gameObject.SetActive(true);
                }
        }
    }

    public void Popup_Scenario(GameObject DataObj)
    {
        Popup_Scenario_Select.transform.Find("Text").GetComponent<Text>().text =
            DataObj.transform.GetComponent<ScenarioScript>().ScenarioData.Name;
        Popup_Scenario_Select.transform.Find("Quality Image").GetChild(0).GetComponent<Text>().text =
            "연출력" + DataObj.transform.GetComponent<ScenarioScript>().ScenarioData.Quality;
        Popup_Scenario_Select.transform.Find("need Actor Image").GetChild(0).GetComponent<Text>().text =
            "필요 배우" + DataObj.transform.GetComponent<ScenarioScript>().ScenarioData.Actors;

        Popup_Black.SetActive(true);
        Popup_Scenario_Select.SetActive(true);
    }
    public void Popup_ScenarioBuy(GameObject DataObj)
    {
        Popup_Black.SetActive(true);
        Popup_Scenario_Select.SetActive(false);
        if (GameManager.Instance.Money >= 
            DataObj.transform.parent.GetComponent<ScenarioScript>().ScenarioData.Price)//현 금액이 필요 금액 보다 많으면
        {
            GameManager.Instance.CostMoney(
                DataObj.transform.parent.GetComponent<ScenarioScript>().ScenarioData.Price, true);
            Popup_Buy_Checking.SetActive(true);
        }
        else
        {
            Popup_Warning.SetActive(true);
        }
    }
    public void To_Ingame()
    {
        LoadManager.LoaderCallback();
        LoadManager.Load(LoadManager.Scene.Ingame);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_03 : MonoBehaviour
{
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
                Scroll.transform.GetChild(i).GetChild(0).GetComponent<Text>().text =
                    ScenarioData.Instance.ScenarioList[i].Name;
                Scroll.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public void Popup_Scenario(GameObject DataObj)
    {
        Scenario Data = DataObj.transform.GetComponent<ScenarioScript>().ScenarioData;

        Popup_Scenario_Select.transform.Find("Text").GetComponent<Text>().text = Data.Name;
        Popup_Scenario_Select.transform.Find("Quality Image").GetChild(0).GetComponent<Text>().text =
            "연출력" + Data.Quality;
        Popup_Scenario_Select.transform.Find("need Actor Image").GetChild(0).GetComponent<Text>().text =
            "필요 배우" + Data.Actors;
        Popup_Scenario_Select.transform.Find("Upgrade BT").Find("Pay Text").GetComponent<Text>().text =
            "가격 " + Data.Price.ToString("N0");

        Popup_Black.SetActive(true);
        Popup_Scenario_Select.SetActive(true);
    }
    public void Popup_ScenarioBuy(GameObject DataObj)
    {
        Scenario Data = DataObj.transform.parent.GetComponent<ScenarioScript>().ScenarioData;

        Popup_Black.SetActive(true);
        Popup_Scenario_Select.SetActive(false);

        if (GameManager.Instance.Money >=
            Data.Price)
        {
            Popup_Buy_Checking.transform.GetChild(1).GetComponent<Text>().text =
               " 『" + Data.Name + "』 \n을 구매하였습니다.";
            Popup_Buy_Checking.transform.GetChild(2).GetComponent<Text>().text =
                "보유금액 : " + GameManager.Instance.Money.ToString("N0") + " -> " +
                (GameManager.Instance.Money - Data.Price).ToString("N0");
            GameManager.Instance.SetScenario(Data);
            GameManager.Instance.CostMoney(
                Data.Price, true);
            Popup_Buy_Checking.SetActive(true);
        }
        else
            Popup_Warning.SetActive(true);
    }
    public void Close_Popup()
    {
        Popup_Black.SetActive(false);
        Popup_Scenario_Select.SetActive(false);
        Popup_Warning.SetActive(false);
        Popup_Buy_Checking.SetActive(false);
    }
    public void Buy_Scenario()
    {
        GameManager.Instance.SetStep(GameManager.Step.Cast_Actor);
        LoadManager.LoaderCallback();
        LoadManager.Load(LoadManager.Scene.Ingame);
    }
    public void To_Ingame()
    {
        LoadManager.LoaderCallback();
        LoadManager.Load(LoadManager.Scene.Ingame);
    }
}

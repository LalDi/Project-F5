﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager_03 : MonoBehaviour
{
    Scenario Data;

    public Text Text_Money;

    public GameObject Popup_Black;
    public GameObject Popup_Scenario_Select;
    public GameObject Popup_Buy_Checking;
    public GameObject Popup_LoansCk;

    public GameObject Scroll;

    void Awake()
    {
        GameObject Popups = GameObject.Find("Popup").gameObject;

        Popup_Black = Popups.transform.Find("Black BG").gameObject;
        Popup_Scenario_Select = Popups.transform.Find("Scenario Select PU").gameObject;
        Popup_Buy_Checking = Popups.transform.Find("Buy Checking PU").gameObject;
        Scroll = GameObject.Find("Scroll Rect Image").gameObject;

        Popup_Black.SetActive(false);
        Popup_Scenario_Select.SetActive(false);
        Popup_Buy_Checking.SetActive(false);
    }

    void Start()
    {
        for (int i = 0; i < ScenarioData.Instance.ScenarioList.Count; i++)
        {
            GameObject scenario = ObjManager.SpawnPool("Scenario", Vector3.zero, Quaternion.Euler(0, 0, 0));

            int j = i;
            scenario.transform.GetComponent<Button>().onClick.AddListener(() => { Popup_Scenario(j); });

            scenario.transform.GetChild(0).GetComponent<Text>().text =
                ScenarioData.Instance.ScenarioList[i].Name;
            scenario.transform.gameObject.SetActive(true);
        }
        Scroll.GetComponent<RectTransform>().sizeDelta =
            new Vector2(911.0076f, ScenarioData.Instance.ScenarioList.Count * 250);
    }

    void Update()
    {
        Text_Money.text = GameManager.Instance.Money.ToString("N0");
        if (GameManager.Instance.Money <= 0)
            Text_Money.color = Color.red;
        else
            Text_Money.color = Color.black;
    }

    public void Popup_Scenario(int num)
    {
        SoundManager.Instance.PlaySound("Pop_6");
        Data = ScenarioData.Instance.ScenarioList[num];

        Popup_Scenario_Select.transform.Find("Text").GetComponent<Text>().text = Data.Name;
        Popup_Scenario_Select.transform.Find("Quality Image").GetChild(0).GetComponent<Text>().text =
            "연출력 : " + Data.Quality;
        Popup_Scenario_Select.transform.Find("need Actor Image").GetChild(0).GetComponent<Text>().text =
            "필요 배우 : " + Data.Actors;
        Popup_Scenario_Select.transform.Find("Buy BT").Find("Pay Text").GetComponent<Text>().text =
            "가격 " + Data.Price.ToString("N0");

        Popup_Black.SetActive(true);
        Popup_Scenario_Select.SetActive(true);
    }

    public void Popup_Loans()
    {
        SoundManager.Instance.PlaySound("Pop_6");
        if (GameManager.Instance.Money < Data.Price)
        {
            Popup_LoansCk.SetActive(true);

            Popup_LoansCk.transform.GetChild(2).GetComponent<Text>().text = "필요금액: " +
                ((GameManager.Instance.Money <= 0) ?
                         Data.Price.ToString("N0")
                        : ((GameManager.Instance.Money - Data.Price) * -1).ToString("N0"));
        }
        else Popup_ScenarioBuy();
    }

    public void Popup_ScenarioBuy()
    {
        SoundManager.Instance.PlaySound("Cash_Register");
        Popup_Buy_Checking.transform.GetChild(1).GetComponent<Text>().text =
           " 『" + Data.Name + "』 \n을 구매하였습니다.";
        Popup_Buy_Checking.transform.GetChild(2).GetComponent<Text>().text =
            "보유금액 : " + GameManager.Instance.Money.ToString("N0") + " -> " +
            (GameManager.Instance.Money - Data.Price).ToString("N0");
        GameManager.Instance.SetScenario(Data);
        Popup_Buy_Checking.SetActive(true);
    }

    public void Close_Popup()
    {
        SoundManager.Instance.PlaySound("Pop_3");
        Popup_Black.SetActive(false);
        Popup_Scenario_Select.SetActive(false);
        Popup_Buy_Checking.SetActive(false);
        Popup_LoansCk.SetActive(false);
    }

    public void To_Ingame()
    {
        SoundManager.Instance.PlaySound("Prize_Wheel_Spin_2_Reward");
        LoadManager.LoaderCallback();
        LoadManager.Load(LoadManager.Scene.Ingame);
    }
}

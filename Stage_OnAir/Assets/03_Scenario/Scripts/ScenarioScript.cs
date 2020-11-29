using BackEnd.Game.Payment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioScript : MonoBehaviour
{
    public Scenario ScenarioData;

    void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(()=> {
            GameObject.Find("UIManager").GetComponent<UIManager_03>().Popup_Scenario(); });
    }
}

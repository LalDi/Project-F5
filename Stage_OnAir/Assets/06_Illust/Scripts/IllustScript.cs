using BackEnd.Game.Payment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IllustScript : MonoBehaviour
{
    void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(() => {
            GameObject.Find("UIManager").GetComponent<UIManager_06>().Push_IllustBT();
        });
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.SaveData();
        LoadManager.LoaderCallback();
    }
}
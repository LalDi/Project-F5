using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour
{
    void Start()
    {
        LoadManager.LoaderCallback();
        Debug.Log("귀찮아");
    }
}
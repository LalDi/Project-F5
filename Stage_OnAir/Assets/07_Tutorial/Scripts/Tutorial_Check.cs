using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tutorial_Check : Tutorial
{
    public GameObject ClickObj;

    public override void CheckIfHappening()
    {
        if (EventSystem.current.currentSelectedGameObject == ClickObj)
        {
            Tutorial_Manager.Instance.CompletedTutorial();
        }
    }
}

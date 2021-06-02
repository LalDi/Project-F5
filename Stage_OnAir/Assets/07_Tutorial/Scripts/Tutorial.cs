using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public int Order;

    [TextArea(3, 10)]
    public string Explanation;
    public Vector3 ExpPosition;

    private void Awake()
    {
        Tutorial_Manager.Instance.Tutorials.Add(this);
    }

    public virtual void CheckIfHappening() { }
}

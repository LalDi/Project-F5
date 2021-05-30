using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Data : MonoBehaviour
{
    public enum Step { Select_Scenario, Cast_Actor, Set_Period, Prepare_Play, Start_Play };
    public Step NowStep;

    public int Money;
}

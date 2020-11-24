using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // 연극의 3가지 점수 요소
    public float Play_Quality { get; private set; }
    public float Play_Marketing { get; private set; }
    public float Play_Success { get; private set; }

    // 연극의 퀄리티 점수를 결정하는 3가지 수치
    public float Quality_Acting { get; private set; }
    public float Quality_Scenario { get; private set; }
    public float Quality_Direction { get; private set; }

    // 현재 보유 금액
    public int Money { get; private set; }

    // 현재 날짜
    public int Year { get; private set; }
    public int Month { get; private set; }

    // 설정된 연극 준비 기간
    public int Period { get; private set; }

    // 배우의 수
    public int NowActor { get; private set; }
    public int MaxActor { get; private set; }

    //public List<ActorData.Actor> Actors;
    //public Dictionary<string, StaffData> Staffs = new Dictionary<string, StaffData>();

    public enum Step { Select_Scenario, Cast_Actor, Set_Period, Prepare_Play, Start_Play };
    public Step NowStep;



    private void Awake()
    {
        Screen.SetResolution(Screen.width, Screen.width * 9 / 16, true);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 144;
    }

    public void Reset()
    {
        Play_Quality = 0;
        Play_Marketing = 0;
        Play_Success = 0;

        Quality_Acting = 0;
        Quality_Scenario = 0;
        Quality_Direction = 0;

        Period = 0;

        NowActor = 0;
        MaxActor = 0;

        //Actors.Clear();
        NowStep = Step.Select_Scenario;
    }

    public void GoNextMonth()
    {
        Month++;
        if (Month == 13)
        {
            Year++;
            Month = 1;
            // 스태프 연봉 지급하는 코드
        }
    }

}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // 플레이어 데이터
    private string NickName;
    public int DefaultSuccess;

    //public List<ActorData.Actor> Actors;
    //public Dictionary<string, StaffData> Staffs = new Dictionary<string, StaffData>();

    public enum Step { Select_Scenario, Cast_Actor, Set_Period, Prepare_Play, Start_Play };
    public Step NowStep { get; private set; }

    public Button Btn_Play;


    private void Awake()
    {
        Screen.SetResolution(Screen.width, Screen.width * 9 / 16, true);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 144;

        Year = 2000;
        Month = 01;

        Money = 5000000;

        DefaultSuccess = 70;

        Btn_Play = GameObject.Find("Play BT").GetComponent<Button>();
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
        Btn_Play.interactable = false;
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

    public void SetStep(Step NextStep)
    {
        NowStep = NextStep;
    }

    public void SetDefaultPeriod()
    {
        float Success;
        for (int i = 0; i < MaxActor; i++)
        {
            // 모든 Actor의 경험의 평균치를 구하여, 이를 Success에 할당한다.
            // 이 수치는 6개월을 기준으로, 기간이 성공률에 개입하지 않은 상태의 성공률을 의미한다.
            // 이제, 이 수치가 DefaultSuccess에 가장 가깝게 조절한다. (성공률은 DefaultSuccess보다 항상 크게 책정된다.)
            // 1개월을 기준으로 변화치는 Success의 10%이다.
        }
    }
}
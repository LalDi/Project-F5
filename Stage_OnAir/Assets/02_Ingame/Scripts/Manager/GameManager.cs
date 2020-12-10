using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Define;
using BackEnd;
using LitJson;

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
    public bool OnBGM;
    public bool OnSFX;
    public bool OnPush;

    // 랭킹

    //시나리오
    public Scenario NowScenario { get; private set; }

    // 오디션에서 고용한 배우들
    public List<Actor> Actors = new List<Actor>();
    //public Dictionary<string, StaffData> Staffs = new Dictionary<string, StaffData>();

    public enum Step { Select_Scenario, Cast_Actor, Set_Period, Prepare_Play, Start_Play };
    public Step NowStep { get; private set; }

    private void Awake()
    {
        Screen.SetResolution(Screen.width, Screen.width * 16 / 9, true);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 144;

        Backend.Initialize(() =>
        {
            // 초기화 성공한 경우 실행
            if (Backend.IsInitialized)
            {
                Debug.Log("초기화 완료");

                // 게임 디버깅 및 테스트를 위한 임시 로그인
                var data = Backend.BMember.CustomLogin("jungjh0513", "1234");
                Debug.Log("로그인 완료");

            }
            // 초기화 실패한 경우 실행
            else
            {

            }
        });
        //Backend.Chart.GetAllChartAndSave(true);

        Year = 2000;
        Month = 01;

        Money = 5000000;

        DefaultSuccess = 60;
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

    public void SetStep(Step NextStep)
    {
        NowStep = NextStep;
    }

    public void CostMoney(int value, bool Reduction = true)
    {
        Money = Reduction ? Money - value : Money + value;
    }

    public void SetDefaultPeriod()
    {
        float Success = 0;
        int Count = 0;

        foreach (var item in Actors)
        {
            Success += item.Experience;
            Count++;
        }
        Success = Success / Count;

        Count = -5;
        float temp = Success * 0.1f;
        Success -= temp * 5;

        while (Success < DefaultSuccess)
        {
            Success += temp;
            Count++;
        }

        Period = Count + 6;
    }

    public float GetSuccess()
    {
        float Success = 0;
        int Count = 0;

        foreach (var item in Actors)
        {
            Success += item.Experience;
            Count++;
        }
        Success = Success / Count;

        float temp = Success * 0.1f;

        return Success + (temp * (Period - 6));
    }
    public void SetScenario(Scenario NextScenario)
    {
        NowScenario = NextScenario;
    }

    public void SetPeriod()
    {
        Play_Success = GetSuccess();
        SetStep(Step.Prepare_Play);
    }

    public void SetPeriod(int value)
    {
        Period += value;
    }

    public void PlusNowActor()
    {
        NowActor++;
    }
}
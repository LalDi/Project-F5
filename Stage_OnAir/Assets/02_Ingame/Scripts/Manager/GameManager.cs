using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;
using BackEnd;
using LitJson;
using System.Linq;

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
    public string NickName;
    public int DefaultSuccess;
    public bool OnBGM;
    public bool OnSFX;
    public bool OnPush;
    [SerializeField]
    public bool IsBankrupt;

    //시나리오
    public Scenario NowScenario { get; private set; }
    public bool[] ScenarioIllust = new bool[10];

    // 오디션에서 고용된 배우들
    public List<Actor> Actors = new List<Actor>();

    //스탭 레벨 저장
    public int[] StaffLevel = new int[10];
    // 스태프 정보 저장
    public List<Staff> Staffs = new List<Staff>();

    // 발전 정보 저장
    public List<Develop> Develops = new List<Develop>();

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

        OnBGM = true;
        OnSFX = true;
        OnPush = true;
        IsBankrupt = false;
    }
    public new void Start()
    {
        base.Start();

        LoadData();
    }

    public void Reset()
    {
        Play_Quality = 0;
        Play_Marketing = 100;
        Play_Success = 0;

        Quality_Acting = 0;
        Quality_Scenario = 0;
        Quality_Direction = 0;

        Period = 0;

        NowActor = 0;
        MaxActor = 0;

        Actors.Clear();
        ActorData.Instance.SetActorsData();

        Develops.Clear();
        Develops = DevelopData.Instance.GetDevelopRandom();

        NowStep = Step.Select_Scenario;
    }

    public void SaveData()
    {
        string InDate = Backend.BMember.GetUserInfo().GetInDate();

        Param param = new Param();
        param.Add("Money", Money);
        param.Add("Year", Year);
        param.Add("Month", Month);
        param.Add("DefaultSuccess", DefaultSuccess);

        var Info = Backend.GameSchemaInfo.Get("Player", InDate);
        string InfoInDate;

        if (Info.GetStatusCode() == "404")
        {
            Backend.GameSchemaInfo.Insert("Player", param); // 동기
            Debug.LogError("새로운 데이터 생성");
        }
        else
        {
            InfoInDate = Info.Rows()[0]["inDate"]["S"].ToString();
            Backend.GameSchemaInfo.Update("Player", InfoInDate, param); // 동기
            Debug.LogError("기존 데이터 갱신");
        }

        StaffData.SaveAllStaff();

        Debug.LogError("데이터 저장 완료");
    }

    public void LoadData()
    {
        string InDate = Backend.BMember.GetUserInfo().GetInDate();
        var Info = Backend.GameSchemaInfo.Get("Player", InDate);

        NickName = Backend.BMember.GetUserInfo().GetReturnValuetoJSON()["row"]["nickname"].ToString();

        if (Info.GetStatusCode() == "200")
        {
            BackendReturnObject contents = Backend.GameSchemaInfo.Get("Player", InDate);

            JsonData data = contents.Rows()[0];

            if (data.Keys.Contains("Money"))
            {
                Money = int.Parse(data["Money"]["N"].ToString());
            }
            if (data.Keys.Contains("Year"))
            {
                Year = int.Parse(data["Year"]["N"].ToString());
            }
            if (data.Keys.Contains("Month"))
            {
                Month = int.Parse(data["Month"]["N"].ToString());
            }
            if (data.Keys.Contains("DefaultSuccess"))
            {
                DefaultSuccess = int.Parse(data["DefaultSuccess"]["N"].ToString());
            }

            Debug.Log("기존 데이터 불러오기");
        }
        else
        {
            Param param = new Param();

            param.Add("Money", 5000000);
            param.Add("Year", 2000);
            param.Add("Month", 01);
            param.Add("DefaultSuccess", 70);

            param.Add("BestQuality", 0);
            param.Add("BestAudience", 0);
            param.Add("BestProfit", 0);

            Backend.GameSchemaInfo.Insert("Player", param); // 동기

            Money = 5000000;
            Year = 2000;
            Month = 1;
            DefaultSuccess = 70;
            
            Debug.Log("새 데이터 생성");
        }

        Backend.Chart.GetAllChartAndSave(true);

        Staffs = StaffData.Instance.SetStaffData();
        Debug.Log("스태프 데이터 생성");

        DevelopData.Instance.SetDevelopData();

        Develops.Clear();
        Develops = DevelopData.Instance.GetDevelopRandom();

        Debug.Log("발전 데이터 생성");
    }

    public void SetValue(MANAGERDATA.DATALIST data, float value, bool IsPlus = false)
    {
        if (IsPlus)
        {
            switch (data)
            {
                case MANAGERDATA.DATALIST.QUALITY:
                    Play_Quality += value;
                    break;
                case MANAGERDATA.DATALIST.MARKETING:
                    Play_Marketing += value;
                    break;
                case MANAGERDATA.DATALIST.SUCCESS:
                    Play_Success += value;
                    break;
                case MANAGERDATA.DATALIST.ACTING:
                    Quality_Acting += value;
                    break;
                case MANAGERDATA.DATALIST.SCENARIO:
                    Quality_Scenario += value;
                    break;
                case MANAGERDATA.DATALIST.DIRECTION:
                    Quality_Direction += value;
                    break;
            }
        }
        else
        {
            switch (data)
            {
                case MANAGERDATA.DATALIST.QUALITY:
                    Play_Quality = value;
                    break;
                case MANAGERDATA.DATALIST.MARKETING:
                    Play_Marketing = value;
                    break;
                case MANAGERDATA.DATALIST.SUCCESS:
                    Play_Success = value;
                    break;
                case MANAGERDATA.DATALIST.ACTING:
                    Quality_Acting = value;
                    break;
                case MANAGERDATA.DATALIST.SCENARIO:
                    Quality_Scenario = value;
                    break;
                case MANAGERDATA.DATALIST.DIRECTION:
                    Quality_Direction = value;
                    break;
            }
        }
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

    #region Play n Quality
    // 연극의 3가지 점수 요소
    public void Plus_Play_Quality(int value)
    {
        Play_Quality += value;
    }
    public void Plus_Play_Marketing(int value)
    {
        Play_Marketing += value;
    }
    public void Plus_Play_Success(int value)
    {
        Play_Success += value;
    }
    // 연극의 퀄리티 점수를 결정하는 3가지 수치
    public void Plus_Quality_Acting(int value)
    {
        Quality_Acting += value;
        Plus_Play_Quality(value);
    }
    public void Plus_Quality_Scenario(int value)
    {
        Quality_Scenario += value;
        Plus_Play_Quality(value);
    }
    public void Plus_Quality_Direction(int value)
    {
        Quality_Direction += value;
        Plus_Play_Quality(value);
    }
    #endregion

    #region Money n Day

    public void CostMoney(int value, bool Reduction = true)
    {
        Money = Reduction ? Money - value : Money + value;
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
    #endregion

    #region Player Data

    public void ReStart()
    {
        Reset();
        Year = 2000;
        Month = 01;
        Money = 5000000;
        DefaultSuccess = 70;
        IsBankrupt = false;
    }

    public void Is_Bankrupt(bool Is) 
    {
        IsBankrupt = Is;
    }
    #endregion

    #region Scenario n Actor n Staff

    public void SetScenario(Scenario NextScenario)
    {
        NowScenario = NextScenario;
        CostMoney(ScenarioData.Instance.FindScenario(NextScenario.Code).Price);
        SetMaxActor(ScenarioData.Instance.FindScenario(NextScenario.Code).Actors);
        Plus_Quality_Scenario(ScenarioData.Instance.FindScenario(NextScenario.Code).Quality);
        SetStep(Step.Cast_Actor);
    }

    public void SetMaxActor(int Num)
    {
        MaxActor = Num;
    }

    public void PlusNowActor()
    {
        NowActor++;
    }

    public void SetStep(Step NextStep)
    {
        NowStep = NextStep;
    }
    #endregion
}
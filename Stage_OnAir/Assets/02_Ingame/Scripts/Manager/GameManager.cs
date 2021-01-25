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
    public float Plus_Direction { get; private set; }

    // 현재 보유 금액
    public int Money { get; private set; }

    // 현재 날짜
    public int Month { get; private set; }
    public int Day { get; private set; }

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

    // 상점 데이터 구매 여부
    public bool Adblock { get; private set; }
    public bool StartPackage { get; private set; }

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
        Plus_Direction = 0;
        GetDirection();

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
        param.Add("Month", Month);
        param.Add("Day", Day);
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
        // 플레이어 InDate
        string InDate = Backend.BMember.GetUserInfo().GetInDate();

        // 서버에서 Player테이블의 데이터를 받아옴
        var Info = Backend.GameSchemaInfo.Get("Player", InDate);

        NickName = Backend.BMember.GetUserInfo().GetReturnValuetoJSON()["row"]["nickname"].ToString();

        // Player테이블의 데이터를 받아오는데 성공
        if (Info.GetStatusCode() == "200")
        {
            BackendReturnObject contents = Backend.GameSchemaInfo.Get("Player", InDate);

            JsonData data = contents.Rows()[0];

            if (data.Keys.Contains("Money"))
            {
                Money = int.Parse(data["Money"]["N"].ToString());
            }
            if (data.Keys.Contains("Month"))
            {
                Month = int.Parse(data["Month"]["N"].ToString());
            }
            if (data.Keys.Contains("Day"))
            {
                Day = int.Parse(data["Day"]["N"].ToString());
            }
            if (data.Keys.Contains("DefaultSuccess"))
            {
                DefaultSuccess = int.Parse(data["DefaultSuccess"]["N"].ToString());
            }

            Debug.Log("기존 데이터 불러오기");
        }
        // Player테이블의 데이터를 받아오는데 실패
        else
        {
            Param param = new Param();

            param.Add("Money", PLAYER.DEFAULT_MONEY);
            param.Add("Year", PLAYER.DEFAULT_YEAR);
            param.Add("Month", PLAYER.DEFAULT_MONTH);
            param.Add("DefaultSuccess", PLAYER.DEFAULT_SUCCESS);

            param.Add("BestQuality", 0);
            param.Add("BestAudience", 0);
            param.Add("BestProfit", 0);

            Backend.GameSchemaInfo.Insert("Player", param); // 동기

            Money = PLAYER.DEFAULT_MONEY;
            Year = PLAYER.DEFAULT_YEAR;
            Month = PLAYER.DEFAULT_MONTH;
            DefaultSuccess = PLAYER.DEFAULT_SUCCESS;
            
            Debug.Log("새 데이터 생성");
        }

        // 서버로부터 Shop테이블의 데이터를 받아옴
        Info = Backend.GameSchemaInfo.Get("Shop", InDate);

        // Shop테이블의 데이터를 받아오는데 성공
        if (Info.GetStatusCode() == "200")
        {
            BackendReturnObject contents = Backend.GameSchemaInfo.Get("Shop", InDate);

            JsonData data = contents.Rows()[0];

            if (data.Keys.Contains("Adblock"))
            {
                var temp = (bool)data["Adblock"]["BOOL"];
                Adblock = temp;
            }
            if (data.Keys.Contains("StartPackage"))
            {
                var temp = (bool)data["StartPackage"]["BOOL"];
                StartPackage = temp;
            }
        }
        // Shop테이블의 데이터를 받아오는데 실패
        else
        {
            Param param = new Param();

            param.Add("Adblock", false);
            param.Add("StartPackage", false);

            // 서버에 새로운 데이터 삽입
            Backend.GameSchemaInfo.Insert("Shop", param); // 동기

            Adblock = false;
            StartPackage = false;
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
                    Plus_Direction += value;
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
                    Plus_Direction = value;
                    break;
            }
        }
    }

    public void GetDirection()
    {
        Quality_Direction = 0;
        foreach (var item in Staffs)
            Quality_Direction += item.Directing;
        Quality_Direction += Plus_Direction;
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

    public void SetShopData()
    {
        // 플레이어 InDate
        string InDate = Backend.BMember.GetUserInfo().GetInDate();

        BackendReturnObject contents = Backend.GameSchemaInfo.Get("Shop", InDate);

        JsonData data = contents.Rows()[0];

        if (data.Keys.Contains("Adblock"))
        {
            var temp = (bool)data["Adblock"]["BOOL"];
            Adblock = temp;
        }
        if (data.Keys.Contains("StartPackage"))
        {
            var temp = (bool)data["StartPackage"]["BOOL"];
            StartPackage = temp;
        }
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

    public bool GoNextMonth()
    {
        Day++;

        bool IsNext = false;
        switch(Month)
        {
            case 2:
                if (Day > 28) IsNext = true;
                break;

            case 4: 
            case 6: 
            case 9: 
            case 11:
                if (Day > 30) IsNext = true;
                break;

            case 1: 
            case 3: 
            case 5: 
            case 7: 
            case 8: 
            case 10: 
            case 12:
                if (Day > 31) IsNext = true;
                break;
        }
        if (IsNext) {
            if (Month >= 12)
                Month = 1;
            else
                Month++;
            Day = 1;
            // 스태프 연봉 지급하는 코드

            return true;
        }
        return false;
    }
    #endregion

    #region Player Data

    public void ReStart()
    {
        Reset();
        for (int i = 0; i < StaffLevel.Length; i++)
            StaffLevel[i] = 0;
        Month = 1;
        Day = 1;
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
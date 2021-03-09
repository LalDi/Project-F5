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

    // 연극의 최고 점수
    public float Best_Quality { get; private set; }
    public float Best_Audience { get; private set; }
    public float Best_Profit { get; private set; }

    // 현재 보유 금액
    public int Money { get; private set; }

    // 현재 날짜
    public int Month { get; private set; }
    public int Day { get; private set; }

    // 설정된 연극 준비 기간
    public int Period { get; private set; }
    public float LeftDays { get; private set; }

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
    public bool OnPackage { get; private set; }
    public bool UsePackage { get; private set; }

    [SerializeField]
    public bool Tutorial;
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

    private new void Awake()
    {
        base.Awake();

#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = false;
#endif
        Screen.SetResolution(Screen.width, Screen.width * 16 / 9, true);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //Application.targetFrameRate = 144;

        Backend.Initialize(() =>
        {
            // 초기화 성공한 경우 실행
            if (Backend.IsInitialized)
            {
                Debug.Log("초기화 완료");

                // 게임 디버깅 및 테스트를 위한 임시 로그인
                //var data = Backend.BMember.CustomLogin("jungjh0513", "1234");
                //Debug.Log("로그인 완료");
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
    public void Start()
    {
        //Init();
    }

    public void Init()
    {
        //Backend.Chart.GetAllChartAndSave(true);

        DevelopData.Instance.SetDevelopData();
        Debug.Log("발전 데이터 생성");

        Staffs = StaffData.Instance.SetStaffData();
        Debug.Log("스태프 데이터 생성");

        LoadData();
        CreateDevelop();
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

        NowScenario = null;

        Actors.Clear();
        ActorData.Instance.SetActorsData();

        CreateDevelop();

        NowStep = Step.Select_Scenario;
    }

    public void SaveData()
    {
        string InDate = Backend.BMember.GetUserInfo().GetInDate();

        // Player 저장
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
            Debug.LogError("Player -> 새로운 데이터 생성");
        }
        else
        {
            InfoInDate = Info.Rows()[0]["inDate"]["S"].ToString();
            Backend.GameSchemaInfo.Update("Player", InfoInDate, param); // 동기
            Debug.LogError("Player -> 기존 데이터 갱신");
        }

        // PlayData 저장
        param = new Param();

        if (NowScenario == null)
            param.AddNull("Scenario");
        else
            param.Add("Scenario", NowScenario.No);
        param.Add("NowStep", (int)NowStep);

        param.Add("Play_Quality", Play_Quality);
        param.Add("Play_Marketing", Play_Marketing);
        param.Add("Play_Success", Play_Success);

        param.Add("Quality_Acting", Quality_Acting);
        param.Add("Quality_Scenario", Quality_Scenario);
        param.Add("Quality_Direction", Quality_Direction);

        param.Add("Plus_Direction", Plus_Direction);

        param.Add("Period", Period);
        param.Add("LeftDays", LeftDays);

        Info = Backend.GameInfo.GetPrivateContents("PlayData");

        if (Info.GetStatusCode() == "200")
        {
            if (Info.GetReturnValuetoJSON()["rows"].Count > 0)
            {
                InfoInDate = Info.Rows()[0]["inDate"]["S"].ToString();
                Backend.GameInfo.Update("PlayData", InfoInDate, param);
                Debug.Log("PlayData -> 기존 데이터 갱신");
            }
            else
            {
                Backend.GameInfo.Insert("PlayData", param);
                Debug.Log("PlayData -> 새로운 데이터 생성");
            }
        }

        // Actors 저장
        param = new Param();

        for (int i = 0; i < Actors.Count; i++)
        {
            int item = Actors[i].No;
            param.Add("Actor" + i, item);
        }

        Info = Backend.GameInfo.GetPrivateContents("Actors");

        if (Info.GetStatusCode() == "200")
        {
            if (Info.GetReturnValuetoJSON()["rows"].Count > 0)
            {
                InfoInDate = Info.Rows()[0]["inDate"]["S"].ToString();
                Backend.GameInfo.Delete("Actors", InfoInDate);
                Backend.GameInfo.Insert("Actors", param);
                Debug.Log("Actors -> 기존 데이터 갱신");
            }
            else
            {
                Backend.GameInfo.Insert("Actors", param);
                Debug.Log("Actors -> 새로운 데이터 생성");
            }
        }

        // Develops 저장
        param = new Param();

        for (int i = 0; i < Develops.Count; i++)
        {
            Develop item = Develops[i];
            param.Add("Develop" + i, item);
        }

        //Info = Backend.GameInfo.GetPrivateContents("Develops");
        //
        //if (Info.GetStatusCode() == "200")
        //{
        //    if (Info.GetReturnValuetoJSON()["rows"].Count > 0)
        //    {
        //        InfoInDate = Info.Rows()[0]["inDate"]["S"].ToString();
        //        Backend.GameInfo.Delete("Develops", InfoInDate);
        //        Backend.GameInfo.Insert("Develops", param);
        //        Debug.Log("Develops -> 기존 데이터 갱신");
        //    }
        //    else
        //    {
        //        Backend.GameInfo.Insert("Develops", param);
        //        Debug.Log("Develops -> 새로운 데이터 생성");
        //    }
        //}

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
                DefaultSuccess = PlayerPrefs.GetInt(PLAYERPREFSLIST.DEFAULT_SUCCESS,
                    int.Parse(data["DefaultSuccess"]["N"].ToString()));
            }
            if (data.Keys.Contains("BestQuality"))
            {
                Best_Quality = int.Parse(data["BestQuality"]["N"].ToString());
            }
            if (data.Keys.Contains("BestAudience"))
            {
                Best_Audience = int.Parse(data["BestAudience"]["N"].ToString());
            }
            if (data.Keys.Contains("BestProfit"))
            {
                Best_Profit = int.Parse(data["BestProfit"]["N"].ToString());
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
            //Year = PLAYER.DEFAULT_YEAR;
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
                OnPackage = temp;
            }
            if (data.Keys.Contains("UseStartPackage"))
            {
                var temp = (bool)data["UseStartPackage"]["BOOL"];
                UsePackage = temp;
            }
        }
        // Shop테이블의 데이터를 받아오는데 실패
        else
        {
            Param param = new Param();

            param.Add("Adblock", false);
            param.Add("StartPackage", false);
            param.Add("UseStartPackage", false);

            // 서버에 새로운 데이터 삽입
            Backend.GameSchemaInfo.Insert("Shop", param); // 동기

            Adblock = false;
            OnPackage = false;
            UsePackage = false;
        }

        // 서버로부터 PlayData테이블의 데이터를 받아옴
        Info = Backend.GameInfo.GetPrivateContents("PlayData");

        if (Info.GetStatusCode() == "200")
        {
            Debug.Log("PlayData 테이블의 데이터를 받아오는데 성공");

            // PlayData테이블의 데이터를 받아오는데 성공
            if (Info.GetReturnValuetoJSON()["rows"].Count > 0)
            {
                Debug.Log("PlayData테이블 데이터 있음");

                var data = BackendReturnObject.Flatten(Info.Rows());
                var Json = Info.Rows()[0];

                if (Json.Keys.Contains("Scenario"))
                {
                    if (Json["Scenario"].Keys.Contains("NULL"))
                    {
                        NowScenario = null;
                        MaxActor = 0;
                    }
                    else
                    {
                        int code = int.Parse(Json["Scenario"]["N"].ToString());
                        NowScenario = ScenarioData.Instance.FindScenario(code);
                        MaxActor = NowScenario.Actors;
                    }
                }
                if (Json.Keys.Contains("NowStep"))
                {
                    SetStep((Step)int.Parse(Json["NowStep"]["N"].ToString()));
                }
                if (Json.Keys.Contains("Play_Quality"))
                {
                    Play_Quality = int.Parse(Json["Play_Quality"]["N"].ToString());
                }
                if (Json.Keys.Contains("Play_Marketing"))
                {
                    Play_Marketing = int.Parse(Json["Play_Marketing"]["N"].ToString());
                }
                if (Json.Keys.Contains("Play_Success"))
                {
                    Play_Success = int.Parse(Json["Play_Success"]["N"].ToString());
                }
                if (Json.Keys.Contains("Quality_Acting"))
                {
                    Quality_Acting = int.Parse(Json["Quality_Acting"]["N"].ToString());
                }
                if (Json.Keys.Contains("Quality_Scenario"))
                {
                    Quality_Scenario = int.Parse(Json["Quality_Scenario"]["N"].ToString());
                }
                if (Json.Keys.Contains("Quality_Direction"))
                {
                    Quality_Direction = int.Parse(Json["Quality_Direction"]["N"].ToString());
                }
                if (Json.Keys.Contains("Plus_Direction"))
                {
                    Plus_Direction = int.Parse(Json["Plus_Direction"]["N"].ToString());
                }
                if (Json.Keys.Contains("Period"))
                {
                    Period = int.Parse(Json["Period"]["N"].ToString());
                }
                if (Json.Keys.Contains("LeftDays"))
                {
                    LeftDays = int.Parse(Json["LeftDays"]["N"].ToString());
                }
            }
            // PlayData테이블의 데이터를 받아오는데 실패
            else
            {
                Debug.Log("PlayData테이블의 데이터를 받아오는데 실패");

                Reset();

                Param param = new Param();

                if (NowScenario == null)
                    param.AddNull("Scenario");
                else
                    param.Add("Scenario", NowScenario.No);
                param.Add("NowStep", (int)NowStep);

                param.Add("Play_Quality", Play_Quality);
                param.Add("Play_Marketing", Play_Marketing);
                param.Add("Play_Success", Play_Success);

                param.Add("Quality_Acting", Quality_Acting);
                param.Add("Quality_Scenario", Quality_Scenario);
                param.Add("Quality_Direction", Quality_Direction);

                param.Add("Plus_Direction", Plus_Direction);

                param.Add("Period", Period);
                param.Add("LeftDays", LeftDays);

                Backend.GameInfo.Insert("PlayData", param);
                Debug.Log("PlayData -> 새로운 데이터 생성");
            }
        }

        // 서버로부터 Actors테이블의 데이터를 받아옴
        Info = Backend.GameInfo.GetPrivateContents("Actors");

        if (Info.GetStatusCode() == "200")
        {
            Debug.Log("Actors 테이블의 데이터를 받아오는데 성공");

            // Actors 테이블의 데이터를 받아오는데 성공
            if (Info.GetReturnValuetoJSON()["rows"].Count > 0)
            {
                Debug.Log("Actors 테이블 데이터 있음");

                var Data = Info.Rows()[0];

                for (int i = 0; i < MaxActor; i++)
                {
                    if (Data.Keys.Contains("Actor"+i))
                    {
                        int No = int.Parse(Data["Actor" + i]["N"].ToString());
                        var temp = ActorData.Instance.FindActor(No);
                        Actors.Add(temp);
                    }
                }

                NowActor = Actors.Count();
            }
            // Actors 테이블의 데이터를 받아오는데 실패
            else
            {
                Debug.Log("Actors 테이블 데이터 없음");

                Actors.Clear();
            }
        }
        

        //// 서버로부터 Develops테이블의 데이터를 받아옴
        //Info = Backend.GameInfo.GetPrivateContents("Develops");
        //
        //if (Info.GetStatusCode() == "200")
        //{
        //    Debug.Log("Develops 테이블의 데이터를 받아오는데 성공");
        //
        //    // Develops 테이블의 데이터를 받아오는데 성공
        //    if (Info.GetReturnValuetoJSON()["rows"].Count > 0)
        //    {
        //        Debug.Log("Develops 테이블 데이터 있음");
        //
        //        Develops.Clear();
        //
        //        var Data = Info.Rows()[0];
        //
        //        var json = BackendReturnObject.Flatten(Info.Rows());
        //        for (int i = 0; i < json.Count; i++)
        //        {
        //            var item = JsonMapper.ToObject<Develop>(json[i].ToJson());
        //            Develops.Add(item);
        //        }
        //
        //    }
        //    // Develops 테이블의 데이터를 받아오는데 실패
        //    else
        //    {
        //        Debug.Log("Develops 테이블 데이터 없음");
        //
        //        CreateDevelop();
        //    }
        //}
    }

    public void CreateDevelop()
    {
        Develops.Clear();
        Develops = DevelopData.Instance.GetDevelopRandom();

        Debug.Log("발전 데이터 재생성");
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
            OnPackage = temp;
        }
        if (data.Keys.Contains("UseStartPackage"))
        {
            var temp = (bool)data["UseStartPackage"]["BOOL"];
            UsePackage = temp;
        }
    }

    public void UsedStartPackage()
    {
        UsePackage = false;

        string InDate = Backend.BMember.GetUserInfo().GetInDate();
        var Info = Backend.GameSchemaInfo.Get("Shop", InDate);

        Param param = new Param();
        param.Add("StartPackage", true);
        param.Add("UseStartPackage", false);

        string InfoInDate = Info.Rows()[0]["inDate"]["S"].ToString();
        Backend.GameSchemaInfo.Update("Shop", InfoInDate, param); // 동기
    }

    public void CheckBestScore(float Quality, float Audience, float Profit)
    {
        string InDate = Backend.BMember.GetUserInfo().GetInDate();
        var Info = Backend.GameSchemaInfo.Get("Player", InDate);
        string InfoInDate = Info.Rows()[0]["inDate"]["S"].ToString();
        Param param = new Param();

        if (Quality > Best_Quality)
        {
            Best_Quality = Quality;
            param.Add("BestQuality", Quality);
        }
        if (Audience > Best_Audience)
        {
            Best_Audience = Audience;
            param.Add("BestAudience", Audience);
        }
        if (Profit > Best_Profit)
        {
            Best_Profit = Profit;
            param.Add("BestProfit", Profit);
        }

        Backend.GameSchemaInfo.Update("Player", InfoInDate, param, (callback) => { }); // 비동기
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
    public void SetLeftDays(int value)
    {
        LeftDays = value;
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
        LeftDays--;

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
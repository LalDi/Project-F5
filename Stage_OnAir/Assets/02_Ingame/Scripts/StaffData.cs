using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using BackEnd;
using Define;

public class Staff
{
    public string Name { get; private set; }

    public int Level { get; private set; }
    public int Pay { get; private set; }
    public int Directing { get; private set; }

    public int Cost_Purchase { get; private set; }
    public int Cost_Upgrade { get; private set; }

    public int Plus_Pay { get; private set; }
    public int Plus_Directing { get; private set; }
    public int Plus_Cost { get; private set; }

    public int Default_Pay { get; private set; }
    public int Default_Directing { get; private set; }
    public int Default_Cost { get; private set; }

    public bool IsPurchase { get; private set; } // 현재 스태프를 보유하고 있는지 여부
    public int Code { get; private set; } // 스태프를 구분하는 코드

    public Staff(JsonData Data)
    {
        Code = int.Parse(Data["num"]["S"].ToString());

        Name = Data["Name"]["S"].ToString();

        string indate = Backend.BMember.GetUserInfo().GetInDate();

        Debug.Log(Backend.GameSchemaInfo.Get("Staff", indate).GetStatusCode());
        if (Backend.GameSchemaInfo.Get("Staff", indate).GetStatusCode() == "200")
        {
            BackendReturnObject contents = Backend.GameSchemaInfo.Get("Staff", indate);
            var rows = contents.Rows();

            JsonData data = rows[0];

            string InfoName = "Staff" + Code.ToString();

            if (data.Keys.Contains(InfoName))
            {
                Level = int.Parse(data[InfoName]["N"].ToString());
            }
            else
                Debug.Log("그없");
        }
        else
        {
            Level = 0;
            Param param = new Param();
            param.Add("Staff" + Code.ToString(), Level);
            Backend.GameSchemaInfo.Insert("Staff", param, (callback) => { }); // 비동기
        }

        Default_Pay = int.Parse(Data["Pay"]["S"].ToString());
        Default_Directing = int.Parse(Data["Directing"]["S"].ToString());
        Default_Cost = int.Parse(Data["Cost_Upgrade"]["S"].ToString());

        Plus_Pay = int.Parse(Data["Plus_Pay"]["S"].ToString());
        Plus_Directing = int.Parse(Data["Plus_Directing"]["S"].ToString());
        Plus_Cost = int.Parse(Data["Plus_Cost"]["S"].ToString());

        Pay = Default_Pay + ((Level - 1) * Plus_Pay);
        Directing = Default_Directing + ((Level - 1) * Plus_Directing);

        Cost_Purchase = int.Parse(Data["Cost_Purchase"]["S"].ToString());
        Cost_Upgrade = Default_Cost + ((Level - 1) * Plus_Cost);

        IsPurchase = Level == 0 ? false : true;
    }

    public void SetLevel()
    {
        Pay = Default_Pay + (Plus_Pay * (Level - 1));
        Directing = Default_Directing + (Plus_Directing * (Level - 1));
        Cost_Upgrade = Default_Cost + (Plus_Cost * (Level - 1));
    }

    public void BuyStaff()
    {
        string Name = "Staff" + Code.ToString();
        string InDate = Backend.BMember.GetUserInfo().GetInDate();

        if (IsPurchase)
        {
            Debug.Log("이미 고용한 스태프입니다.");
            return;
        }

        GameManager.Instance.CostMoney(Cost_Purchase);
        Level = 1;
        SetLevel();
        SaveStaff();

        IsPurchase = true;
    }

    public void UpgradeStaff()
    {
        string Name = "Staff" + Code.ToString();
        string InDate = Backend.BMember.GetUserInfo().GetInDate();

        if (!IsPurchase)
        {
            Debug.Log("아직 고용하지 않은 스태프입니다.");
            return;
        }

        GameManager.Instance.CostMoney(Cost_Upgrade);
        Level++;
        SetLevel();
        SaveStaff();
    }

    public void SetStaff(JsonData Data)
    {
        Code = int.Parse(Data["num"]["S"].ToString());

        Name = Data["Name"]["S"].ToString();

        string indate = Backend.BMember.GetUserInfo().GetInDate();

        JsonData data = Backend.GameSchemaInfo.Get("Staff", indate).GetReturnValuetoJSON();
        Level = int.Parse(data["Staff" + Code.ToString()]["N"].ToString());

        Default_Pay = int.Parse(Data["Pay"]["S"].ToString());
        Default_Directing = int.Parse(Data["Directing"]["S"].ToString());
        Default_Cost = int.Parse(Data["Cost_Upgrade"]["S"].ToString());

        Plus_Pay = int.Parse(Data["Plus_Pay"]["S"].ToString());
        Plus_Directing = int.Parse(Data["Plus_Directing"]["S"].ToString());
        Plus_Cost = int.Parse(Data["Plus_Cost"]["S"].ToString());

        Pay = Default_Pay + ((Level - 1) * Plus_Pay);
        Directing = Default_Directing + ((Level - 1) * Plus_Directing);

        Cost_Purchase = int.Parse(Data["Cost_Purchase"]["S"].ToString());
        Cost_Upgrade = Default_Cost + ((Level - 1) * Plus_Cost);

        IsPurchase = Level == 0 ? false : true;
    }

    public void SaveStaff()
    {
        string Name = "Staff" + Code.ToString();
        string InDate = Backend.BMember.GetUserInfo().GetInDate();

        Param param = new Param();
        param.Add(Name, Level);

        var Info = Backend.GameSchemaInfo.Get("Staff", InDate);
        string InfoInDate;

        if (Info.GetStatusCode() == "404")
        {
            Backend.GameSchemaInfo.Insert("Staff", param, (callback) => { }); // 비동기
            Debug.Log("정보 추가");
        }
        else
        {
            InfoInDate = Info.Rows()[0]["inDate"]["S"].ToString();
            Backend.GameSchemaInfo.Update("Staff", InfoInDate, param, (callback) => { }); // 비동기
            Debug.Log("정보 갱신");
        }
    }
}

public class StaffData : Singleton<StaffData>
{
    public List<Sprite> StaffIcon = new List<Sprite>();
    public List<Sprite> StaffImage = new List<Sprite>();

    public List<Staff> SetStaffData()
    {
        List<Staff> StaffsList = new List<Staff>();

        JsonData ChartJson = JsonMapper.ToObject(Backend.Chart.GetLocalChartData("Staff"));
        var row = ChartJson["rows"];
        StaffsList.Clear();

        foreach (JsonData item in row)
        {
            Staff staff = new Staff(item);
            StaffsList.Add(staff);
        }

        return StaffsList;
    }

    public List<Staff> ResetStaffData()
    {
        List<Staff> StaffsList = SetStaffData();

        JsonData ChartJson = JsonMapper.ToObject(Backend.Chart.GetLocalChartData("Staff"));

        for (int i = 0; i < StaffsList.Count; i++)
        {
            foreach (JsonData item in ChartJson)
            {
                StaffsList[i].SetStaff(item);
            }
        }

        return StaffsList;
    }

    public static Staff FindStaff(int Code)
    {
        Staff result = null;
        List<Staff> StaffsList = GameManager.Instance.Staffs;

        foreach (var item in StaffsList)
        {
            if (item.Code == Code)
                result = item;
        }

        return result;
    }

    public static void SaveAllStaff()
    {
        List<Staff> StaffsList = GameManager.Instance.Staffs;

        foreach (var item in StaffsList)
        {
            item.SaveStaff();
        }
    }
}

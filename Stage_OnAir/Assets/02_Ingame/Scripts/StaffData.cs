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
        Code = int.Parse(Data["rows"]["num"]["S"].ToString());

        Name = Data["rows"]["Name"]["S"].ToString();
        Level = 0;

        Default_Pay = int.Parse(Data["rows"]["Pay"]["S"].ToString());
        Default_Directing = int.Parse(Data["rows"]["Directing"]["S"].ToString());
        Default_Cost = int.Parse(Data["rows"]["Cost_Upgrade"]["S"].ToString());

        Plus_Pay = int.Parse(Data["rows"]["Plus_Pay"]["S"].ToString());
        Plus_Directing = int.Parse(Data["rows"]["Plus_Directing"]["S"].ToString());
        Plus_Cost = int.Parse(Data["rows"]["Plus_Cost"]["S"].ToString());

        Pay = Default_Pay;
        Directing = Default_Directing;

        Cost_Purchase = int.Parse(Data["rows"]["Cost_Purchase"]["S"].ToString());
        Cost_Upgrade = Default_Cost;

        IsPurchase = false;
    }

    public void SetLevel()
    {
        Pay = Default_Pay + (Plus_Pay * Level-1);
        Directing = Default_Directing + (Plus_Directing * Level-1);
        Cost_Upgrade = Default_Cost + (Plus_Cost * Level-1);
    }

    public void BuyStaff()
    {
        string Name = "Staff" + Code.ToString();
        string InDate = Backend.BMember.GetUserInfo().GetInDate();

        GameManager.Instance.CostMoney(Cost_Purchase);
        Level = 1;
        SetLevel();
        IsPurchase = true;

        Param param = new Param();
        param.Add(Name, Level);
        Backend.GameSchemaInfo.Insert("Staff", param, (callback) => { }); // 비동기
    }

    public void UpgradeStaff()
    {
        string Name = "Staff" + Code.ToString();

        GameManager.Instance.CostMoney(Cost_Upgrade);
        Level++;
        SetLevel();

        Param param = new Param();
        param.Add(Name, Level);
        Backend.GameSchemaInfo.Insert("Staff", param, (callback) => { }); // 비동기
    }

    public void SetStaff(JsonData Data)
    {
        Code = int.Parse(Data["rows"]["num"]["S"].ToString());

        Name = Data["rows"]["Name"]["S"].ToString();

        string indate = Backend.BMember.GetUserInfo().GetInDate();

        JsonData data = Backend.GameSchemaInfo.Get("Staff", indate).GetReturnValuetoJSON();
        Level = int.Parse(data["rows"]["Staff" + Code.ToString()]["N"].ToString());

        Default_Pay = int.Parse(Data["rows"]["Pay"]["S"].ToString());
        Default_Directing = int.Parse(Data["rows"]["Directing"]["S"].ToString());
        Default_Cost = int.Parse(Data["rows"]["Cost_Upgrade"]["S"].ToString());

        Plus_Pay = int.Parse(Data["rows"]["Plus_Pay"]["S"].ToString());
        Plus_Directing = int.Parse(Data["rows"]["Plus_Directing"]["S"].ToString());
        Plus_Cost = int.Parse(Data["rows"]["Plus_Cost"]["S"].ToString());

        Pay = Default_Pay + ((Level - 1) * Plus_Pay);
        Directing = Default_Directing + ((Level - 1) * Plus_Directing);

        Cost_Purchase = int.Parse(Data["rows"]["Cost_Purchase"]["S"].ToString());
        Cost_Upgrade = Default_Cost + ((Level - 1) * Plus_Cost);

        IsPurchase = false;
    }
}

public class StaffData : MonoBehaviour
{
    public List<Staff> StaffsList = new List<Staff>();
    public List<Sprite> StaffImage = new List<Sprite>();
    public List<Sprite> StaffProfileImage = new List<Sprite>();

    public void SetStaffData()
    {
        JsonData ChartJson = JsonMapper.ToObject(Backend.Chart.GetLocalChartData("Staff"));
        StaffsList.Clear();

        foreach (JsonData item in ChartJson)
        {
            Staff staff = new Staff(item);
            StaffsList.Add(staff);
        }
    }

    public void ResetStaffData()
    {
        JsonData ChartJson = JsonMapper.ToObject(Backend.Chart.GetLocalChartData("Staff"));

        for (int i = 0; i < StaffsList.Count; i++)
        {
            foreach (JsonData item in ChartJson)
            {
                StaffsList[i].SetStaff(item);
            }
        }
    }

    public Staff FindStaff(int Code)
    {
        Staff result = null;

        foreach (var item in StaffsList)
        {
            if (item.Code == Code)
                result = item;
        }

        return result;
    }
}

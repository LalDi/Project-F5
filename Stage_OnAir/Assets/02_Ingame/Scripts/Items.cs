using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using BackEnd;

[System.Serializable]
public class ShopItem
{
    [Tooltip("번호")]    public int no;
    [Tooltip("이름")]    public string name;
    [TextArea(1, 3)]
    [Tooltip("설명")]    public string script;
    [Tooltip("가격")]    public int pay;
    [Tooltip("아이콘")] public Sprite Icon;
}
public class StaffItem
{
    public int no { get; private set; }
    public string name { get; private set; }
    public int pay { get; private set; }
    public int directing { get; private set; }
    public int cost_purchass { get; private set; }
    public int cost_upgrade { get; private set; }
    public int plus_pay { get; private set; }
    public int plus_directing { get; private set; }
    public int plus_cost { get; private set; }

    public Sprite Icon;

    public StaffItem(int _No, string _Name, int _Pay, int _Directing, int _Cost_purchass, 
        int _Cost_upgrade, int _Plus_pay, int _Plus_directing, int _Plus_cost)
    {
        no = _No;
        name = _Name;
        pay = _Pay;
        directing = _Directing;
        cost_purchass = _Cost_purchass;
        cost_upgrade = _Cost_upgrade;
        plus_pay = _Plus_pay;
        plus_directing = _Plus_directing;
        plus_cost = _Plus_cost;
    }
};
[System.Serializable]
public class MarketingItem
{
    [Tooltip("번호")] public int no;
    [Tooltip("이름")] public string name;
    [TextArea(1, 3)]
    [Tooltip("설명")] public string script;
    [Tooltip("가격")] public int pay;
    [Tooltip("아이콘")] public Sprite Icon;
};
[System.Serializable]
public class DevelopItem
{
    [Tooltip("번호")] public int no;
    [Tooltip("이름")] public string name;
    [TextArea(1, 3)]
    [Tooltip("설명")] public string script;
    [Tooltip("가격")] public int pay;
    [Tooltip("아이콘")] public Sprite Icon;
};
public class Items : Singleton<Items>
{
    public List<ShopItem> ShopItems = new List<ShopItem>();
    public List<StaffItem> StaffItems = new List<StaffItem>();
    public List<MarketingItem> MarketingItems = new List<MarketingItem>();
    public List<DevelopItem> DevelopItems = new List<DevelopItem>();

    public void SetStaffData()
    {
        JsonData ChartJson = JsonMapper.ToObject(Backend.Chart.GetLocalChartData("Staff"));
        var rows = ChartJson["rows"];
        StaffItems.Clear();

        foreach (JsonData item in rows)
        {
            StaffItem staff = new StaffItem(
                int.Parse(item["num"]["S"].ToString()),
                item["Name"]["S"].ToString(),
                int.Parse(item["Pay"]["S"].ToString()),
                int.Parse(item["Directing"]["S"].ToString()),
                int.Parse(item["Cost_Purchase"]["S"].ToString()),
                int.Parse(item["Cost_Upgrade"]["S"].ToString()),
                int.Parse(item["Plus_Pay"]["S"].ToString()),
                int.Parse(item["Plus_Directing"]["S"].ToString()),
                int.Parse(item["Plus_Cost"]["S"].ToString()) );

            StaffItems.Add(staff);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using BackEnd;
using Define;

public class Marketing
{
    public int Code { get; private set; }
    public string Name { get; private set; }
    public int Score { get; private set; }
    public int Price { get; private set; }

    public Marketing(JsonData Data)
    {
        Code = int.Parse(Data["Code"]["S"].ToString());
        Name = Data["Name"]["S"].ToString();
        Score = int.Parse(Data["Marketing"]["S"].ToString());
        Price = int.Parse(Data["Price"]["S"].ToString());
    }

}

public class MarketingData : Singleton<MarketingData>
{
    public List<Marketing> MarketingList = new List<Marketing>();
    public List<Sprite> MarketingIcon = new List<Sprite>();

    public void SetMarketingData()
    {
        Debug.Log("홍보 품목 초기화");
        JsonData ChartJson = JsonMapper.ToObject(Backend.Chart.GetLocalChartData("Marketing"));
        var rows = ChartJson["rows"];
        MarketingList.Clear();

        foreach (JsonData item in rows)
        {
            Marketing marketing = new Marketing(item);
            MarketingList.Add(marketing);
        }
        SortMarketing();
    }

    public void SortMarketing()
    {
        MarketingList.Sort(delegate (Marketing A, Marketing B)
        {
            if (A.Code > B.Code) return 1;
            else if (A.Code < B.Code) return -1;
            return 0;
        });
    }
}

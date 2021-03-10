using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using BackEnd;
using Define;

public class Develop
{
    public int Code { get; private set; }
    public string Name { get; private set; }
    public int Price { get; private set; }
    public int Effect { get; private set; }
    public int Effect_Code { get; private set; }
    public int Month { get; private set; }
    public bool IsOn { get; private set; }

    public Develop(JsonData Data)
    {
        Code = int.Parse(Data["num"]["S"].ToString());
        Name = Data["Name"]["S"].ToString();
        Price = int.Parse(Data["Price"]["S"].ToString());
        Effect = int.Parse(Data["Effect"]["S"].ToString());
        Effect_Code = int.Parse(Data["Effect_Code"]["S"].ToString());
        Month = int.Parse(Data["Month"]["S"].ToString());
        IsOn = false;
    }

    public void SetOn(bool value)
    {
        IsOn = value;
    }
}

public class DevelopData : Singleton<DevelopData>
{
    public List<Develop> DevelopList = new List<Develop>();
    public List<Sprite> DevelopIcon = new List<Sprite>();

    public void SetDevelopData()
    {
        Debug.Log("발전 품목 초기화");
        JsonData ChartJson = JsonMapper.ToObject(Backend.Chart.GetLocalChartData("Develop"));
        var rows = ChartJson["rows"];
        DevelopList.Clear();

        foreach (JsonData item in rows)
        {
            Develop develop = new Develop(item);
            int count = int.Parse(item["Count"]["S"].ToString());
            Debug.Log(develop.Name + ":"+ count+"개");
            
            for (int i = 0; i < count; i++)
            {
                DevelopList.Add(develop);
                Debug.Log(develop.Name + "추가됨");
            }
        }
    }

    public List<Develop> GetDevelopRandom()
    {
        List<Develop> result = new List<Develop>();

        OffDevelop();

        int rand = Random.Range(3, 6);

        while (result.Count != rand)
        {
            foreach (var item in DevelopList)
            {
                if (Random.Range(0, 2) == 1 && !item.IsOn)
                {
                    item.SetOn(true);
                    result.Add(item);
                }

                if (result.Count == rand)
                {
                    return result;
                }
            }
        }
        return result;
    }

    public void OffDevelop()
    {
        foreach (var item in DevelopList)
        {
            item.SetOn(false);
        }
    }

    public Develop FindDevelop(int code)
    {
        foreach (var item in DevelopList)
        {
            if (item.Code == code)
                return item;
        }

        Debug.LogError("없는 코드의 Develop을 찾으려 시도함");
        return null;
    }
}

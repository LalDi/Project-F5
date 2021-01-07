﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using BackEnd;
using Define;

public class Scenario
{
    public int No { get; private set; }
    public string Name { get; private set; }
    public int Quality { get; private set; }
    public int Actors { get; private set; }
    public int Price { get; private set; }
    public int Code { get; private set; }

    public Scenario(int _No, string _Name, int _Quality, int _Actors, int _Price, int _Code)
    {
        No = _No;
        Name = _Name;
        Quality = _Quality;
        Actors = _Actors;
        Price = _Price;
        Code = _Code;
    }
}
[System.Serializable]
public class ScenarioBG
{
    public Sprite Ingame;
    public Sprite Play_1;
    public Sprite Play_2;
}

public class ScenarioData : Singleton<ScenarioData>
{
    public List<Scenario> ScenarioList = new List<Scenario>();
    public ScenarioBG[] scenarioBGs = new ScenarioBG[10];

    public void SetScenarioData()
    {
        Debug.Log("시나리오 초기화");
        JsonData ChartJson = JsonMapper.ToObject(Backend.Chart.GetLocalChartData("Scenario"));
        var rows = ChartJson["rows"];
        ScenarioList.Clear();

        foreach (JsonData item in rows)
        {
            Scenario scenario = new Scenario(
                int.Parse(item["No"]["S"].ToString()),
                item["Name"]["S"].ToString(),
                int.Parse(item["Quality"]["S"].ToString()),
                int.Parse(item["Actors"]["S"].ToString()),
                int.Parse(item["Price"]["S"].ToString()),
                int.Parse(item["Code"]["S"].ToString()) );

            ScenarioList.Add(scenario);
        }
        SortScenario();
    }

    public void SortScenario()
    {
        ScenarioList.Sort(delegate (Scenario A, Scenario B)
        {
            if (A.No > B.No) return 1;
            else if (A.No < B.No) return -1;
            return 0;
        });
    }

    public Scenario FindScenario(int Code)
    {
        Scenario result = null;

        foreach (var item in ScenarioList)
        {
            if (item.Code == Code)
                result = item;
        }

        return result;
    }
}

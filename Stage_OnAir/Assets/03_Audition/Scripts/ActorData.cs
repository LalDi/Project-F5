using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using BackEnd;
using Define;

public class ActorData : Singleton<ActorData>
{
    public class Actor
    {
        public int No { get; private set; }
        public string Name { get; private set; }
        public int Acting { get; private set; }
        public int Experience { get; private set; }
        public int Price { get; private set; }
        public bool IsCasting { get; private set; }
        public int Sprite { get; private set; }

        public Actor(int _No, string _Name, int _Acting, int _Experience, int _Price, bool _IsCasting, int _Sprite)
        {
            No = _No;
            Name = _Name;
            Acting = _Acting;
            Experience = _Experience;
            Price = _Price;
            IsCasting = _IsCasting;
            Sprite = _Sprite;
        }

        public void SetIsCasting(bool Casting)
        {
            IsCasting = Casting;
        }
    }

    public List<Actor> ActorsList = new List<Actor>();

    //void Start()
    //{
    //    SetActorsData();
    //}

    /**
     *  @Func    SetActorsData()
     *  @remark  서버에서 데이터를 불러와 ActorsList의 값을 초기화 시키는 함수.
     */
    public void SetActorsData()
    {
        JsonData ChartJson = JsonMapper.ToObject(Backend.Chart.GetLocalChartData("Actor"));
        var rows = ChartJson["rows"];
        ActorsList.Clear();

        foreach (JsonData item in rows)
        {
            Actor actor = new Actor(
                int.Parse(item["num"]["S"].ToString()),
                item["Name"]["S"].ToString(),
                int.Parse(item["Acting"]["S"].ToString()),
                int.Parse(item["Experience"]["S"].ToString()),
                int.Parse(item["Price"]["S"].ToString()),
                false,
                int.Parse(item["Sprite"]["S"].ToString()));

            ActorsList.Add(actor);
        }
    }

    /**
     *  @Func    GetActorsData()
     *  @remark  새로운 배우가 추가되었을 때, 서버에서 그 데이터를 불러와 ActorsList에 추가하는 함수.
     *  @param   int Count - 서버에서 가져올 배우의 수
     */
    public void GetActorsData(int Count)
    {
        JsonData ChartJson = JsonMapper.ToObject(Backend.Chart.GetLocalChartData("Actor"));
        var rows = ChartJson["rows"];

        int NowActors = ActorsList.Count;

        for (int i = 0; i < Count; i++)
        {
            var Data = rows[NowActors + i + 1];
            Actor actor = new Actor(
                int.Parse(Data["num"]["S"].ToString()),
                Data["Name"]["S"].ToString(),
                int.Parse(Data["Acting"]["S"].ToString()),
                int.Parse(Data["Experience"]["S"].ToString()),
                int.Parse(Data["Price"]["S"].ToString()),
                false,
                int.Parse(Data["Sprite"]["S"].ToString()));

            ActorsList.Add(actor);
        }
    }

    /**
     *  @Func    RandomActors(int Count)
     *  @remark  ActorList에서 캐스팅 되지 않은 배우들을 정해진 수만큼 랜덤으로 받아오는 함수
     *  @param   int Count - 목록에서 가져올 배우의 수
     *  @return  Count수만큼 랜덤으로 받아온 배우들의 List값
     */
    public List<Actor> RandomActors(int Count)
    {
        List<Actor> RandomActor = new List<Actor>();

        List<Actor> NonSelect = new List<Actor>();

        foreach (Actor item in ActorsList)
        {
            if (item.IsCasting == false)
            {
                NonSelect.Add(item);
            }
        }

        NonSelect = Math.ShuffleList(NonSelect);
        
        for (int i = 0; i < Count; i++)
        {
            RandomActor.Add(NonSelect[i]);
        }

        return RandomActor;
    }

    /**
     *  @Func    RiseActor(List<Actor> Actors, bool IsSuccess)
     *  @remark  지정된 배우들의 경험, 가격을 성장시키는 함수
     *  @param   List<Actor> Actors - 성장할 배우들의 목록
     *  @param   bool IsSuccess - 목록에서 가져올 배우의 수
     */
    public void RiseActor(List<Actor> Actors, bool IsSuccess)
    {
        if (IsSuccess)
        {

        }
        else
        {

        }
    }

}

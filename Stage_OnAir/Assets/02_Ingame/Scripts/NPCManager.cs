using Define;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public Transform Parent;

    public List<int> Actors = new List<int>(); //화면에 그려줄 배우 NPC 데이터
    public int Staffs; //화면에 그려줄 스탭 NPC 데이터

    public List<GameObject> ActorPre = new List<GameObject>();
    public List<GameObject> StaffPre = new List<GameObject>();

    public void Summon()
    {
        Actors = RandomActor(GameManager.Instance.NowActor);

        Staffs = RandomStaff();

        for (int i = 0; i < Actors.Count(); i++) {
            GameObject actorObj = Instantiate(ActorPre[Actors[i]], Parent);
            actorObj.transform.localPosition = new Vector3(-800 * (i - 1), -800);
        }
        
        if (Staffs != -1)
        {
            GameObject staffObj = Instantiate(StaffPre[Staffs], Parent);
            staffObj.transform.localPosition = new Vector3(0, -1100);
        }
    }

    public List<int> RandomActor(int count)
    {
        if (count > 2) count = 2;

        List<int> RandomActor = new List<int>();
        List<Actor> Select = new List<Actor>();

        Select = GameManager.Instance.Actors.ToList();
        Select = Math.ShuffleList(Select);

        for (int i = 0; i < count; i++)
        {
            RandomActor.Add(Select[i].No % 8);
        }

        return RandomActor;
    }

    public int RandomStaff()
    {
        List<int> RandomStaff = new List<int>();

        foreach (var item in GameManager.Instance.Staffs)
        {
            if (item.IsPurchase)
                RandomStaff.Add(item.Code-1);
        }

        if (RandomStaff.Count == 0)
            return -1;

        RandomStaff = Math.ShuffleList(RandomStaff);

        return RandomStaff[0];
    }
}

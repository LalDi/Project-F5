using Define;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public Transform Parent;

    public List<int> Actors = new List<int>(); //화면에 그려줄 배우 NPC 데이터
    public List<int> Staffs = new List<int>(); //화면에 그려줄 스탭 NPC 데이터
    public int StaffCnt = 0;

    public List<GameObject> Actor = new List<GameObject>();
    public List<GameObject> Staff = new List<GameObject>();

    public void Summon()
    {
        Actors = RandomActor(GameManager.Instance.NowActor);

        foreach (var item in GameManager.Instance.Staffs)
            if (item.Level >= 1) StaffCnt++;
        Staffs = RandomStaff(StaffCnt);

        for (int i = 0; i < Actors.Count(); i++) {
            GameObject obj = Instantiate(Actor[Actors[i]], Parent);
            obj.transform.localPosition = new Vector3(-800 * (i - 1), -800);
        }
        for (int i = 0; i < Staffs.Count(); i++)
        {
            GameObject obj = Instantiate(Staff[Staffs[i]], Parent);
            obj.transform.localPosition = new Vector3(-800 * (i - 1), -1100);
        }
    }

    public List<int> RandomActor(int count)
    {
        if (count > 3) count = 3;

        List<int> RandomActor = new List<int>();
        List<Actor> Select = new List<Actor>();

        Select = GameManager.Instance.Actors.ToList();
        Select = Math.ShuffleList(Select);

        foreach(var item in Select)
        {
            Debug.Log("캐스팅 된 배우: " + item.Name);
        }

        for (int i = 0; i < count; i++)
        {
            RandomActor.Add(Select[i].No % 8);
        }

        return RandomActor;
    }
    public List<int> RandomStaff(int count)
    {
        if (count > 3) count = 3;

        List<int> RandomStaff = new List<int>();
        List<Staff> Select = new List<Staff>();

        int value = 0;

        foreach (var item in GameManager.Instance.Staffs)
        {
            if (item.Level >= 1)
                Select.Add(item);
        }

        Select = Math.ShuffleList(Select);

        for (int i = 0; i < count; i++)
        {
            RandomStaff.Add(Select[i].Code);
        }

        return RandomStaff;
    }
}

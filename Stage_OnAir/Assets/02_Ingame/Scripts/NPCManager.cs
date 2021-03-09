using Define;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public Transform Parent;

    public int[] Actors = new int[3]; //화면에 그려줄 배우 NPC 데이터
    public int[] Staffs = new int[3]; //화면에 그려줄 스탭 NPC 데이터
    public int StaffCnt = 0;

    public GameObject[] Actor = new GameObject[8];
    public GameObject[] Staff = new GameObject[7];

    void Start()
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

    public int[] RandomActor(int count)
    {
        if (count > 3) count = 3;

        int[] RandomActor = new int[count];
        List<Actor> Select = new List<Actor>();

        Select = GameManager.Instance.Actors;
        Select = Math.ShuffleList(Select);

        for (int i = 0; i < count; i++)
        {
            RandomActor[i] = Select[i].No;
        }

        return RandomActor;
    }
    public int[] RandomStaff(int count)
    {
        if (count > 3) count = 3;

        int[] RandomStaff = new int[count];
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
            RandomStaff[i] = Select[i].Code;
        }

        return RandomStaff;
    }
}

//using Define;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class NPCManager : MonoBehaviour
//{
//    public GameObject Parent;

//    public int[] Actors = new int[3]; //화면에 그려줄 배우 NPC 데이터
//    public int[] Staffs = new int[3];; //화면에 그려줄 스탭 NPC 데이터

//    public GameObject[] Actor = new GameObject[8];
//    public GameObject[] Staff = new GameObject[7];

//    void Start()
//    {
//        Actors
//        ActorCount += GameManager.Instance.NowActor;

//        for(int i = 0; i < ActorCount; i++)
//        {
//            GameObject obj = GameManager.Instance.Staffs[Random.Range(0, )]
//        }
//    }

//    void Update()
//    {
        
//    }

//    public int[] RandomActor(int count)
//    {
//        int[] RandomActor = new int[];

//        List<Actor> Select = new List<Actor>();

//        Select = GameManager.Instance.Actors;
//        Select = Math.ShuffleList(Select);

//        for (int i = 0; i < count; i++)
//        {
//            RandomActor[i] = Select[i].No;
//        }

//        return RandomActor;
//    }
//    public int[] RandomStaff(int count)
//    {
//        int[] RandomStaff = new int[];

//        List<Actor> Select = new List<Actor>();

//        int value = 0;

//        foreach(var item in GameManager.Instance.Staffs)
//        {
//            if (item.Level >= 1)
//                Select[value] = item.n
//        }

//        Select = Math.ShuffleList(Select);

//        for (int i = 0; i < count; i++)
//        {
//            RandomStaff[i] = Select[i].No;
//        }

//        return RandomStaff;
//    }
//}

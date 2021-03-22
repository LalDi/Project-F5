using Define;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NPCManager : MonoBehaviour
{
    public Transform Parent;
    [Space(10f)]
    public List<int> Actors = new List<int>(); //화면에 그려줄 배우 NPC 데이터
    public int Staffs; //화면에 그려줄 스탭 NPC 데이터
    [Space(10f)]
    public List<GameObject> ActorPre = new List<GameObject>();
    public List<GameObject> StaffPre = new List<GameObject>();
    [Space(10f)]
    public GameObject[] ActorObj = new GameObject[2];
    public GameObject StaffObj;
    [Space(10f)]
    public bool IsOn_Scr = true;
    public List<string> Scripts = new List<string>();
    public GameObject ScrChr;
    public GameObject ScrObj_1;
    public GameObject ScrObj_2;

    private void Update()
    {
        if (ScrChr != null)
            ScrObj_1.transform.parent.position = ScrChr.transform.position + new Vector3(0, 1000f);
    }

    public void Summon()
    {
        Actors = RandomActor(GameManager.Instance.NowActor);

        Staffs = RandomStaff();

        for (int i = 0; i < Actors.Count(); i++) {
            ActorObj[i] = Instantiate(ActorPre[Actors[i]], Parent);
            ActorObj[i].transform.localPosition = new Vector3(-800 * (i - 1), -800);
        }
        
        if (Staffs != -1)
        {
            StaffObj = Instantiate(StaffPre[Staffs], Parent);
            StaffObj.transform.localPosition = new Vector3(0, -1100);
        }
    }

    public void DisSummon()
    {
        for (int i = 0; i < 2; i++)
        {
            if (ActorObj[i] != null)
            { 
                ActorObj[i].SetActive(false);
                ActorObj[i] = null;
            }
        }
        if (StaffObj != null)
        {
            StaffObj.SetActive(false);
            StaffObj = null;
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

    public void Scr_Bt_On()
    {
        if (IsOn_Scr == true)
            return;

        ScrChr = (Random.Range(0, 2) == 0) ? ActorObj[Random.Range(0, 2)] : StaffObj ;

        ScrObj_1.SetActive(true);
        ScrObj_2.SetActive(false);
        IsOn_Scr = true;
    }

    public void Scr_On()
    {
        Debug.Log("Scr_On");
        ScrObj_1.SetActive(false);
        ScrObj_2.transform.Find("Text").GetComponent<Text>().text = Scripts[Random.Range(0, Scripts.Count + 1)];
        ScrObj_2.SetActive(true);
        StartCoroutine(Scr_Timer(Random.Range(5, 10)));
    }

    public IEnumerator Scr_Timer(int time)
    {
        yield return new WaitForSeconds(3f);
        ScrObj_1.SetActive(false);
        ScrObj_2.SetActive(false);
        ScrObj_2.transform.GetChild(0).GetComponent<Text>().text = "";
        yield return new WaitForSeconds(time);
        IsOn_Scr = false;
    }
}

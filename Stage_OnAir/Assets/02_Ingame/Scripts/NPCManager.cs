using Define;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIExtensions;
using DG.Tweening;

public class NPCManager : MonoBehaviour
{
    public Transform Parent;
    [Space(10f)]
    public List<int> Actors = new List<int>(); //화면에 그려줄 배우 코드
    public int Staffs; //화면에 그려줄 스태프 코드
    [Space(10f)]
    public List<GameObject> ActorPre = new List<GameObject>(); //배우 NPC Prefab
    public List<GameObject> StaffPre = new List<GameObject>(); //스탭 NPC Prefab
    [Space(10f)]
    public GameObject[] ActorObj = new GameObject[2]; //그려주고 있는 배우 Object
    public GameObject StaffObj; //그려주고 있는 스탭 Object
    [Space(10f)]
    public bool IsOn_Scr = false; //화면에 그려줄 타이밍인가?
    public List<string> Scripts = new List<string>(); //대사 들
    public GameObject ScrChr;

    public GameObject ScrObj_1;
    public GameObject ScrObj_2;
    public UIParticle ParticleObj;

    public IEnumerator Crt;

    private void Update()
    {
        if (ScrChr != null)
            ScrObj_1.transform.parent.position = ScrChr.transform.position + new Vector3(0, 1000f);
    }

    public void Summon()
    {
        Actors = RandomActor(GameManager.Instance.NowActor);

        Staffs = RandomStaff();

        if (Actors != null)
        {
            for (int i = 0; i < Actors.Count(); i++)
            {
                ActorObj[i] = Instantiate(ActorPre[Actors[i]], Parent);
                ActorObj[i].transform.localPosition = new Vector3(-800 * (i - 1), -750);
                ActorObj[i].transform.GetComponent<NPC>().Code = 10 + i;
            }
        }

        if (Staffs != -1)
        {
            StaffObj = Instantiate(StaffPre[Staffs], Parent);
            StaffObj.transform.localPosition = new Vector3(0, -1100);
            StaffObj.transform.GetComponent<NPC>().Code = Staffs;
        }

        ScrObj_1.SetActive(false);
        ScrObj_2.SetActive(false);
        Crt = Scr_Timer(30);
        StartCoroutine(Crt);
    }

    public void DisSummon()
    {
        for (int i = 0; i < 2; i++)
        {
            if (ActorObj[i] != null)
            {
                ActorObj[i].GetComponent<NPC>().Stop();
                DOTween.Kill(ActorObj[i]);
                ActorObj[i].SetActive(false);
                ActorObj[i] = null;
            }
        }
        if (StaffObj != null)
        {
            StaffObj.GetComponent<NPC>().Stop();
            DOTween.Kill(StaffObj);
            StaffObj.SetActive(false);
            StaffObj = null;
        }

        ScrObj_1.SetActive(false);
        ScrObj_2.SetActive(false);

        StopCoroutine(Crt);
        ScrObj_1.SetActive(false);
        ScrObj_2.SetActive(false);
        ScrObj_2.transform.GetChild(0).GetComponent<Text>().text = "";
        IsOn_Scr = false;
    }

    public List<int> RandomActor(int count)
    {
        if (count <= 0) return null;
        if (count > 2) count = 2;

        List<int> RandomActor = new List<int>();
        List<Actor> Select = new List<Actor>();

        Select = new List<Actor>(GameManager.Instance.Actors);
        Select = Math.ShuffleList(Select);

        for (int i = 0; i < count; i++)
        {
            RandomActor.Add(Select[i].Sprite - 1);
            Debug.Log(Select[i].Name);
        }

        return RandomActor;
    }

    public int RandomStaff()
    {
        List<int> RandomStaff = new List<int>();

        foreach (var item in GameManager.Instance.Staffs)
        {
            if (item.IsPurchase)
                RandomStaff.Add(item.Code - 1);
        }

        if (RandomStaff.Count == 0)
            return -1;

        RandomStaff = Math.ShuffleList(RandomStaff);

        return RandomStaff[0];
    }

    public void Scr_Bt_On()
    {
        if (IsOn_Scr == false)
            return;

        if (ActorObj[0] == null && StaffObj == null)
        {
            IsOn_Scr = false;
            return;
        }
        else if (ActorObj[0] != null && StaffObj != null)
            ScrChr = (Random.Range(0, 2) == 0) ? ActorObj[Random.Range(0, 2)] : StaffObj;
        else
        {
            if (ActorObj[0] == null)
                ScrChr = StaffObj;
            else if (StaffObj == null)
                ScrChr = ActorObj[(Random.Range(0, 2))];
        }

        ScrObj_1.SetActive(true);
        ScrObj_2.SetActive(false);
        IsOn_Scr = false;
    }

    public void Scr_On()
    {
        string text;
        if (ScrChr.GetComponent<NPC>().Code >= 10)
            text = Scripts[Random.Range(0, 7)];
        else
            text = Scripts[7 + (ScrChr.GetComponent<NPC>().Code * 2) + Random.Range(0, 2)];
        ScrObj_2.transform.Find("Text").GetComponent<Text>().text = text;
        ScrObj_1.SetActive(false);
        ScrObj_2.SetActive(true);

        GameManager.Instance.CostMoney(Random.Range(1000, 5000), false);
        ParticleObj.Play();
        Crt = Scr_Timer(Random.Range(10, 30));
        StartCoroutine(Crt);
    }

    public IEnumerator Scr_Timer(int time)
    {
        yield return new WaitForSeconds(2f);
        ScrObj_1.SetActive(false);
        ScrObj_2.SetActive(false);
        ScrObj_2.transform.GetChild(0).GetComponent<Text>().text = "";
        yield return new WaitForSeconds(time);
        IsOn_Scr = true;
    }
}

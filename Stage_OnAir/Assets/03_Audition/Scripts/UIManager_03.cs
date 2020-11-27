using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_03 : MonoBehaviour
{
    public GameObject Popup_Balck;
    public GameObject Popup_Result;
    public GameObject Count;
    public GameObject Profile;

    public List<ActorData.Actor> PprActors;
    public List<ActorData.Actor> PassActors = new List<ActorData.Actor>();

    public int ActorCount;//오디션 완료한 배우들 수 카운트
    //Preparation Actors
    void Start()
    {
        Popup_Balck.SetActive(false);
        Popup_Result.SetActive(false);

        ActorCount = 0;
        //오디션 배우 준비
        PprActors = ActorData.Instance.
            RandomActors(GameManager.Instance.MaxActor - GameManager.Instance.NowActor);
        Reroad_ActorProfile();
    }

    public void Reroad_ActorProfile()
    {
        ActorCount++;
        Count.transform.GetChild(0).GetComponent<Text>().text =
            GameManager.Instance.MaxActor - GameManager.Instance.NowActor + "";
        Profile.transform.GetChild(2).GetComponent<Text>().text = PprActors[ActorCount].Name;
        Profile.transform.GetChild(3).GetComponent<Text>().text =
            "연기력 : " + PprActors[ActorCount].Acting + "\n" + 
            "경험 : " + PprActors[ActorCount].Experience + "\n" +
            "가격 : " + PprActors[ActorCount].Price.ToString();
    }

    public void Out_BT()
    {
        Reroad_ActorProfile();
    }
    public void Pass_BT()
    {
        PassActors.Add(PprActors[ActorCount]);
        Reroad_ActorProfile(); 
    }

    public void To_Ingame()
    {
        LoadManager.LoaderCallback();
        LoadManager.Load(LoadManager.Scene.Ingame);
    }
}

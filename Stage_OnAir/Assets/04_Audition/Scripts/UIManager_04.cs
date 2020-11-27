using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_04 : MonoBehaviour
{
    public GameObject ActorPrefab;

    public GameObject Popup_Balck;
    public GameObject Popup_Result;
    public GameObject Count;
    public GameObject Profile;

    public List<Actor> PprActors = null;
    public List<Actor> PassActors = new List<Actor>();

    public int ActorCount;
    //오디션 완료한 배우들 수 카운트 (Preparation Actors)
    public int MaxActor; 
    //오디션 보는 배우 수 (한명에서 7명정도)
    void Start()
    {
        Popup_Balck.SetActive(false);
        Popup_Result.SetActive(false);

        ActorCount = 0;
        MaxActor = Random.Range(1, 7);
    }
    void Update()
    {
        if (PprActors == null)
        {
            ActorData.Instance.SetActorsData();
            PprActors = ActorData.Instance.RandomActors(MaxActor);
            Reroad_ActorProfile();
        }
    }

    public void Reroad_ActorProfile()
    {
        ActorCount++;
        if (ActorCount <= MaxActor)
        {
            Count.transform.Find("Count Text").GetComponent<Text>().text = 
                ActorCount.ToString() + " / " + MaxActor;
            Profile.transform.Find("Profile Name Text").GetComponent<Text>().text = PprActors[ActorCount-1].Name;
            Profile.transform.Find("Profile Stats Text").GetComponent<Text>().text =
                "연기력 : " + PprActors[ActorCount-1].Acting + "\n" +
                "경험 : " + PprActors[ActorCount-1].Experience + "\n" +
                "가격 : " + PprActors[ActorCount-1].Price.ToString();
        }
        else
        {
            Popup_Balck.SetActive(true);
            Popup_Result.SetActive(true);
            Result();
        }
    }

    public void Result()
    {
        for(int i = 0; i < PassActors.Count; i++)
        {
            PassActors[i].SetIsCasting(true);
            GameManager.Instance.Actors.Add(PassActors[i]);

            GameObject ActorData = Instantiate(ActorPrefab);
            ActorData.transform.parent = Popup_Result.transform.Find("Scroll Rect Mask").GetChild(0);
            ActorData.transform.Find("Actor Name Text").GetComponent<Text>().text =
                PassActors[i].Name;
        }
    }

    public void Out_BT()
    {
        Reroad_ActorProfile();
    }
    public void Pass_BT()
    {
        PassActors.Add(PprActors[ActorCount-1]);
        Reroad_ActorProfile(); 
    }

    public void To_Ingame()
    {
        LoadManager.LoaderCallback();
        LoadManager.Load(LoadManager.Scene.Ingame);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

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
        //Backend.Initialize(() =>
        //{
        //    // 초기화 성공한 경우 실행
        //    if (Backend.IsInitialized)
        //    {
        //        var data = Backend.BMember.CustomLogin("test2", "1234");

        //        Debug.Log("초기화 완료");
        //    }
        //    // 초기화 실패한 경우 실행
        //    else
        //    {

        //    }
        //});

        //Backend.Chart.GetAllChartAndSave(true);

        Popup_Balck.SetActive(false);
        Popup_Result.SetActive(false);

        ActorCount = 0;
        //MaxActor = Random.Range(1, GameManager.Instance.MaxActor - GameManager.Instance.NowActor);
        MaxActor = Random.Range(1, 7);

        PprActors = ActorData.Instance.RandomActors(MaxActor);
        Reroad_ActorProfile();
    }
    public void Reroad_ActorProfile()
    {
        ActorCount++;
        if (GameManager.Instance.NowActor + PassActors.Count >= GameManager.Instance.MaxActor)
        {
            GameManager.Instance.SetStep(GameManager.Step.Set_Period);
            Popup_Balck.SetActive(true);
            Popup_Result.SetActive(true);
            Result();
        }
        else if (ActorCount <= MaxActor)
        {
            Count.transform.Find("Count Text").GetComponent<Text>().text =
                ActorCount.ToString() + " / " + MaxActor;
            Profile.transform.Find("Profile Name Text").GetComponent<Text>().text = PprActors[ActorCount - 1].Name;
            Profile.transform.Find("Profile Stats Text").GetComponent<Text>().text =
                "연기력 : " + PprActors[ActorCount - 1].Acting + "\n" +
                "경험 : " + PprActors[ActorCount - 1].Experience + "\n" +
                "가격 : " + PprActors[ActorCount - 1].Price.ToString();
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
        for (int i = 0; i < PassActors.Count; i++)
        {
            GameObject ActorProfile = Instantiate(ActorPrefab);
            ActorProfile.transform.parent = Popup_Result.transform.Find("Scroll Rect Mask").GetChild(0);
            ActorProfile.transform.Find("Actor Name Text").GetComponent<Text>().text =
                PassActors[i].Name;
            ActorData.Instance.ActorsList[PassActors[i].No].SetIsCasting(true);
            GameManager.Instance.Actors.Add(ActorData.Instance.ActorsList[PassActors[i].No]);
            GameManager.Instance.PlusNowActor();
        }
        Popup_Result.transform.Find("Scroll Rect Mask").GetChild(0).GetComponent<RectTransform>().sizeDelta =
            new Vector2(PassActors.Count * 325, 450);
    }

    public void Out_BT()
    {
        Reroad_ActorProfile();
    }
    public void Pass_BT()
    {
        PassActors.Add(PprActors[ActorCount - 1]);
        Reroad_ActorProfile();
    }

    public void To_Ingame()
    {
        LoadManager.LoaderCallback();
        LoadManager.Load(LoadManager.Scene.Ingame);
    }
}

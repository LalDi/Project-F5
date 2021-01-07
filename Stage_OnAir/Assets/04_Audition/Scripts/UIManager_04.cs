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

    public SpriteRenderer Character;

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
        MaxActor = Random.Range(1, 8 - GameManager.Instance.NowActor);

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
            Count.GetComponent<Text>().text =
                ActorCount.ToString() + " / " + MaxActor;
            Profile.transform.GetChild(0).GetComponent<Text>().text = PprActors[ActorCount - 1].Name;
            Profile.transform.GetChild(1).GetComponent<Text>().text =
                "연기력 : " + PprActors[ActorCount - 1].Acting + "\n" +
                "경험 : " + PprActors[ActorCount - 1].Experience + "\n" +
                "가격 : " + PprActors[ActorCount - 1].Price.ToString();
            Profile.transform.GetChild(5).GetComponent<Image>().sprite = ActorData.Instance.ActorProfileImage[PprActors[ActorCount - 1].No];
            Character.sprite = ActorData.Instance.ActorImage[PprActors[ActorCount - 1].No];
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
        float Width = 0;
        for(int i = 0; i < PassActors.Count; i++)
        {
            Width += 325;

            GameObject ActorObj = Instantiate(ActorPrefab);
            ActorObj.transform.SetParent(Popup_Result.transform.GetChild(2).GetChild(0));
            ActorObj.transform.GetChild(1).GetComponent<Text>().text =
                PassActors[i].Name;
            ActorObj.transform.GetChild(0).GetComponent<Image>().sprite = 
                ActorData.Instance.ActorProfileImage[PassActors[i].No];
            ActorData.Instance.ActorsList[PassActors[i].No].SetIsCasting(true);
            GameManager.Instance.Actors.Add(ActorData.Instance.ActorsList[PassActors[i].No]);
            GameManager.Instance.PlusNowActor();
            GameManager.Instance.Plus_Quality_Acting(PassActors[i].Acting);
        }
        Popup_Result.transform.GetChild(2).GetChild(0).
            GetComponent<RectTransform>().sizeDelta = new Vector2(Width, 940.4614f);
    }

    public void Out_BT()
    {
        SoundManager.Instance.PlaySound("Pop_3");
        Reroad_ActorProfile();
    }
    public void Pass_BT()
    {
        SoundManager.Instance.PlaySound("Pop_6");
        PassActors.Add(PprActors[ActorCount - 1]);
        Reroad_ActorProfile();
    }

    public void To_Ingame()
    {
        SoundManager.Instance.PlaySound("Prize_Wheel_Spin_2_Reward");
        LoadManager.LoaderCallback();
        LoadManager.Load(LoadManager.Scene.Ingame);
    }
}

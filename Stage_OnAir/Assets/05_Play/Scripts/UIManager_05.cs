using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using Coffee.UIExtensions;

public class UIManager_05 : MonoBehaviour
{
    public bool Success = false;

    public List<GameObject> Backgrounds;
    public GameObject Light;

    public GameObject Script_Image;
    public Text tmp;
    [SerializeField]
    public ScriptS Scrs = new ScriptS();
    public Sprite[] Illusts = new Sprite[10];
    
    public GameObject BlackBG;
    public GameObject Popup_Result;
    public GameObject Popup_Illust;
    public GameObject Bgm;
    public GameObject SkipBT;
    public GameObject EndingUI;

    public SpriteRenderer Character1;
    public SpriteRenderer Character2;

    public IEnumerator DelayCrt;

    void Start()
    {
        if (GameManager.Instance.Adblock == false)
        {
            GoogleAdsManager.Instance.IntersAdsShow();
        }

        SoundManager.Instance.StopBGM();
        Bgm = SoundManager.Instance.LoopSound(Scrs.scripts[GameManager.Instance.NowScenario.No - 1].Bgm 
            ? "Bgm_Play2" : "Bgm_Play1");

        //시나리오에 맞는 배경 켜기
        foreach (var Obj in Backgrounds)
        {
            Obj.SetActive(false);
        }
        Backgrounds[GameManager.Instance.NowScenario.No - 1].SetActive(true);

        Light.GetComponent<SpriteRenderer>().DOFade(0, 2).SetLoops(-1, LoopType.Yoyo);

        Character1.sprite = ActorData.Instance.ActorImage[GameManager.Instance.Actors[1].No];
        Character2.sprite = ActorData.Instance.ActorImage[GameManager.Instance.Actors[2].No];

        DelayCrt = StartDelay();
        StartCoroutine(DelayCrt);
    }

    public IEnumerator StartDelay()
    {
        BlackBG.GetComponent<Image>().DOFade(0, 2);
        yield return new WaitForSeconds(2f);
        BlackBG.SetActive(false);
        Script_Image.SetActive(true);

        Script Scr = Scrs.scripts[GameManager.Instance.NowScenario.No - 1];

        for (int i = 0; i < Scr.script.Count; i++)
        {
            tmp.text = "";
            tmp.DOText(Scr.script[i], 3f);
            Script_Image.transform.localScale = new Vector3((Scr.Direction[i] ? 1 : -1), 1, 1);
            yield return new WaitForSeconds(4f);
        }
        SkipBT.SetActive(false);
        StartCoroutine(Result());
    }

    public void Skip()
    {
        SkipBT.SetActive(false);
        StopCoroutine(DelayCrt);
        StartCoroutine(Result());
    }

    public IEnumerator Result()
    {
        //성공여부 판단
        int Rand = UnityEngine.Random.Range(1, 101);
        Success = (Rand <= GameManager.Instance.Play_Success);
        Debug.Log("결과" + Success);

        EndingUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text
            = (Success) ? "<bounce>공연 성공!</bounce>" : "공연 실패...";
        EndingUI.transform.GetChild(0).DOLocalMoveY(-35, 1.5f).SetEase(Ease.Linear);
        EndingUI.transform.GetChild(1).DOLocalMoveY(-500, 0.5f);
        yield return new WaitForSeconds(1f);

        BlackBG.SetActive(true);
        BlackBG.GetComponent<Image>().DOFade(0.5f, 2);
        yield return new WaitForSeconds(2f);
        Popup_Result.SetActive(true);
        if (Success)
        {
            SoundManager.Instance.PlaySound("Positive_3");
            SoundManager.Instance.PlaySound("Special&Powerup_35");
            Popup_Result.transform.GetChild(1).GetChild(0).GetComponent<Text>().text
                = "수익: " + Define.Math.RESULT().ToString("N0");
            Popup_Result.transform.GetChild(2).GetChild(0).GetComponent<Text>().text
                = "관객수: " + (GameManager.Instance.Play_Marketing * 10).ToString("N0");
        }
        else
        {
            SoundManager.Instance.PlaySound("Negative_6");
            Popup_Result.transform.GetChild(1).GetChild(0).GetComponent<Text>().text
                = "수익: " + (Define.Math.RESULT() / 2).ToString("N0");
            Popup_Result.transform.GetChild(2).GetChild(0).GetComponent<Text>().text
                = "관객수: " + ((GameManager.Instance.Play_Marketing / 10) * 20).ToString("N0");
        }
    }

    public void Illust()
    {
        if (GameManager.Instance.Play_Success >= 90) //일정 성공률 이상이면 일러스트 해금
            GameManager.Instance.ScenarioIllust[GameManager.Instance.NowScenario.No - 1] = true;
        else 
            To_Ingame();

        BlackBG.SetActive(true);
        Popup_Result.SetActive(false);
        Popup_Illust.SetActive(true);

        Sequence IllustSeq = DOTween.Sequence();

        IllustSeq.Append(Popup_Illust.transform.GetChild(1).DOScale(new Vector3(0.7f, 0.7f), 0.3f));
        IllustSeq.AppendCallback(() => Popup_Illust.transform.GetChild(2).GetComponent<UIParticle>().Play());
        IllustSeq.AppendCallback(() => Popup_Illust.transform.GetChild(1).GetComponent<Image>().sprite 
        = Illusts[GameManager.Instance.NowScenario.No - 1]);
        IllustSeq.Append(Popup_Illust.transform.GetChild(1).DOScale(new Vector3(1f, 1f), 0.3f));
        IllustSeq.AppendCallback(() => Popup_Illust.transform.GetChild(3).GetComponent<UIParticle>().Play());
        IllustSeq.AppendCallback(() => Popup_Illust.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "<bounce>일러스트 획득!</bounce>");
    }

    public void To_Ingame()
    {
        Destroy(Bgm);
        SoundManager.Instance.PlayBGM();

        GameManager.Instance.CostMoney(
            (Success) ? (int)Define.Math.RESULT() : (int)Define.Math.RESULT() / 2, false);
        if (GameManager.Instance.Money < 0) //파산
            GameManager.Instance.Is_Bankrupt(true);

        GameManager.Instance.Reset();
        LoadManager.LoaderCallback();
        LoadManager.Load(LoadManager.Scene.Ingame);
    }
}
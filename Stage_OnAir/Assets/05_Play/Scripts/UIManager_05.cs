using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using Coffee.UIExtensions;

public class UIManager_05 : MonoBehaviour
{
    private bool Success = false;
    private long ResultMoney;

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
    public TutorialScript TutorialObj;

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

        Character1.sprite = ActorData.Instance.ActorImage[GameManager.Instance.Actors[1].Sprite-1];
        Character2.sprite = ActorData.Instance.ActorImage[GameManager.Instance.Actors[2].Sprite-1];

        DelayCrt = StartDelay();
        StartCoroutine(DelayCrt);

        if (GameManager.Instance.Tutorial == true)
        {
            TutorialObj = GameObject.Find("TutorialObj").GetComponent<TutorialScript>();
            TutorialObj.Tutorial();
        }
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
        // 패키지 사용 중일 경우, 성공 확정
        if (GameManager.Instance.OnPackage == true && GameManager.Instance.UsePackage == true)
            Success = true;
        //성공여부 판단
        else
        {
            float Rand = UnityEngine.Random.Range(0f, 100f);
            Success = (Rand <= GameManager.Instance.Play_Success);
        }

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

        ResultMoney = Success ? Define.Math.RESULT() : (Define.Math.RESULT() / 2);

        // 패키지 사용중일 경우, 보상 금액 2배
        if (GameManager.Instance.OnPackage == true && GameManager.Instance.UsePackage == true)
            ResultMoney *= 2;

        if (Success)
        {
            SoundManager.Instance.PlaySound("Positive_3");
            SoundManager.Instance.PlaySound("Special&Powerup_35");
            Popup_Result.transform.GetChild(2).GetChild(0).GetComponent<Text>().text
                = "수익: " + ResultMoney.ToString("N0");
            Popup_Result.transform.GetChild(3).GetChild(0).GetComponent<Text>().text
                = "관객수: " + Define.Math.AUDIENCE().ToString("N0");

            GameManager.Instance.CheckBestScore(
                GameManager.Instance.Play_Quality,
                Define.Math.AUDIENCE(),
                ResultMoney); // 연극의 최종 결과의 값을 가지고 최고기록을 갱신하는 함수 
        }
        else if (Success && GameManager.Instance.OnPackage == true && GameManager.Instance.UsePackage == true)
        {
            SoundManager.Instance.PlaySound("Positive_3");
            SoundManager.Instance.PlaySound("Special&Powerup_35");
            Popup_Result.transform.GetChild(2).GetChild(0).GetComponent<Text>().text
                = "수익: " + (ResultMoney/2).ToString("N0") + "<color=#ff0000><bounce>X 2</bounce></color>";
            Popup_Result.transform.GetChild(3).GetChild(0).GetComponent<Text>().text
                = "관객수: " + Define.Math.AUDIENCE().ToString("N0");

            Debug.Log("스타트 패키지 적용 결과");
        }
        else
        {
            SoundManager.Instance.PlaySound("Negative_6");
            Popup_Result.transform.GetChild(2).GetChild(0).GetComponent<Text>().text
                = "수익: " + ResultMoney.ToString("N0");
            Popup_Result.transform.GetChild(3).GetChild(0).GetComponent<Text>().text
                = "관객수: " + (Define.Math.AUDIENCE() / 10).ToString("N0");
        }
    }

    public void Illust()
    {
        if (!(GameManager.Instance.OnPackage == true && GameManager.Instance.UsePackage == true) && // 패키지 적용중이 아니면서
            Define.Math.FINALQUALITY() >= GameManager.Instance.NowScenario.Mission // 퀄리티가 시나리오의 조건값 이상이고
            && Success)                                                 //공연에 성공했다면 일러스트 해금
            GameManager.Instance.ScenarioIllust[GameManager.Instance.NowScenario.No - 1] = true;
        else
        {
            To_Ingame();
            return;
        }

        BlackBG.SetActive(true);
        Popup_Result.SetActive(false);
        Popup_Illust.SetActive(true);

        Sequence IllustSeq = DOTween.Sequence();

        IllustSeq.Append(Popup_Illust.transform.GetChild(2).DOScale(new Vector3(0.7f, 0.7f), 0.3f));
        IllustSeq.AppendCallback(() => Popup_Illust.transform.GetChild(3).GetComponent<UIParticle>().Play());
        IllustSeq.AppendCallback(() => Popup_Illust.transform.GetChild(2).GetComponent<Image>().sprite
        = Illusts[GameManager.Instance.NowScenario.No - 1]);
        IllustSeq.Append(Popup_Illust.transform.GetChild(2).DOScale(new Vector3(1f, 1f), 0.3f));
        IllustSeq.AppendCallback(() => Popup_Illust.transform.GetChild(4).GetComponent<UIParticle>().Play());
        IllustSeq.AppendCallback(() => Popup_Illust.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "<bounce>일러스트 획득!</bounce>");
    }

    public void To_Ingame()
    {
        Destroy(Bgm);
        SoundManager.Instance.PlayBGM();

        if (GameManager.Instance.OnPackage == true && GameManager.Instance.UsePackage == true)
            GameManager.Instance.UsedStartPackage();

        GameManager.Instance.CostMoney(ResultMoney, false);
        if (GameManager.Instance.Money < 0) //파산
            GameManager.Instance.Is_Bankrupt(true);


        GameManager.Instance.Reset();
        LoadManager.Load(LoadManager.Scene.Ingame);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using LitJson;
using Define;
using DG.Tweening;
using System.Globalization;

public class UIManager_02 : MonoBehaviour
{
    #region Definition
    [Header("Top UI")]
    public Text Text_Money;
    public Text Text_Year;
    public Text Text_Month;
    public GameObject Stat_UI;

    [Header("Bottom UI")]
    public Image Buttom_Progress;
    public Image Gauge_Progress;
    public Sprite[] Image_Progress = new Sprite[4];

    [Header("Popup UI")]
    public GameObject Popup_Black;
    [Space(10)]
    public GameObject Popup_Option;
    public GameObject Popup_Rank;
    [Space(10)]
    public GameObject Popup_Audition;
    public GameObject Popup_Period;
    public GameObject Popup_Prepare;
    [Space(10)]
    public GameObject Popup_Marketing;
    public GameObject Popup_MarketingUp;
    public GameObject Popup_MarketingCk;
    public GameObject Popup_Develop;
    public GameObject Popup_DevelopUp;
    public GameObject Popup_DevelopCk;
    [Space(10)]
    public GameObject Popup_Play;
    [Space(10)]
    public GameObject Popup_Staff;
    public GameObject Popup_StaffUp;
    public GameObject Popup_StaffCk;
    public GameObject Popup_Shop;
    public GameObject Popup_ShopUp;
    public GameObject Popup_ShopCk;
    [Space(10)]
    public GameObject Popup_Warning;
    public GameObject Popup_LoansCk;

    [Header("Period")]
    public Text Text_Period;
    public Text Text_Success;

    [Header("Option")]
    public Toggle BGM;
    public Toggle SFX;
    public Toggle Push;
    public InputField Nickname;
    public Text DefaultSuccess;

    [Header("Ranking")]
    public Text Text_MyRank;
    public GameObject Ranks;
    public GameObject Rank_Button;
    public Toggle[] Rank_Toggle = new Toggle[3];

    RANKING.RANK NowRank = RANKING.RANK.QUALITY;

    [Header("Button")]
    public GameObject Btn_Progress;

    public List<Sprite> Progress_Btn_Sprites;

    [Header("BG")]
    public GameObject Background_1;
    public GameObject Background_2;
    public GameObject Background_3;

    [Header("Error")]
    public GameObject Popup_Error;
    public Text Error_Text;
    private string Error_Message;

    [Header("Anim UI")]
    public GameObject GameOver;
    public GameObject Play;

    public enum PopupList
    {
        Option = 0, //  0
        Rank,       //  1
        Audition,   //  2
        Period,     //  3
        Prepare,    //  4
        Marketing,  //  5
        MarketingUp, //6
        MarketingCk,//  7
        Develop,    //  8
        DevelopUp,  //  9
        DevelopCk,  //  10
        Play,       //  11
        Staff,      //  12
        StaffUp,    //  13
        StaffCk,   //  14
        Shop,       //  15
        ShopUp,  //16
        ShopCk,  //17
        Error,       //  18
        Warning, //   19
        LoansCk   //20
    }

    public delegate void ProgressDel();
    public ProgressDel Progress;

    private int CountMonth = 0;

    #endregion

    private void Start()
    {
        if (GameManager.Instance.IsBankrupt == true)
            StartCoroutine(GameOver_Anim());
        SetProgress();
        if (GameManager.Instance.NowStep == GameManager.Step.Prepare_Play)
        {
            StartCoroutine(StartPrepare());
        }
    }

    private void Update()
    {
        Text_Money.text = GameManager.Instance.Money.ToString("N0");
        if (GameManager.Instance.Money <= 0)
            Text_Money.color = Color.red;
        else
            Text_Money.color = Color.black;
        Text_Year.text = GameManager.Instance.Year.ToString("D2");
        Text_Month.text = GameManager.Instance.Month.ToString("D2");

        string StatText;
        //진행 단계
        StatText = "진행 단계: ";
        switch (GameManager.Instance.NowStep)
        {
            case GameManager.Step.Select_Scenario:
                StatText += "시나리오 선택";
                DOTween.To(() => Gauge_Progress.fillAmount, x => Gauge_Progress.fillAmount = x, 0.2f, 1);
                break;
            case GameManager.Step.Cast_Actor:
                StatText += "배우 캐스팅";
                DOTween.To(() => Gauge_Progress.fillAmount, x => Gauge_Progress.fillAmount = x, 0.4f, 1);
                break;
            case GameManager.Step.Set_Period:
                StatText += "준비 기간 설정";
                DOTween.To(() => Gauge_Progress.fillAmount, x => Gauge_Progress.fillAmount = x, 0.6f, 1);
                break;
            case GameManager.Step.Prepare_Play:
                StatText += "공연 준비";
                DOTween.To(() => Gauge_Progress.fillAmount, x => Gauge_Progress.fillAmount = x, 0.8f, 1);
                break;
            case GameManager.Step.Start_Play:
                StatText += "연극 공연 개시";
                DOTween.To(() => Gauge_Progress.fillAmount, x => Gauge_Progress.fillAmount = x, 1.0f, 1);
                break;
            default:
                break;
        }
        //시나리오, 배우
        if (GameManager.Instance.NowStep == GameManager.Step.Select_Scenario)
            StatText += "\n시나리오 이름: 없음" + "\n배우: 없음";
        else
        {
            StatText += "\n시나리오 이름: " + GameManager.Instance.NowScenario.Name
            + "\n배우: " + GameManager.Instance.NowActor + " / " + GameManager.Instance.MaxActor;
        }
        //준비 기간
        if (GameManager.Instance.NowStep > GameManager.Step.Set_Period)
            StatText += "\n준비 기간: " + GameManager.Instance.Period + "개월";
        else
            StatText += "\n준비 기간: 없음";
        //퀄리티, 마케팅, 성공률
        StatText += "\n\n퀄리티: " + (Math.FINALQUALITY()).ToString("N0")
            + "\n마케팅: " + GameManager.Instance.Play_Marketing.ToString("N0")
            + "\n성공률: " + GameManager.Instance.Play_Success + "%";

        Stat_UI.transform.GetChild(1).GetComponent<Text>().text = StatText;
        //Debug.LogError(Math.FINALQUALITY());

        SetProgress();
    }

    #region Rank
    public void SetRankKind(int Code)
    {
        NowRank = (RANKING.RANK)Code;
        for (int i = 0; i < Ranks.transform.childCount; i++)
        {
            Ranks.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < 3; i++)
        {
            RectTransform trans = Rank_Toggle[i].gameObject.GetComponent<RectTransform>();
            RectTransform back = Rank_Toggle[i].transform.Find("Background").GetComponent<RectTransform>();

            if (Rank_Toggle[i].isOn)
            {
                trans.sizeDelta = RANKING.SELECT_BT;
                back.sizeDelta = RANKING.SELECT_BT;
            }
            else
            {
                trans.sizeDelta = RANKING.NONSELECT_BT;
                back.sizeDelta = RANKING.NONSELECT_BT;
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)Rank_Button.transform);
        SetRank();
    }

    public void SetRank()
    {
        SoundManager.Instance.PlaySound("Pop_6");
        JsonData Rank = new JsonData();
        JsonData MyRank = new JsonData();

        switch (NowRank)
        {
            case RANKING.RANK.QUALITY:
                Rank = Backend.Rank.GetRankByUuid(RANKING.QUALITY_UUID).GetReturnValuetoJSON()["rows"];
                MyRank = Backend.Rank.GetMyRank(RANKING.QUALITY_UUID).GetReturnValuetoJSON()["rows"];
                break;
            case RANKING.RANK.AUDIENCE:
                Rank = Backend.Rank.GetRankByUuid(RANKING.AUDIENCE_UUID).GetReturnValuetoJSON()["rows"];
                MyRank = Backend.Rank.GetMyRank(RANKING.AUDIENCE_UUID).GetReturnValuetoJSON()["rows"];
                break;
            case RANKING.RANK.PROFIT:
                Rank = Backend.Rank.GetRankByUuid(RANKING.PROFIT_UUID).GetReturnValuetoJSON()["rows"];
                MyRank = Backend.Rank.GetMyRank(RANKING.PROFIT_UUID).GetReturnValuetoJSON()["rows"];
                break;
        }

        int Count;

        if (Rank.Count >= 50)
            Count = 50;
        else
            Count = Rank.Count;

        for (int i = 0; i < Count; i++)
        {
            GameObject Text = ObjManager.SpawnPool("RankText", Vector3.zero, Quaternion.Euler(0, 0, 0));

            string rank = int.Parse(Rank[i]["rank"]["N"].ToString()).ToString("D2");
            string nickname = Rank[i]["nickname"]["S"].ToString();
            string score = Rank[i]["score"]["N"].ToString();

            Text.GetComponent<RectTransform>().localScale = Vector3.one;
            Text.GetComponent<Text>().text = rank + "." + nickname + " : " + score;
        }

        string Myrank = int.Parse(MyRank[0]["rank"]["N"].ToString()).ToString("D2");
        string Mynickname = MyRank[0]["nickname"]["S"].ToString();
        string Myscore = MyRank[0]["score"]["N"].ToString();

        Text_MyRank.text = Myrank + "." + Mynickname + " : " + Myscore;
    }

    #endregion

    #region Popup
    public void Popup_On(int Popup)
    {
        SoundManager.Instance.PlaySound("Pop_6");
        PopupList Select = (PopupList)Popup;

        Popup_Black.SetActive(true);

        switch (Select)
        {
            case PopupList.Option:
                Popup_Black.SetActive(false);
                Popup_Option.SetActive(true);
                OnOption();
                break;
            case PopupList.Rank:
                SetRankKind((int)RANKING.RANK.QUALITY);
                Popup_Rank.SetActive(true);
                break;
            case PopupList.Audition:
                Popup_Audition.SetActive(true);
                break;
            case PopupList.Period:
                Popup_Period.SetActive(true);
                break;
            case PopupList.Prepare:
                Popup_Prepare.SetActive(true);
                break;
            case PopupList.Marketing:
                SetMarketingItem();
                Popup_Marketing.SetActive(true);
                break;
            case PopupList.MarketingUp:
                Popup_MarketingUp.SetActive(true);
                break;
            case PopupList.MarketingCk:
                Popup_MarketingCk.SetActive(true);
                break;
            case PopupList.Develop:
                SetDevelopItem();
                Popup_Develop.SetActive(true);
                break;
            case PopupList.DevelopUp:
                Popup_DevelopUp.SetActive(true);
                break;
            case PopupList.DevelopCk:
                Popup_DevelopCk.SetActive(true);
                break;
            case PopupList.Play:
                Popup_Play.SetActive(true);
                break;
            case PopupList.Staff:
                SetStaffItem();
                Popup_Staff.SetActive(true);
                break;
            case PopupList.StaffUp:
                Popup_StaffUp.SetActive(true);
                Close_Item(Popup_Staff);
                break;
            case PopupList.StaffCk:
                Popup_StaffCk.SetActive(true);
                break;
            case PopupList.Shop:
                SetShopItem();
                Popup_Shop.SetActive(true);
                break;
            case PopupList.ShopUp:
                Popup_ShopUp.SetActive(true);
                break;
            case PopupList.ShopCk:
                Popup_ShopCk.SetActive(true);
                break;
            case PopupList.Error:
                Error_Text.text = Error_Message;
                Popup_Error.SetActive(true);
                break;
            case PopupList.Warning:
                Popup_Warning.SetActive(true);
                break;
            case PopupList.LoansCk:
                Popup_LoansCk.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void Popup_Quit()
    {
        SoundManager.Instance.PlaySound("Pop_3");
        Popup_Black.SetActive(false);

        Popup_Option.SetActive(false);
        Popup_Rank.SetActive(false);

        Popup_Audition.SetActive(false);
        Popup_Period.SetActive(false);
        Popup_Prepare.SetActive(false);
        Popup_Marketing.SetActive(false);
        Popup_MarketingUp.SetActive(false);
        Popup_MarketingCk.SetActive(false);
        Popup_Develop.SetActive(false);
        Popup_DevelopUp.SetActive(false);
        Popup_DevelopCk.SetActive(false);
        Popup_Play.SetActive(false);

        Popup_Staff.SetActive(false);
        Popup_StaffUp.SetActive(false);
        Popup_StaffCk.SetActive(false);
        Popup_Shop.SetActive(false);
        Popup_ShopUp.SetActive(false);
        Popup_ShopCk.SetActive(false);

        Popup_Error.SetActive(false);
        Popup_Warning.SetActive(false);
        Popup_LoansCk.SetActive(false);

        Close_Item(Popup_Shop);
        Close_Item(Popup_Staff);
        Close_Item(Popup_Marketing);
        Close_Item(Popup_Develop);
    }

    public void Popup_Quit(int Popup)
    {
        SoundManager.Instance.PlaySound("Pop_3");
        PopupList Select = (PopupList)Popup;

        switch (Select)
        {
            case PopupList.Option:
                Popup_Option.SetActive(false);
                break;
            case PopupList.Rank:
                Popup_Rank.SetActive(false);
                break;
            case PopupList.Audition:
                Popup_Audition.SetActive(false);
                break;
            case PopupList.Period:
                Popup_Period.SetActive(false);
                break;
            case PopupList.Prepare:
                Popup_Prepare.SetActive(false);
                break;
            case PopupList.Marketing:
                Close_Item(Popup_Marketing);
                Popup_Marketing.SetActive(false);
                break;
            case PopupList.MarketingUp:
                Popup_MarketingUp.SetActive(false);
                break;
            case PopupList.MarketingCk:
                Popup_MarketingCk.SetActive(false);
                break;
            case PopupList.Develop:
                Close_Item(Popup_Develop);
                Popup_Develop.SetActive(false);
                break;
            case PopupList.DevelopUp:
                Popup_DevelopUp.SetActive(false);
                break;
            case PopupList.DevelopCk:
                Popup_DevelopCk.SetActive(false);
                break;
            case PopupList.Play:
                Popup_Play.SetActive(false);
                break;
            case PopupList.Staff:
                Close_Item(Popup_Staff);
                Popup_Staff.SetActive(false);
                break;
            case PopupList.StaffUp:
                Popup_Quit(12);
                Popup_On(12);
                Popup_StaffUp.SetActive(false);
                break;
            case PopupList.StaffCk:
                Popup_StaffCk.SetActive(false);
                break;
            case PopupList.Shop:
                Close_Item(Popup_Shop);
                Popup_Shop.SetActive(false);
                break;
            case PopupList.ShopUp:
                Popup_ShopUp.SetActive(false);
                break;
            case PopupList.ShopCk:
                Popup_ShopCk.SetActive(false);
                break;
            case PopupList.Error:
                Popup_Error.SetActive(false);
                break;
            case PopupList.Warning:
                Popup_Warning.SetActive(false);
                break;
            case PopupList.LoansCk:
                Popup_LoansCk.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void Load_Illust()
    {
        SoundManager.Instance.PlaySound("Pop_6");
        DOTween.PauseAll();
        LoadManager.Load(LoadManager.Scene.Illust);
    }

    public void Control_Error(bool Open)
    {
        Popup_Black.SetActive(Open);
        Popup_Error.SetActive(Open);
        Error_Text.text = Error_Message;
    }
    #endregion

    #region Progress
    public void SetProgress()
    {
        GameManager.Step NowStep = GameManager.Instance.NowStep;

        switch (NowStep)
        {
            case GameManager.Step.Select_Scenario:
                Progress = () =>
                {
                    SoundManager.Instance.PlaySound("Prize_Wheel_Spin_2_Reward");
                    DOTween.PauseAll();
                    LoadManager.Load(LoadManager.Scene.Scenario);
                };
                Buttom_Progress.sprite = Image_Progress[0];
                Background_3.SetActive(false);
                Background_1.SetActive(true);
                break;
            case GameManager.Step.Cast_Actor:
                Progress = () =>
                {
                    Popup_On((int)PopupList.Audition);
                };
                Buttom_Progress.sprite = Image_Progress[1];
                Background_1.SetActive(false);
                Background_2.SetActive(true);
                break;
            case GameManager.Step.Set_Period:
                Progress = () =>
                {
                    Popup_On((int)PopupList.Period);
                    GameManager.Instance.SetDefaultPeriod();
                    Set_Period_Text();
                };
                Buttom_Progress.sprite = Image_Progress[2];
                Background_1.SetActive(false);
                Background_2.SetActive(true);
                break;
            case GameManager.Step.Prepare_Play:
                Progress = () =>
                {
                    Popup_On((int)PopupList.Prepare);
                    Popup_Prepare.transform.Find("Play BT").GetComponent<Button>().interactable = false;
                };
                Buttom_Progress.sprite = Image_Progress[3];
                break;
            case GameManager.Step.Start_Play:
                Progress = () =>
                {
                    Popup_On((int)PopupList.Prepare);
                    Popup_Prepare.transform.Find("Play BT").GetComponent<Button>().interactable = true;
                };
                Btn_Progress.GetComponent<Image>().sprite = Progress_Btn_Sprites[3];
                Background_2.SetActive(false);

                Background_3.GetComponent<SpriteRenderer>().sprite =
                    ScenarioData.Instance.scenarioBGs[GameManager.Instance.NowScenario.Code - 1].Ingame;
                Background_3.SetActive(true);
                break;
            default:
                break;
        }

        Btn_Progress.GetComponent<Button>().onClick.RemoveAllListeners();
        Btn_Progress.GetComponent<Button>().onClick.AddListener(delegate { Progress(); });
    }

    public void Audition_PayChk()
    {
        if (GameManager.Instance.Money < AUDITION.AUDITION_PRICE)
        {
            Popup_On(20);
            Popup_LoansCk.transform.GetChild(2).GetComponent<Text>().text = "필요금액: " +
                ((GameManager.Instance.Money <= 0) ?
                        AUDITION.AUDITION_PRICE.ToString("N0")
                        : ((GameManager.Instance.Money - AUDITION.AUDITION_PRICE) * -1).ToString("N0"));

            Popup_LoansCk.transform.GetChild(4).GetComponent<Button>().onClick.RemoveAllListeners();
            Popup_LoansCk.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => Progress_Audition());
            Popup_LoansCk.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => Popup_Quit(20));
        }
        else
            Progress_Audition();
    }

    public void Progress_Audition()
    {
        SoundManager.Instance.PlaySound("Prize_Wheel_Spin_2_Reward");
        GameManager.Instance.CostMoney(AUDITION.AUDITION_PRICE);
        DOTween.PauseAll();
        LoadManager.Load(LoadManager.Scene.Audition);
    }

    public void Progress_Period()
    {
        GameManager.Instance.SetPeriod();
        SetProgress();
        CountMonth = 0;
        StartCoroutine(StartPrepare());
        Popup_Quit();
    }

    public void Progress_Play()
    {
        SoundManager.Instance.PlaySound("Prize_Wheel_Spin_2_Reward");
        DOTween.PauseAll();
        LoadManager.Load(LoadManager.Scene.Play);
    }

    public void Play_Popup()
    {
        Popup_Play.transform.GetChild(2).GetChild(0).GetComponent<Text>().text
            = "퀄리티 : " + Define.Math.FINALQUALITY().ToString("N0");
        Popup_Play.transform.GetChild(3).GetChild(0).GetComponent<Text>().text
            = "마케팅 : " + GameManager.Instance.Play_Marketing.ToString("N0");
        Popup_Play.transform.GetChild(4).GetChild(0).GetComponent<Text>().text
            = "성공률 : " + GameManager.Instance.Play_Success + "%";
        Popup_On(11);
    }
    #endregion

    #region Period
    public void Click_Period(int value)
    {
        SoundManager.Instance.PlaySound("Pop_6");
        if (GameManager.Instance.Period + value > 0)
        {
            GameManager.Instance.SetPeriod(value);
            Set_Period_Text();
        }
    }

    private void Set_Period_Text()
    {
        Text_Period.text = GameManager.Instance.Period + "개월";
        Text_Success.text = GameManager.Instance.GetSuccess() + "%";
    }
    #endregion

    private IEnumerator StartPrepare()
    {
        Debug.Log("개발 시작");
        yield return new WaitForSeconds(10);
        GameManager.Instance.GoNextMonth();
        CountMonth++;

        if (CountMonth == GameManager.Instance.Period)
        {
            GameManager.Instance.SetStep(GameManager.Step.Start_Play);
            CountMonth = 0;
            SetProgress();
            StartCoroutine(Play_Anim());
            Debug.Log("개발 완료");
        }
        else
        {
            StartCoroutine(StartPrepare());
            Debug.Log("한달 개발함");
        }
    }

    #region Option

    public void OnOption()
    {
        BGM.isOn = GameManager.Instance.OnBGM;
        SFX.isOn = GameManager.Instance.OnSFX;
        Push.isOn = GameManager.Instance.OnPush;

        Nickname.text = Backend.BMember.GetUserInfo().GetReturnValuetoJSON()["row"]["nickname"].ToString();

        DefaultSuccess.text = GameManager.Instance.DefaultSuccess.ToString() + "%";
    }

    public void SetBGM()
    {
        GameManager.Instance.OnBGM = BGM.isOn;
        SoundManager.Instance.SetBGM(GameManager.Instance.OnBGM ? 1f : 0f);
        SoundManager.Instance.PlaySound("Pop_6");
    }

    public void SetSFX()
    {
        GameManager.Instance.OnSFX = SFX.isOn;
        SoundManager.Instance.SetSFX(GameManager.Instance.OnSFX ? 1f : 0f);
        SoundManager.Instance.PlaySound("Pop_6");
    }

    public void SetPush()
    {
        SoundManager.Instance.PlaySound("Pop_6");
        GameManager.Instance.OnPush = Push.isOn;
    }

    public void SaveData()
    {
        SoundManager.Instance.PlaySound("Pop_6");
    }

    public void ChangeNickname()
    {
        BackendReturnObject bro = Backend.BMember.CheckNicknameDuplication(Nickname.text);

        switch (bro.GetStatusCode())
        {
            case "204":
                Backend.BMember.UpdateNickname(Nickname.text);
                break;
            case "400":
                switch (bro.GetErrorCode())
                {
                    case "UndefinedParameterException":
                        Error_Message = ERROR_MESSAGE.SETNICK_EMPTY;
                        Control_Error(true);
                        break;
                    case "BadParameterException":
                        Error_Message = ERROR_MESSAGE.SETNICK_BAD;
                        Control_Error(true);
                        break;
                }
                break;
            case "409":
                Error_Message = ERROR_MESSAGE.SETNICK_DUPLICATE;
                Control_Error(true);
                break;
        }

    }

    public void Regulate_DefaultSuccess(int value)
    {
        SoundManager.Instance.PlaySound("Pop_6");
        GameManager.Instance.DefaultSuccess += value;

        if (GameManager.Instance.DefaultSuccess > 100)
            GameManager.Instance.DefaultSuccess = 100;

        if (GameManager.Instance.DefaultSuccess < 0)
            GameManager.Instance.DefaultSuccess = 0;

        DefaultSuccess.text = GameManager.Instance.DefaultSuccess.ToString() + "%";
    }
    #endregion

    #region Item
    public void SetMarketingItem()
    {
        for (int i = 0; i < Items.Instance.MarketingItems.Count; i++)
        {
            GameObject item = ObjManager.SpawnPool("MarketingItem", Vector3.zero, Quaternion.Euler(0, 0, 0));

            item.transform.GetChild(0).GetComponent<Image>().sprite = Items.Instance.MarketingItems[i].Icon;
            item.transform.GetChild(1).GetComponent<Text>().text = Items.Instance.MarketingItems[i].name;
            item.transform.GetChild(2).GetComponent<Text>().text = "비용: " + Items.Instance.MarketingItems[i].pay.ToString("N0")
                + "\n점수: +" + Items.Instance.MarketingItems[i].score.ToString("N0");
            int j = i;
            item.transform.GetComponent<Button>().onClick.AddListener(() => Open_Item_Popup("Marketing", j));
        }
        double count = (Items.Instance.MarketingItems.Count / 2f);
        Popup_Marketing.transform.GetChild(2).GetChild(0).GetComponent<RectTransform>().sizeDelta =
            new Vector2(690f, (float)(System.Math.Ceiling(count) * 450) + 50);
    }

    public void SetDevelopItem()
    {
        for (int i = 0; i < Items.Instance.DevelopItems.Count; i++)
        {
            GameObject item = ObjManager.SpawnPool("DevelopItem", Vector3.zero, Quaternion.Euler(0, 0, 0));

            item.transform.GetChild(0).GetComponent<Image>().sprite = Items.Instance.DevelopItems[i].Icon;
            item.transform.GetChild(1).GetComponent<Text>().text = Items.Instance.DevelopItems[i].name;
            int j = i;
            item.transform.GetComponent<Button>().onClick.AddListener(() => Open_Item_Popup("Develop", j));
        }
        double count = (Items.Instance.DevelopItems.Count / 2f);
        Popup_Develop.transform.GetChild(2).GetChild(0).GetComponent<RectTransform>().sizeDelta =
            new Vector2(690f, (float)(System.Math.Ceiling(count) * 450) + 50);
    }

    public void SetStaffItem()
    {
        for (int i = 0; i < Items.Instance.StaffItems.Count; i++)
        {
            GameObject item = ObjManager.SpawnPool("StaffItem", Vector3.zero, Quaternion.Euler(0, 0, 0));

            item.transform.GetChild(0).GetComponent<Image>().sprite = Items.Instance.Staff_Icons[i];
            item.transform.GetChild(1).GetComponent<Text>().text = Items.Instance.StaffItems[i].name;
            int j = i;
            item.transform.GetComponent<Button>().onClick.AddListener(() => Open_Item_Popup("Staff", j));
        }
        double count = Items.Instance.StaffItems.Count / 2f;
        Popup_Staff.transform.GetChild(2).GetChild(0).GetComponent<RectTransform>().sizeDelta =
            new Vector2(690f, (float)(System.Math.Ceiling(count) * 450) + 50);
    }

    public void SetShopItem()
    {
        for (int i = 0; i < Items.Instance.ShopItems.Count; i++)
        {
            GameObject item = ObjManager.SpawnPool("ShopItem", Vector3.zero, Quaternion.Euler(0, 0, 0));

            item.transform.GetChild(0).GetComponent<Image>().sprite = Items.Instance.ShopItems[i].Icon;
            item.transform.GetChild(1).GetComponent<Text>().text = Items.Instance.ShopItems[i].name;
            item.transform.GetChild(2).GetComponent<Text>().text = "비용: " + Items.Instance.ShopItems[i].pay.ToString("N0")
                + "\n점수: +" + Items.Instance.ShopItems[i].score.ToString("N0");
            int j = i;
            item.transform.GetComponent<Button>().onClick.AddListener(() => Open_Item_Popup("Shop", j));
        }
        double count = (Items.Instance.ShopItems.Count / 2f);
        Popup_Shop.transform.GetChild(2).GetChild(0).GetComponent<RectTransform>().sizeDelta =
            new Vector2(690f, (float)(System.Math.Ceiling(count) * 450) + 50);
    }

    public void Open_Item_Popup(string sort, int num)
    {
        GameObject obj;
        Sprite Icon;
        string Name;
        string Script;
        string Pay;

        if (sort == "Marketing")
        {
            Popup_On(6);
            obj = Popup_MarketingUp;
            Icon = Items.Instance.MarketingItems[num].Icon;
            Name = Items.Instance.MarketingItems[num].name;
            Script = "마케팅점수: +" + Items.Instance.MarketingItems[num].score.ToString("N0");
            Pay = "가격: " + Items.Instance.MarketingItems[num].pay.ToString("N0");

        }
        else if (sort == "Develop")
        {
            Popup_On(9);
            obj = Popup_DevelopUp;
            Icon = Items.Instance.DevelopItems[num].Icon;
            Name = Items.Instance.DevelopItems[num].name;
            Script = Items.Instance.DevelopItems[num].script;
            Pay = "가격: " + Items.Instance.DevelopItems[num].pay.ToString("N0");
        }
        else if (sort == "Staff")
        {
            Popup_On(13);
            obj = Popup_StaffUp;
            Icon = Items.Instance.Staff_Icons[num];
            Name = Items.Instance.StaffItems[num].name;
            if (GameManager.Instance.StaffLevel[num] == 0)
            {
                Script = "월급: " + Items.Instance.StaffItems[num].pay
                    + "\n개발력" + Items.Instance.StaffItems[num].directing;
                Pay = "가격: " + Items.Instance.StaffItems[num].cost_purchass.ToString("N0");
                obj.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = "구매";
            }
            else
            {
                Script = "월급: " + Items.Instance.Staff_MathPay("Pay", num, GameManager.Instance.StaffLevel[num]).ToString("N0")
                    + "\n -> " + Items.Instance.Staff_MathPay("Pay", num, GameManager.Instance.StaffLevel[num] + 1).ToString("N0")
                    + "\n개발력" + Items.Instance.Staff_MathPay("Directing", num, GameManager.Instance.StaffLevel[num]).ToString("N0")
                    + "\n -> " + Items.Instance.Staff_MathPay("Directing", num, GameManager.Instance.StaffLevel[num] + 1).ToString("N0");
                Pay = "가격: " + Items.Instance.Staff_MathPay("Cost", num, GameManager.Instance.StaffLevel[num]).ToString("N0");
                obj.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = "업그레이드";
            }
        }
        else if (sort == "Shop")
        {
            Popup_On(16);
            obj = Popup_ShopUp;
            Icon = Items.Instance.ShopItems[num].Icon;
            Name = Items.Instance.ShopItems[num].name;
            Script = "점수: +" + Items.Instance.ShopItems[num].score.ToString("N0");
            Pay = "가격: " + Items.Instance.ShopItems[num].pay.ToString("N0");
        }
        else
        {
            obj = new GameObject();
            Icon = null;
            Name = null;
            Script = null;
            Pay = null;
        }

        obj.transform.GetChild(5).GetComponent<Button>().onClick.RemoveAllListeners();
        obj.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(() => Loans_Item(sort, num));

        obj.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = Icon;
        obj.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = Name;
        obj.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = Script;
        obj.transform.GetChild(5).GetChild(1).GetComponent<Text>().text = Pay;
    }

    public void Loans_Item(string sort, int num)
    {
        if (sort == "Marketing" && (GameManager.Instance.Money < Items.Instance.MarketingItems[num].pay))
        {
            Popup_On(20);
            Popup_LoansCk.transform.GetChild(2).GetComponent<Text>().text = "필요금액: " +
                ((GameManager.Instance.Money <= 0) ?
                        Items.Instance.MarketingItems[num].pay.ToString("N0")
                        : ((GameManager.Instance.Money - Items.Instance.MarketingItems[num].pay) * -1).ToString("N0"));
        }
        else if (sort == "Staff")
        {
            if (GameManager.Instance.StaffLevel[num] == 0)
            {
                if (GameManager.Instance.Money < Items.Instance.StaffItems[num].cost_purchass)
                {
                    Popup_On(20);
                    Popup_LoansCk.transform.GetChild(2).GetComponent<Text>().text = "필요금액: " +
                        ((GameManager.Instance.Money <= 0) ?
                                Items.Instance.StaffItems[num].cost_purchass.ToString("N0")
                                : ((GameManager.Instance.Money - Items.Instance.StaffItems[num].cost_purchass) * -1).ToString("N0"));
                }
                else Buy_Item(sort, num);
            }
            else
            {
                if (GameManager.Instance.Money < Items.Instance.Staff_MathPay("Cost", num, GameManager.Instance.StaffLevel[num]))
                {
                    Popup_On(20);
                    Popup_LoansCk.transform.GetChild(2).GetComponent<Text>().text = "필요금액: " +
                        ((GameManager.Instance.Money <= 0) ?
                                 Items.Instance.Staff_MathPay("Cost", num, GameManager.Instance.StaffLevel[num]).ToString("N0")
                                : ((GameManager.Instance.Money - Items.Instance.Staff_MathPay("Cost", num, GameManager.Instance.StaffLevel[num])) * -1).ToString("N0"));
                }
                else Buy_Item(sort, num);
            }
        }
        else if (sort == "Shop" && (GameManager.Instance.Money < Items.Instance.ShopItems[num].pay))
        {
            Popup_On(20);
            Popup_LoansCk.transform.GetChild(2).GetComponent<Text>().text = "필요금액: " +
                ((GameManager.Instance.Money <= 0) ?
                        Items.Instance.ShopItems[num].pay.ToString("N0")
                        : ((GameManager.Instance.Money - Items.Instance.ShopItems[num].pay) * -1).ToString("N0"));
        }
        else Buy_Item(sort, num);

        Popup_LoansCk.transform.GetChild(4).GetComponent<Button>().onClick.RemoveAllListeners();
        Popup_LoansCk.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => Buy_Item(sort, num));
        Popup_LoansCk.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => Popup_Quit(20));
    }

    public void Buy_Item(string sort, int num)
    {
        Popup_Quit(20);
        if (sort == "Marketing")
        {
            SoundManager.Instance.PlaySound("Cash_Register");
            Popup_MarketingCk.transform.GetChild(1).GetComponent<Text>().text
                = "『" + Items.Instance.MarketingItems[num].name + "』을\n구매하였습니다.";
            Popup_MarketingCk.transform.GetChild(2).GetComponent<Text>().text
                = "보유금액: " + GameManager.Instance.Money.ToString("N0") + " -> "
                + (GameManager.Instance.Money - Items.Instance.DevelopItems[num].pay).ToString("N0")
                + "\n마케팅 점수: " + (GameManager.Instance.Play_Marketing.ToString("N0")) + " -> "
                + (GameManager.Instance.Play_Marketing + Items.Instance.MarketingItems[num].score).ToString("N0");

            Popup_On(7);
            GameManager.Instance.CostMoney(Items.Instance.MarketingItems[num].pay);
            GameManager.Instance.Plus_Play_Marketing(Items.Instance.MarketingItems[num].score);
            //marketing아이템을 구매했을 때 나타나는 효과.
        }
        else if (sort == "Develop" && GameManager.Instance.Money >= Items.Instance.DevelopItems[num].pay)
        {
            SoundManager.Instance.PlaySound("Cash_Register");
            Popup_DevelopCk.transform.GetChild(1).GetComponent<Text>().text
                = "『" + Items.Instance.DevelopItems[num].name + "』을\n구매하였습니다.";
            Popup_DevelopCk.transform.GetChild(2).GetComponent<Text>().text
                = "보유금액: " + GameManager.Instance.Money.ToString("N0") + " -> "
                + (GameManager.Instance.Money - Items.Instance.DevelopItems[num].pay).ToString("N0");

            Popup_On(10);
            GameManager.Instance.CostMoney(Items.Instance.DevelopItems[num].pay);
            //develop아이템을 구매했을 때 나타나는 효과.
        }
        else if (sort == "Staff")
        {
            SoundManager.Instance.PlaySound("Cash_Register");
            if (GameManager.Instance.StaffLevel[num] == 0)
            {
                Popup_StaffCk.transform.GetChild(1).GetComponent<Text>().text
                = "『" + Items.Instance.StaffItems[num].name + "』을\n구매하였습니다.";
                Popup_StaffCk.transform.GetChild(2).GetComponent<Text>().text
                    = "보유금액: " + GameManager.Instance.Money.ToString("N0") + " -> "
                    + (GameManager.Instance.Money - Items.Instance.StaffItems[num].cost_purchass).ToString("N0")
                    + "\n개발력: " + (GameManager.Instance.Quality_Direction.ToString("N0")) + " -> "
                    + (GameManager.Instance.Quality_Direction + Items.Instance.StaffItems[num].directing).ToString("N0");

                GameManager.Instance.CostMoney(Items.Instance.StaffItems[num].cost_purchass);
                GameManager.Instance.Plus_Quality_Direction(Items.Instance.StaffItems[num].directing);
                Popup_On(14);
            }
            else
            {
                GameManager.Instance.CostMoney(Items.Instance.Staff_MathPay("Cost", num, GameManager.Instance.StaffLevel[num]));
                GameManager.Instance.Plus_Quality_Direction(Items.Instance.StaffItems[num].plus_directing);
            }
            GameManager.Instance.StaffLevel[num]++;
            Open_Item_Popup("Staff", num);
        }
        else if (sort == "Shop")
        {
            SoundManager.Instance.PlaySound("Cash_Register");
            Popup_ShopCk.transform.GetChild(1).GetComponent<Text>().text
                = "『" + Items.Instance.ShopItems[num].name + "』을\n구매하였습니다.";
            Popup_ShopCk.transform.GetChild(2).GetComponent<Text>().text
                = "보유금액: " + GameManager.Instance.Money.ToString("N0") + " -> "
                + (GameManager.Instance.Money - Items.Instance.ShopItems[num].pay).ToString("N0");
            //+ "\n점수: " + (GameManager.Instance.ShopItems.ToString("N0")) + " -> "
            //+ (GameManager.Instance.ShopItems + Items.Instance.MarketingItems[num].score).ToString("N0");

            Popup_On(17);
            GameManager.Instance.CostMoney(Items.Instance.ShopItems[num].pay);
            //Shop아이템을 구매했을 때 나타나는 효과.
        }
        else
        {
            Popup_On(19);
        }
    }

    public void Close_Item(GameObject Obj)
    {
        for (int i = 0; i < Obj.transform.GetChild(2).GetChild(0).childCount; i++)
        {
            Obj.transform.GetChild(2).GetChild(0).GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            Obj.transform.GetChild(2).GetChild(0).GetChild(i).gameObject.SetActive(false);
        }
    }
    #endregion

    public void Stat_UI_Anim()
    {
        SoundManager.Instance.PlaySound("Pop_6");
        if (Stat_UI.transform.position.x >= -700)
            Stat_UI.transform.DOLocalMoveX(-720f, 0.3f).SetEase(Ease.OutBack);
        else
            Stat_UI.transform.DOLocalMoveX(-290f, 0.3f).SetEase(Ease.OutBack);
    }

    public void ReStart()
    {
        SoundManager.Instance.PlaySound("Pop_6");
        GameManager.Instance.ReStart();

        GameOver.SetActive(false);
        GameOver.transform.GetChild(1).GetComponent<Text>().color = new Color(255, 255, 255, 0);
        GameOver.transform.GetChild(3).GetComponent<Text>().color = new Color(255, 255, 255, 0);
        GameOver.transform.GetChild(4).GetComponent<Button>().image.color = new Color(255, 255, 255, 0);
        GameOver.transform.GetChild(4).GetChild(0).GetComponent<Text>().color = new Color(255, 255, 255, 0);
    }

    public IEnumerator GameOver_Anim()
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlaySound("Negative_6");
        GameOver.SetActive(true);
        GameOver.transform.GetChild(0).GetComponent<Image>().DOFade(1, 1f);
        GameOver.transform.GetChild(2).DOScale(3, 0.7f).From().SetEase(Ease.OutExpo);
        yield return new WaitForSeconds(0.5f);
        GameOver.transform.GetChild(1).GetComponent<Text>().DOFade(1, 0.5f);
        GameOver.transform.GetChild(3).GetComponent<Text>().DOFade(1, 0.5f);
        GameOver.transform.GetChild(4).GetComponent<Button>().image.DOFade(1, 0.5f);
        GameOver.transform.GetChild(4).GetChild(0).GetComponent<Text>().DOFade(1, 0.5f);
    }

    public IEnumerator Play_Anim()
    {
        Popup_Quit();
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlaySound("Positive_6");
        Play.SetActive(true);
        Play.transform.GetChild(1).DOLocalMoveY(-35, 0.5f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(0.5f);
        Play.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0.5f);
        yield return new WaitForSeconds(1.5f);
        Play.transform.GetChild(1).DOLocalMoveY(2500, 0.5f);
        Play.transform.GetChild(2).DOLocalMoveY(-500, 0.5f);
        yield return new WaitForSeconds(2.5f);
        Play.transform.GetChild(2).DOLocalMoveY(-1500, 0.5f);
        yield return new WaitForSeconds(0.5f);

        Play.SetActive(false);
        SoundManager.Instance.PlayBGM();

        Play.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }

}

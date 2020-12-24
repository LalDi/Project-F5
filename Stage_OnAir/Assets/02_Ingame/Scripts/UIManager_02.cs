using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using LitJson;
using Define;

public class UIManager_02 : MonoBehaviour
{
    [Header("Top UI")]
    public Text Text_Money;
    public Text Text_Year;
    public Text Text_Month;

    [Header("Bottom UI")]
    public Image Buttom_Progress;
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
    public GameObject Popup_Marketing;
    public GameObject Popup_MarketingCk;
    public GameObject Popup_Develop;
    public GameObject Popup_DevelopUp;
    public GameObject Popup_DevelopCk;
    public GameObject Popup_Play;
    [Space(10)]
    public GameObject Popup_Staff;
    public GameObject Popup_StaffUp;
    public GameObject Popup_Shop;
    [Space(10)]
    public GameObject Popup_Warning;

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

    [Space(20)]
    public GameObject StepText;

    public enum PopupList
    {
        Option = 0, //  0
        Rank,       //  1
        Audition,   //  2
        Period,     //  3
        Prepare,    //  4
        Marketing,  //  5
        MarketingCk,//  6
        Develop,    //  7
        DevelopUp,  //  8
        DevelopCk,  //  9
        Play,       //  10
        Staff,      //  11
        StaffUp,    //  12
        Shop,       //  13
        Error,       //  14
        Warning //   15
    }

    public delegate void ProgressDel();
    public ProgressDel Progress;

    private int CountMonth = 0;


    private void Start()
    {
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

        StepText.GetComponent<Text>().text = GameManager.Instance.NowStep.ToString() + 
            "\n" + GameManager.Instance.NowActor + 
            " / " +  GameManager.Instance.MaxActor;

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
                break;
            case PopupList.Shop:
                SetShopItem();
                Popup_Shop.SetActive(true); 
                break;
            case PopupList.Error:
                Error_Text.text = Error_Message;
                Popup_Error.SetActive(true);
                break;
            case PopupList.Warning:
                Popup_Warning.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void Popup_Quit()
    {
        Popup_Black.SetActive(false);

        Popup_Option.SetActive(false);
        Popup_Rank.SetActive(false);

        Popup_Audition.SetActive(false);
        Popup_Period.SetActive(false);
        Popup_Prepare.SetActive(false);
        Popup_Marketing.SetActive(false);
        Popup_MarketingCk.SetActive(false);
        Popup_Develop.SetActive(false);
        Popup_DevelopUp.SetActive(false);
        Popup_DevelopCk.SetActive(false);
        Popup_Play.SetActive(false);

        Popup_Staff.SetActive(false);
        Popup_StaffUp.SetActive(false);
        Popup_Shop.SetActive(false);

        Popup_Error.SetActive(false);
        Popup_Warning.SetActive(false);

        Close_Item(Popup_Shop);
        Close_Item(Popup_Staff);
        Close_Item(Popup_Marketing);
        Close_Item(Popup_Develop);
    }

    public void Popup_Quit(int Popup)
    {
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
                Popup_StaffUp.SetActive(false);
                break;
            case PopupList.Shop:
                Close_Item(Popup_Shop);
                Popup_Shop.SetActive(false);
                break;
            case PopupList.Error:
                Popup_Error.SetActive(false);
                break;
            case PopupList.Warning:
                Popup_Warning.SetActive(false);
                break;
                
            default:
                break;
        }
    }
    public void Load_Illust()
    {
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
        GameManager.Step NowStep = /*GameManager.Step.Select_Scenario*/GameManager.Instance.NowStep;

        switch (NowStep)
        {
            case GameManager.Step.Select_Scenario:
                Progress = () =>
                {
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
                    ScenarioData.Instance.scenarioBGs[GameManager.Instance.NowScenario.Code-1].Ingame;
                Background_3.SetActive(true);
                break;
            default:
                break;
        }

        Btn_Progress.GetComponent<Button>().onClick.RemoveAllListeners();
        Btn_Progress.GetComponent<Button>().onClick.AddListener(delegate { Progress(); } );
    }
    public void Progress_Audition()
    {
        GameManager.Instance.CostMoney(AUDITION.AUDITION_PRICE);
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
        Popup_On(10);
    }
    #endregion

    #region Period
    public void Click_Period(int value)
    {
        GameManager.Instance.SetPeriod(value);
        Set_Period_Text();
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
    }
    
    public void SetSFX()
    {
        GameManager.Instance.OnSFX = SFX.isOn;
    }

    public void SetPush()
    {
        GameManager.Instance.OnPush = Push.isOn;
    }

    public void SaveData()
    {

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
        GameManager.Instance.DefaultSuccess += value;

        if (GameManager.Instance.DefaultSuccess > 100)
            GameManager.Instance.DefaultSuccess = 100;

        if (GameManager.Instance.DefaultSuccess < 0)
            GameManager.Instance.DefaultSuccess = 0;

        DefaultSuccess.text = GameManager.Instance.DefaultSuccess.ToString() + "%";
    }
    #endregion

    #region Shop
    public void SetShopItem()
    {
        for(int i = 0; i < Items.Instance.ShopItems.Count; i++)
        {
            GameObject item = ObjManager.SpawnPool("ShopItem", Vector3.zero, Quaternion.Euler(0, 0, 0));

            item.transform.GetChild(0).GetComponent<Image>().sprite = Items.Instance.ShopItems[i].Icon;
            item.transform.GetChild(1).GetComponent<Text>().text = Items.Instance.ShopItems[i].name;
            item.transform.GetChild(2).GetComponent<Text>().text = "비용: " + Items.Instance.ShopItems[i].pay.ToString("N0")
                + "\n점수: +" + Items.Instance.ShopItems[i].score.ToString("N0");
            int j = i;
            item.transform.GetComponent<Button>().onClick.AddListener(() => Buy_Item("Shop", j));
        }
        double count = (Items.Instance.ShopItems.Count / 2f);
        Popup_Shop.transform.GetChild(2).GetChild(0).GetComponent<RectTransform>().sizeDelta =
            new Vector2(690f, (float)(System.Math.Ceiling(count) * 450) + 50);
    }
    #endregion

    #region Staff
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
    #endregion

    #region Marketing
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
            item.transform.GetComponent<Button>().onClick.AddListener(() => Buy_Item("Marketing", j));
        }
        double count = (Items.Instance.MarketingItems.Count / 2f);
        Popup_Marketing.transform.GetChild(2).GetChild(0).GetComponent<RectTransform>().sizeDelta =
            new Vector2(690f, (float)(System.Math.Ceiling(count) * 450) + 50);
    }
    #endregion

    #region Develop
    public void SetDevelopItem()
    {
        for (int i = 0; i < Items.Instance.DevelopItems.Count; i++)
        {
            GameObject item = ObjManager.SpawnPool("DevelopItem", Vector3.zero, Quaternion.Euler(0, 0, 0));

            item.transform.GetChild(0).GetComponent<Image>().sprite = Items.Instance.DevelopItems[i].Icon;
            item.transform.GetChild(1).GetComponent<Text>().text = Items.Instance.DevelopItems[i].name;
            int j = i;
            item.transform.GetComponent<Button>().onClick.AddListener(() => Buy_Item("Develop", j));
        }
        double count = (Items.Instance.DevelopItems.Count / 2f);
        Popup_Develop.transform.GetChild(2).GetChild(0).GetComponent<RectTransform>().sizeDelta =
            new Vector2(690f, (float)(System.Math.Ceiling(count) * 450) + 50);
    }

    public void Buy_Item(string sort, int num)
    {
        //Debug.Log(GameManager.Instance.Money + " , " + Items.Instance.DevelopItems[num].pay);
        if (sort == "Marketing")
        {
            Popup_MarketingCk.transform.GetChild(1).GetComponent<Text>().text
                = "『" + Items.Instance.MarketingItems[num].name + "』을\n구매하였습니다.";
            Popup_MarketingCk.transform.GetChild(2).GetComponent<Text>().text
                = "보유금액 : " + GameManager.Instance.Money.ToString("N0") + " -> "
                + (GameManager.Instance.Money - Items.Instance.DevelopItems[num].pay).ToString("N0")
                + "\n마케팅 점수 : " + (GameManager.Instance.Play_Marketing.ToString("N0")) + " -> "
                + (GameManager.Instance.Play_Marketing + Items.Instance.MarketingItems[num].score).ToString("N0");

            Popup_On(6);
            GameManager.Instance.CostMoney(Items.Instance.MarketingItems[num].pay);
            GameManager.Instance.CostMarketing(Items.Instance.MarketingItems[num].score, false);
            //marketing아이템을 구매했을 때 나타나는 효과.
        }
        else if (sort == "Develop" && GameManager.Instance.Money >= Items.Instance.DevelopItems[num].pay)
        {
            Popup_DevelopCk.transform.GetChild(1).GetComponent<Text>().text 
                = "『" + Items.Instance.DevelopItems[num].name + "』을\n구매하였습니다.";
            Popup_DevelopCk.transform.GetChild(2).GetComponent<Text>().text
                = "보유금액 : " + GameManager.Instance.Money.ToString("N0") + " -> " 
                + (GameManager.Instance.Money - Items.Instance.DevelopItems[num].pay).ToString("N0");

            Popup_On(9);
            GameManager.Instance.CostMoney(Items.Instance.DevelopItems[num].pay);
            //develop아이템을 구매했을 때 나타나는 효과.
        }
        else if (sort == "Staff")
        {
            GameManager.Instance.CostMoney(Items.Instance.StaffItems[num].pay);
            GameManager.Instance.StaffLevel[num]++;
            Open_Item_Popup("Staff", num);
        }
        else if (sort == "Shop")
        {
            GameManager.Instance.CostMoney(Items.Instance.ShopItems[num].pay);
            //Shop아이템을 구매했을 때 나타나는 효과.
        }
        else
        {
            Popup_On(15);
        }
    }

    public void Open_Item_Popup(string sort, int num)
    {
        GameObject obj;
        Sprite Icon;
        string Name;
        string Script;
        string Pay;

        if (sort == "Develop"){
            Popup_On(8);
            obj = Popup_DevelopUp;
            Icon = Items.Instance.DevelopItems[num].Icon;
            Name = Items.Instance.DevelopItems[num].name;
            Script = Items.Instance.DevelopItems[num].script;
            Pay = "가격: " + Items.Instance.DevelopItems[num].pay.ToString("N0");
            obj.transform.GetChild(5).GetComponent<Button>().onClick.RemoveAllListeners();
            obj.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(() => Buy_Item("Develop", num));
        }
        else if (sort == "Staff"){
            Popup_On(12);
            obj = Popup_StaffUp;
            Icon = Items.Instance.Staff_Icons[num];
            Name = Items.Instance.StaffItems[num].name;
            obj.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = "구매";
            obj.transform.GetChild(5).GetComponent<Button>().onClick.RemoveAllListeners();
            if (GameManager.Instance.StaffLevel[num] == 0)
            {
                Script = "월급: " + Items.Instance.StaffItems[num].pay
                    + "\n개발력" + Items.Instance.StaffItems[num].directing;
                Pay = "가격: " + Items.Instance.StaffItems[num].cost_purchass.ToString("N0");
                obj.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(() => Buy_Item("Staff", num));
            }
            else
            {
                Script = "월급: " + Items.Instance.Staff_MathPay("Pay", num, GameManager.Instance.StaffLevel[num]).ToString("N0")
                    + "\n -> " + Items.Instance.Staff_MathPay("Pay", num, GameManager.Instance.StaffLevel[num] + 1).ToString("N0")
                    + "\n개발력" + Items.Instance.Staff_MathPay("Directing", num, GameManager.Instance.StaffLevel[num]).ToString("N0")
                    + "\n -> " + Items.Instance.Staff_MathPay("Directing", num, GameManager.Instance.StaffLevel[num] + 1).ToString("N0");
                Pay = "가격: " + Items.Instance.Staff_MathPay("Cost", num, GameManager.Instance.StaffLevel[num]).ToString("N0");
                obj.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = "업그레이드";
                obj.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(() => Buy_Item("Staff", num));
            }
        }
        else
        {
            obj = new GameObject();
            Icon = null;
            Name = null;
            Script = null;
            Pay = null;
        }

        obj.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = Icon;
        obj.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = Name;
        obj.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = Script;
        obj.transform.GetChild(5).GetChild(1).GetComponent<Text>().text = Pay;
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
}

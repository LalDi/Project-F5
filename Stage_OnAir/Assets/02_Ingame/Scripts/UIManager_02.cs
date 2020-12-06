using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Define;
using BackEnd;

public class UIManager_02 : MonoBehaviour
{
    [Header("Top UI")]
    public Text Text_Money;
    public Text Text_Year;
    public Text Text_Month;

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

    [Header("Period")]
    public Text Text_Period;
    public Text Text_Success;

    [Header("Option")]
    public Toggle BGM;
    public Toggle SFX;
    public Toggle Push;
    public InputField Nickname;
    public Text DefaultSuccess;

    [Header("Button")]
    public Button Btn_Progress;

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
        Shop        //  13
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
                Popup_Marketing.SetActive(true);
                break;
            case PopupList.MarketingCk:
                Popup_MarketingCk.SetActive(true);
                break;
            case PopupList.Develop:
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
                Popup_Staff.SetActive(true);
                break;
            case PopupList.StaffUp:
                Popup_StaffUp.SetActive(true);
                break;
            case PopupList.Shop:
                Popup_Shop.SetActive(true);
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
                Popup_Marketing.SetActive(false);
                break;
            case PopupList.MarketingCk:
                Popup_MarketingCk.SetActive(false);
                break;
            case PopupList.Develop:
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
                Popup_Staff.SetActive(false);
                break;
            case PopupList.StaffUp:
                Popup_StaffUp.SetActive(false);
                break;
            case PopupList.Shop:
                Popup_Shop.SetActive(false);
                break;
            default:
                break;
        }
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
                    ScenarioData.Instance.SetScenarioData();
                    LoadManager.Load(LoadManager.Scene.Scenario);
                };
                break;
            case GameManager.Step.Cast_Actor:
                Progress = () =>
                {
                    Popup_On((int)PopupList.Audition);
                };
                break;
            case GameManager.Step.Set_Period:
                Progress = () =>
                {
                    Popup_On((int)PopupList.Period);
                    GameManager.Instance.SetDefaultPeriod();
                };
                break;
            case GameManager.Step.Prepare_Play:
                Progress = () =>
                {
                    Popup_On((int)PopupList.Prepare);
                    Popup_Prepare.transform.Find("Play BT").GetComponent<Button>().interactable = false;
                };
                break;
            case GameManager.Step.Start_Play:
                Progress = () =>
                {
                    Popup_On((int)PopupList.Prepare);
                    Popup_Prepare.transform.Find("Play BT").GetComponent<Button>().interactable = true;
                };
                break;
            default:
                break;
        }

        Btn_Progress.onClick.RemoveAllListeners();
        Btn_Progress.onClick.AddListener(delegate { Progress(); } );
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
        StartPrepare();
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

    public void Click_Illust()
    {
        LoadManager.Load(LoadManager.Scene.Illust);
    }
}

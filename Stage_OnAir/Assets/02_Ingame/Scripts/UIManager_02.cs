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
    public Text Money;
    public Text Year;
    public Text Month;

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

    private void Start()
    {
        Backend.Initialize(() =>
        {
            // 초기화 성공한 경우 실행
            if (Backend.IsInitialized)
            {
                var data = Backend.BMember.CustomLogin("test2", "1234");

                Debug.Log("초기화 완료");
            }
            // 초기화 실패한 경우 실행
            else
            {

            }
        });

        Backend.Chart.GetAllChartAndSave(true); 
        SetProgress();
    }

    private void Update()
    {
        Money.text = GameManager.Instance.Money.ToString("N0");
        if (GameManager.Instance.Money <= 0)
            Money.color = Color.red;
        else
            Money.color = Color.black;
        Year.text = GameManager.Instance.Year.ToString("D2");
        Month.text = GameManager.Instance.Month.ToString("D2");

        StepText.GetComponent<Text>().text = GameManager.Instance.NowStep.ToString();

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

        GameManager.Instance.SetStep(GameManager.Step.Prepare_Play);
    }
    public void Illuset_Scene()
    {
        ScenarioData.Instance.SetScenarioData();
        LoadManager.Load(LoadManager.Scene.Illust);
    }
}

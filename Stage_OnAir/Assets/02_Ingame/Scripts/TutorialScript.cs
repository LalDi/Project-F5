using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TutorialScript : MonoBehaviour
{
    public GameObject Prefab;
    public Transform BackGround;
    public Transform Message;

    //Obj Stat
    public bool IsAdd;
    public int Num;
    public delegate void TutoDel();
    public TutoDel tutoDel;
    public Transform TutoParent;
    public Transform tutoObj;
    public int TutoObjNum;

    //UI Managers
    public UIManager_02 UIM_2;
    public UIManager_03 UIM_3;
    public UIManager_04 UIM_4;
    public UIManager_05 UIM_5;

    //UI Parents
    public Transform TopUI;
    public Transform LowerUI;
    public Transform PopupUI;
    public Transform ForderUI;

    public Transform ScrollRect;

    public Transform CountUI;

    public Transform EndingUI;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Tutorial()
    {
        IsAdd = true;

        switch (Num)
        {
            #region Main
            case 0:
                //UI 지정
                UIM_2 = GameObject.Find("UIManager").GetComponent<UIManager_02>();

                TopUI = GameObject.Find("Canvas").transform.GetChild(0);
                LowerUI = GameObject.Find("Canvas").transform.GetChild(1);
                PopupUI = GameObject.Find("Canvas").transform.GetChild(3);
                ForderUI = TopUI.Find("Forder UI");

                BackGround = Instantiate(Prefab, GameObject.Find("Canvas").transform).transform;
                Message = BackGround.GetChild(0);

                Message.GetChild(0).GetComponent<Text>().text
                    = "지금부터 튜토리얼을 시작하겠습니다.";
                Num++;
                break;
            case 1:
                Message.GetChild(0).GetComponent<Text>().text
                    = "이곳은 가장 기본적인 메인메뉴입니다.";
                Num++;
                break;
            case 2:
                //위로 가져올 오브젝트의 부모오브젝트, 하이어라키 순서를 저장
                tutoObj = TopUI.transform.Find("Money");
                TutoParent = tutoObj.parent;
                TutoObjNum = tutoObj.GetSiblingIndex();

                //튜토리얼 배경 위로 오브젝트 가져오기
                tutoObj.SetParent(BackGround);

                Message.localPosition = new Vector3(0, 570);
                Message.GetChild(0).GetComponent<Text>().text
                    = "가장 위의 숫자는 현재 보유중인 금액을 표시해 줍니다.";
                Num++;
                break;
            case 3:
                SetParent(TopUI.transform.Find("Date"));

                Message.GetChild(0).GetComponent<Text>().text
                    = "오른쪽의 달력은 몇 월 며칠인지 표시해 줍니다.";
                Num++;
                break;
            case 4:
                UIM_2.MonthBT();

                Message.GetChild(0).GetComponent<Text>().text
                    = "터치를 통해 준비 기간의 남은 일 수를 볼 수도 있습니다.";
                Num++;
                break;
            case 5:
                SetParent(TopUI.transform.Find("Stat UI"));

                Message.localPosition = new Vector3(0, 200);
                Message.GetChild(0).GetComponent<Text>().text
                    = "왼쪽의 연극 정보는 현재 진행중인 연극의 정보를 보여줍니다.";
                Num++;    
                break;
            case 6:
                Message.GetChild(0).GetComponent<Text>().text
                    = "터치를 통해 열고 닫을 수 있습니다.";
                Num++;
                break;
            case 7:
                Message.GetChild(0).GetComponent<Text>().text
                    = "진행 단계는 총 5단계로 시나리오 선택, \n배우 캐스팅, 준비 기간 설정, 공연 준비, \n연극공연 개시 5간계가 있습니다.";
                Num++;
                break;
            case 8:
                Message.GetChild(0).GetComponent<Text>().text
                    = "시나리오 이름은 선택한 시나리오의 이름이 나타납니다.";
                Num++;
                break;
            case 9:
                Message.GetChild(0).GetComponent<Text>().text
                    = "배우는 캐스팅 된 배우의 수 / 필요한 배우의 수로 표기됩니다.";
                Num++;
                break;
            case 10:
                Message.GetChild(0).GetComponent<Text>().text
                    = "준비 기간은 플레이어가 설정한 준비 기간이 표기됩니다.";
                Num++;
                break;
            case 11:
                Message.GetChild(0).GetComponent<Text>().text
                    = "퀄리티, 마케팅, 성공률은 시나리오, 배우, \n준비 기간, 홍보 수단, 발전 레벨, 상점 아이템 및 \n스태프의 레벨에 영향을 받습니다.";
                Num++;
                break;
            case 12:
                SetParent(TopUI.transform.Find("Forder UI"));

                if (ForderUI.GetComponent<SettingMenu>().IsOpen == false)
                    ForderUI.GetChild(3).GetComponent<Button>().onClick.Invoke();
                foreach (Transform child in ForderUI)
                    child.GetComponent<Button>().enabled = false;

                Message.localPosition = new Vector3(-100, 450);
                Message.GetChild(0).GetComponent<Text>().text
                    = "오른쪽의 폴더를 열면 설정, 랭킹, 일러스트북 세가지의 버튼이 있습니다.";
                Num++;
                break;
            case 13:
                SetParent(LowerUI.transform.Find("Shop BT"));
                tutoObj.GetComponent<Button>().enabled = false;

                if (ForderUI.GetComponent<SettingMenu>().IsOpen == true)
                    ForderUI.GetChild(3).GetComponent<Button>().onClick.Invoke();

                Message.localPosition = new Vector3(0, -450);
                Message.GetChild(0).GetComponent<Text>().text
                    = "하단 왼쪽에 위치한 버튼은 상점 버튼입니다.";
                Num++;
                break;
            case 14:
                tutoObj.GetComponent<Button>().enabled = true;
                SetParent(PopupUI.transform.Find("Shop PU"));
                UIM_2.Popup_On(15);

                Message.localPosition = new Vector3(0, -550);
                Message.GetChild(0).GetComponent<Text>().text
                    = "상점에서는 여러 편리한 아이템을 판매합니다.";
                Num++;
                break;
            case 15:
                SetParent(LowerUI.transform.Find("Steff BT"));
                tutoObj.GetComponent<Button>().enabled = false;
                UIM_2.Popup_Quit();

                Message.localPosition = new Vector3(0, -450);
                Message.GetChild(0).GetComponent<Text>().text
                    = "하단 오른쪽에 위치한 버튼은 스태프 버튼입니다.";
                Num++;
                break;
            case 16:
                tutoObj.GetComponent<Button>().enabled = true;
                SetParent(PopupUI.transform.Find("Staff PU"));
                UIM_2.Popup_On(12);

                ScrollRect = tutoObj.Find("Scroll Rect Mask").GetChild(0);
                foreach (Transform child in ScrollRect)
                    child.GetComponent<Button>().enabled = false;

                Message.localPosition = new Vector3(0, -550);
                Message.GetChild(0).GetComponent<Text>().text
                    = "스태프는 연극이 끝난 후에도 유지됩니다.";
                Num++;
                break;
            case 17:
                Transform ScrollObj_2 = tutoObj.Find("Scroll Rect Mask").GetChild(0);
                ScrollRect = tutoObj.Find("Scroll Rect Mask").GetChild(0);
                foreach (Transform child in ScrollRect)
                    child.GetComponent<Button>().enabled = true;

                UIM_2.Popup_Quit();
                SetParent(LowerUI.transform.Find("Progress BT"));
                tutoObj.GetComponent<Button>().enabled = false;

                Message.localPosition = new Vector3(0, -450);
                Message.GetChild(0).GetComponent<Text>().text
                    = "하단 가운데에 위치한 버튼은 진행 버튼입니다.";
                Num++;
                break;
            case 18:
                Message.GetChild(0).GetComponent<Text>().text
                    = "현재 진행 단계에 따라 버튼의 용도가 달라집니다.";
                Num++;
                break;
            case 19:
                tutoObj.GetComponent<Button>().enabled = true;
                SetParent(LowerUI.transform.Find("Progress Gauge_BG"));

                Message.localPosition = new Vector3(0, -350);
                Message.GetChild(0).GetComponent<Text>().text
                    = "버튼 위의 게이지는 진행 단계 게이지입니다.";
                Num++;
            #endregion
            #region Scenario
                break;
            case 20:
                tutoObj.SetParent(TutoParent);
                tutoObj.SetSiblingIndex(TutoObjNum);

                Message.localPosition = new Vector3(0, 0);
                Message.GetChild(0).GetComponent<Text>().text
                    = "그럼 지금부터 본격적으로 연극을 올리기 위한 과정을 설명해드리겠습니다.";
                Num++;
                break;
            case 21:
                SetParent(LowerUI.transform.Find("Progress BT"));
                IsAdd = false;

                Message.localPosition = new Vector3(0, -450);
                Message.GetChild(0).GetComponent<Text>().text
                    = "먼저 연극을 올리기 위해선 시나리오가 필요합니다. \n시나리오를 구매하기 위해 진행버튼을 눌러주세요.";
                Num++;
                break;
            case 22:
                //UI 지정
                UIM_3 = GameObject.Find("UIManager").GetComponent<UIManager_03>();

                TopUI = GameObject.Find("Canvas").transform.GetChild(1);
                ScrollRect = GameObject.Find("Canvas").transform.GetChild(2).GetChild(0);
                LowerUI = GameObject.Find("Canvas").transform.GetChild(3);
                PopupUI = GameObject.Find("Canvas").transform.GetChild(4);

                BackGround = Instantiate(Prefab, GameObject.Find("Canvas").transform).transform;
                Message = BackGround.GetChild(0);

                Message.GetChild(0).GetComponent<Text>().text
                    = "이곳에선 공연은 위해 원하는 시나리오를 구매할 수 있습니다.";
                Num++;
                break;
            case 23:
                //위로 가져올 오브젝트의 부모오브젝트, 하이어라키 순서를 저장
                tutoObj = TopUI;
                TutoParent = tutoObj.parent;
                TutoObjNum = tutoObj.GetSiblingIndex();

                //튜토리얼 배경 위로 오브젝트 가져오기
                tutoObj.SetParent(BackGround);

                Message.localPosition = new Vector3(0, 550);
                Message.GetChild(0).GetComponent<Text>().text
                    = "숫자는 현재 보유중인 금액을 표시해 줍니다.";
                Num++;
                break;
            case 24:
                SetParent(ScrollRect.parent);
                foreach (Transform child in tutoObj.GetChild(0))
                    child.GetComponent<Button>().enabled = false;

                Message.localPosition = new Vector3(0, -400);
                Message.GetChild(0).GetComponent<Text>().text
                    = "각각의 시나리오 버튼들입니다. 클릭하면 시나리오 정보를 볼 수 있습니다.";
                Num++;
                break;
            case 25:
                foreach (Transform child in tutoObj.GetChild(0))
                    child.GetComponent<Button>().enabled = true;

                SetParent(ScrollRect.Find("Scenario 0"));
                tutoObj.GetComponent<Button>().onClick.AddListener(Tutorial);
                IsAdd = false;

                Message.localPosition = new Vector3(0, 150);
                Message.GetChild(0).GetComponent<Text>().text
                    = "이번에는 어린왕자 시나리오를 구매해서 연극을 진행시켜 봅시다.";
                Num++;
                break;
            case 26:
                SetParent(PopupUI.Find("Scenario Select PU"));
                tutoObj.Find("Buy BT").GetComponent<Button>().onClick.AddListener(Tutorial);
                IsAdd = false;

                Message.localPosition = new Vector3(0, -500);
                Message.GetChild(0).GetComponent<Text>().text
                    = "시나리오 정보창에서는 시나리오의 이름과 연출력, 필요 배우의 수, 가격이 표기됩니다.";
                Num++;
                break;
            case 27:
                SetParent(PopupUI.Find("Buy Checking PU"));
                IsAdd = false;

                Message.GetChild(0).GetComponent<Text>().text
                    = "만약 시나리오를 구매할 돈이 부족하다면 대출을 받아 구매할 수 있습니다.";
                Num++;
                break;
            #endregion
            #region Audition
            case 28:
            case 29:
                //UI 지정
                UIM_2 = GameObject.Find("UIManager").GetComponent<UIManager_02>();

                TopUI = GameObject.Find("Canvas").transform.GetChild(0);
                LowerUI = GameObject.Find("Canvas").transform.GetChild(1);
                PopupUI = GameObject.Find("Canvas").transform.GetChild(2);
                ForderUI = TopUI.Find("Forder UI");

                BackGround = Instantiate(Prefab, GameObject.Find("Canvas").transform).transform;
                Message = BackGround.GetChild(0);

                Message.localPosition = new Vector3(0, 0);
                Message.GetChild(0).GetComponent<Text>().text
                    = "이제 시나리오는 준비가 되었으니 배우를 캐스팅해봅시다.";
                Num++;
                break;
            case 30:
                tutoObj = LowerUI.transform.Find("Progress BT");
                TutoParent = tutoObj.parent;
                TutoObjNum = tutoObj.GetSiblingIndex();

                tutoObj.SetParent(BackGround);
                tutoObj.SetSiblingIndex(0); 
                EventTrigger eventTrigger_1 = tutoObj.gameObject.AddComponent<EventTrigger>(); 

                EventTrigger.Entry entry_PointerEnter_1 = new EventTrigger.Entry();
                entry_PointerEnter_1.eventID = EventTriggerType.PointerClick;
                entry_PointerEnter_1.callback.AddListener((data) => { Tutorial(); });
                eventTrigger_1.triggers.Add(entry_PointerEnter_1);

                IsAdd = false;

                Message.localPosition = new Vector3(0, -450);
                Message.GetChild(0).GetComponent<Text>().text
                    = "배우도 마찬가지로 진행버튼을 통해 오디션장으로 이동가능합니다.";
                Num++;
                break;
            case 31:
                SetParent(PopupUI.Find("Audition PU"));
                tutoObj.Find("Progress BT").GetComponent<Button>().onClick.AddListener(Tutorial);
                IsAdd = false;

                Message.localPosition = new Vector3(0, -500);
                Message.GetChild(0).GetComponent<Text>().text
                    = "오디션은 한번에 50,000원의 비용이 들게됩니다.\n만약 돈이 부족하다면 대출을 받아 오디션을 진행할 수 있습니다.";
                Num++;
                break;
            case 32:
                //UI 지정
                UIM_4 = GameObject.Find("UIManager").GetComponent<UIManager_04>();
                Debug.Log(UIM_4.name);

                TopUI = GameObject.Find("Canvas").transform.GetChild(1);
                LowerUI = GameObject.Find("Canvas").transform.GetChild(0);
                PopupUI = GameObject.Find("Canvas").transform.GetChild(4);
                CountUI= GameObject.Find("Canvas").transform.GetChild(2);

                BackGround = Instantiate(Prefab, GameObject.Find("Canvas").transform).transform;
                Message = BackGround.GetChild(0);

                Message.localPosition = new Vector3(0, 0);
                Message.GetChild(0).GetComponent<Text>().text
                    = "이 곳은 배우를 캐스팅할 수 있는 오디션장입니다.\n배우의 이력서를 확인하고 마음에 드는 배우를 합격시킬 수 있습니다.";
                Num++;
                break;
            case 33:
                tutoObj = TopUI;
                TutoParent = tutoObj.parent;
                TutoObjNum = tutoObj.GetSiblingIndex();

                tutoObj.SetParent(BackGround);
                tutoObj.SetSiblingIndex(0);

                Message.localPosition = new Vector3(-50, 700);
                Message.GetChild(0).GetComponent<Text>().text
                    = "가장 위의 숫자는 현재 보유중인 금액을 표시해 줍니다.";
                Num++;
                break;
            case 34:
                SetParent(CountUI);

                Message.localPosition = new Vector3(0, 500);
                Message.GetChild(0).GetComponent<Text>().text
                    = "상단의 숫자는 오디션을 보러 온 배우의 수와 오디션을 마친 배우의 수가 표기됩니다.";
                Num++;
                break;
            case 35:
                SetParent(LowerUI);
                tutoObj.GetChild(2).GetComponent<Button>().enabled = false;
                tutoObj.GetChild(3).GetComponent<Button>().enabled = false;

                Message.localPosition = new Vector3(0, 0);
                Message.GetChild(0).GetComponent<Text>().text
                    = "하단의 종이는 이력서로 오디션을 진행중인 배우의 정보가 표기됩니다.\n각각 연기력, 경험, 캐스팅 비용이 적혀있습니다.";
                Num++;
                break;
            case 36:
                SetParent(LowerUI.GetChild(3));
                tutoObj.GetComponent<Button>().enabled = true;
                tutoObj.GetComponent<Button>().onClick.AddListener(Tutorial);
                IsAdd = false;

                Message.GetChild(0).GetComponent<Text>().text
                    = "다음 단계로 넘어가기 위해 시나리오 필요 배우수가 충족될 때 까지 합격시켜봅시다.";
                Num++;
                break;
            case 37:
                if (!UIM_4.Popup_Result.activeSelf)
                    return;
                IsAdd = false;
                LowerUI.GetChild(2).GetComponent<Button>().enabled = true;
                SetParent(PopupUI);


                Message.localPosition = new Vector3(0, -500);
                Message.GetChild(0).GetComponent<Text>().text
                    = "오디션이 끝나면 결과창을 통해 합격한 인원을 확인할 수 있습니다.\n진행 버튼을 통해 다시 메인화면으로 돌아갈수 있습니다.";
                Num++;
                if (GameManager.Instance.MaxActor == GameManager.Instance.NowActor)
                    Num = 48;

                break;
            case 38:
                //UI 지정
                UIM_2 = GameObject.Find("UIManager").GetComponent<UIManager_02>();

                TopUI = GameObject.Find("Canvas").transform.GetChild(0);
                LowerUI = GameObject.Find("Canvas").transform.GetChild(1);
                PopupUI = GameObject.Find("Canvas").transform.GetChild(2);
                ForderUI = TopUI.Find("Forder UI");

                BackGround = Instantiate(Prefab, GameObject.Find("Canvas").transform).transform;
                Message = BackGround.GetChild(0);

                tutoObj = LowerUI.transform.Find("Progress BT");
                TutoParent = tutoObj.parent;
                TutoObjNum = tutoObj.GetSiblingIndex();

                tutoObj.SetParent(BackGround);
                tutoObj.SetSiblingIndex(0);

                EventTrigger eventTrigger_2 = tutoObj.gameObject.AddComponent<EventTrigger>();

                EventTrigger.Entry entry_PointerEnter_2 = new EventTrigger.Entry();
                entry_PointerEnter_2.eventID = EventTriggerType.PointerClick;
                entry_PointerEnter_2.callback.AddListener((data) => { Tutorial(); });
                eventTrigger_2.triggers.Add(entry_PointerEnter_2);

                Message.localPosition = new Vector3(0, -450);
                Message.GetChild(0).GetComponent<Text>().text
                    = "시나리오 필요 배우 수가 충족되지 않았으므로 한번 더 오디션을 봅니다.";
                Num++;
                break;
            case 39:
                SetParent(PopupUI.Find("Audition PU"));
                tutoObj.Find("Progress BT").GetComponent<Button>().onClick.AddListener(Tutorial);
                IsAdd = false;

                Message.localPosition = new Vector3(0, 1500);
                Message.GetChild(0).GetComponent<Text>().text
                    = "";
                Num++;
                break;
            case 40:
                //UI 지정
                UIM_4 = GameObject.Find("UIManager").GetComponent<UIManager_04>();

                TopUI = GameObject.Find("Canvas").transform.GetChild(1);
                LowerUI = GameObject.Find("Canvas").transform.GetChild(0);
                PopupUI = GameObject.Find("Canvas").transform.GetChild(4);
                CountUI = GameObject.Find("Canvas").transform.GetChild(2);

                BackGround = Instantiate(Prefab, GameObject.Find("Canvas").transform).transform;
                Message = BackGround.GetChild(0);

                tutoObj = LowerUI.Find("Pass BT");
                TutoParent = tutoObj.parent;
                TutoObjNum = tutoObj.GetSiblingIndex();

                tutoObj.SetParent(BackGround);
                tutoObj.SetSiblingIndex(0); 

                tutoObj.GetComponent<Button>().enabled = true;
                tutoObj.GetComponent<Button>().onClick.AddListener(Tutorial);
                IsAdd = false;

                Message.GetChild(0).GetComponent<Text>().text
                    = "";
                Num++;
                break;
            case 41:
                if (!UIM_4.Popup_Result.activeSelf)
                    return;
                IsAdd = false;
                LowerUI.GetChild(2).GetComponent<Button>().enabled = true;
                SetParent(PopupUI);

                Message.GetChild(0).GetComponent<Text>().text
                    = "";
                Num++;
                if (GameManager.Instance.MaxActor > GameManager.Instance.NowActor)
                {
                    Num = 44;
                    Tutorial();
                }
                break;
            case 42:
                //UI 지정
                UIM_2 = GameObject.Find("UIManager").GetComponent<UIManager_02>();

                TopUI = GameObject.Find("Canvas").transform.GetChild(0);
                LowerUI = GameObject.Find("Canvas").transform.GetChild(1);
                PopupUI = GameObject.Find("Canvas").transform.GetChild(2);
                ForderUI = TopUI.Find("Forder UI");

                BackGround = Instantiate(Prefab, GameObject.Find("Canvas").transform).transform;
                Message = BackGround.GetChild(0);

                Message.localPosition = new Vector3(0, 0);
                Message.GetChild(0).GetComponent<Text>().text
                    = "배우를 필요한 만큼 캐스팅을 했으니 다음은 연극을 위한 준비기간입니다.";
                Num++;
                break;
            case 43:
            case 44:
            case 45:
                tutoObj = LowerUI.transform.Find("Progress BT");
                TutoParent = tutoObj.parent;
                TutoObjNum = tutoObj.GetSiblingIndex();

                tutoObj.SetParent(BackGround);
                tutoObj.SetSiblingIndex(0);

                EventTrigger eventTrigger_3 = tutoObj.gameObject.AddComponent<EventTrigger>();

                EventTrigger.Entry entry_PointerEnter_3 = new EventTrigger.Entry();
                entry_PointerEnter_3.eventID = EventTriggerType.PointerClick;
                entry_PointerEnter_3.callback.AddListener((data) => { Tutorial(); });
                eventTrigger_3.triggers.Add(entry_PointerEnter_3);

                IsAdd = false;

                Message.GetChild(0).GetComponent<Text>().text
                    = "버튼을 눌러서 준비기간을 설정할 수 있습니다.";
                Num++;
                break;
            case 46:
                SetParent(PopupUI.Find("Period PU"));
                tutoObj.GetChild(6).GetComponent<Button>().enabled = false;

                Message.localPosition = new Vector3(0, -500);
                Message.GetChild(0).GetComponent<Text>().text
                    = "준비기간이 길수록 연극의 성공률이 올라갑니다. \n반대로 기간이 짧을수록 성공률이 낮아집니다.";
                Num++;
                break;
            case 47:
                Message.GetChild(0).GetComponent<Text>().text
                    = "준비기간을 적당한 2개월에 맞춰줍니다.";
                if (GameManager.Instance.Period == 2)
                    Num++;
                break;
            case 48:
                tutoObj.GetChild(6).GetComponent<Button>().onClick.AddListener(Tutorial);
                tutoObj.GetChild(6).GetComponent<Button>().enabled = true;
                Message.localPosition = new Vector3(0, 0);
                Message.GetChild(0).GetComponent<Text>().text
                    = "준비기간이 설정되면 시간이 흐르게됩니다. \n준비기간동안에는 공연홍보와 연극발전 아이템을 구매할 수 있습니다.";
                Num++;
                break;
            case 49:
                SetParent(LowerUI.transform.Find("Progress BT"));
                IsAdd = false;

                Message.localPosition = new Vector3(0, -450);
                Message.GetChild(0).GetComponent<Text>().text
                    = "홍보와 발전아이템상점은 진행버튼을 눌러 진입가능합니다.";
                Num++;
                break;
            case 50:
                SetParent(PopupUI.Find("Prepare PU"));

                Message.GetChild(0).GetComponent<Text>().text
                    = "홍보는 관람객의 수가, 발전은 여러 효과가 있는 아이템들입니다.";
                Num++;
                break;
            case 51:
                Message.GetChild(0).GetComponent<Text>().text
                    = "아이템의 구매를 통해 보다 더 많은 연극수익을 얻을 수 있습니다.";
                Num++;
                break;
            case 52:
                SetParent(GameObject.Find("Canvas").transform.GetChild(4));
                UIM_2.Popup_Quit();

                Message.localPosition = new Vector3(0, 0);
                Message.GetChild(0).GetComponent<Text>().text
                    = "이외에도 상점에서 유료아이템을 통해 더욱 좋은 아이템들을 얻을 수 있습니다.";
                Num++;
                break;
            case 53:
                if (GameManager.Instance.NowStep != GameManager.Step.Start_Play)
                    return;
                Message.GetChild(0).GetComponent<Text>().text
                    = "준비기간이 끝나게되면 애니메이션이 실행되며, 진행버튼에 공연버튼이 활성화됩니다.";
                Num++;
                break;
            case 54:
                SetParent(LowerUI.transform.Find("Progress BT"));
                IsAdd = false;

                Message.localPosition = new Vector3(0, 1500);
                Message.GetChild(0).GetComponent<Text>().text
                    = "";
                Num++;
                break;
            case 55:
                SetParent(PopupUI.transform.Find("Prepare PU").Find("Play BT"));

                tutoObj.GetComponent<Button>().onClick.AddListener(Tutorial);
                IsAdd = false;

                Message.GetChild(0).GetComponent<Text>().text
                    = "";
                Num++;
                break;
            case 56:
                SetParent(PopupUI.transform.Find("Play PU"));

                IsAdd = false;

                Message.localPosition = new Vector3(0, -450);
                Message.GetChild(0).GetComponent<Text>().text
                    = "공연버튼을 눌러서 바로 공연을 시작할 수 있습니다.";
                Num++;
                break;
            case 57:
                //UI 지정
                UIM_5 = GameObject.Find("UIManager").GetComponent<UIManager_05>();

                TopUI = GameObject.Find("Canvas").transform.GetChild(2);
                EndingUI = GameObject.Find("Canvas").transform.GetChild(3);
                PopupUI = GameObject.Find("Canvas").transform.GetChild(4).GetChild(1);

                BackGround = Instantiate(Prefab, GameObject.Find("Canvas").transform).transform;
                Message = BackGround.GetChild(0);

                tutoObj = TopUI;
                TutoParent = tutoObj.parent;
                TutoObjNum = tutoObj.GetSiblingIndex();

                tutoObj.SetParent(BackGround);
                tutoObj.SetSiblingIndex(0);
                tutoObj.GetComponent<Button>().onClick.AddListener(Tutorial);

                BackGround.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                Message.localPosition = new Vector3(0, -500);
                Message.GetChild(0).GetComponent<Text>().text
                    = "화면 상단에 있는 스킵버튼을 통해 공연 스킵이 가능합니다.";
                Num++;
                break;
            case 58:
                if (!PopupUI.gameObject.activeSelf)
                    return;
                SetParent(PopupUI);
                tutoObj.GetChild(3).GetComponent<Button>().enabled = false;

                BackGround.GetComponent<Image>().color = new Color(0, 0, 0, 168/255f);
                Message.localPosition = new Vector3(0, -450);
                Message.GetChild(0).GetComponent<Text>().text
                    = "공연이 끝나게되면 공연의 성공여부와 함께 결과창이 뜹니다.";
                Num++;
                break;
            case 59:
                Message.GetChild(0).GetComponent<Text>().text
                    = "또한 일정 조건을 클리어시 해당 시나리오의 일러스트를 얻을 수 있습니다.";
                Num++;
                break;
            case 60:
                IsAdd = false;

                tutoObj.GetChild(3).GetComponent<Button>().enabled = true;
                Message.GetChild(0).GetComponent<Text>().text
                    = "확인 버튼을 눌러 메인화면으로 돌아갈 수 있습니다.";
                Num++;
                break;
            case 61:
                //UI 지정
                UIM_2 = GameObject.Find("UIManager").GetComponent<UIManager_02>();

                BackGround = Instantiate(Prefab, GameObject.Find("Canvas").transform).transform;
                Message = BackGround.GetChild(0);

                Message.localPosition = new Vector3(0, 0);
                Message.GetChild(0).GetComponent<Text>().text
                    = "여기까지 튜토리얼이었습니다. 수고하셨습니다.";
                Num++;
                break;
            case 62:
                Destroy(BackGround.gameObject);
                Destroy(gameObject);
                GameManager.Instance.Tutorial = false;
                break;
                #endregion
        }
        BackGround.gameObject.SetActive(true);
        Message.gameObject.SetActive(true);

        tutoDel = () =>
        {
            Tutorial();
        };

        BackGround.GetComponent<Button>().onClick.RemoveAllListeners();
        if (IsAdd)
            BackGround.GetComponent<Button>().onClick.AddListener(delegate { tutoDel(); });
    }

    public void SetParent(Transform Obj)
    {
        tutoObj.SetParent(TutoParent);
        tutoObj.SetSiblingIndex(TutoObjNum);

        tutoObj = Obj;
        TutoParent = tutoObj.parent;
        TutoObjNum = tutoObj.GetSiblingIndex();

        tutoObj.SetParent(BackGround);
        tutoObj.SetSiblingIndex(0);
    }
}

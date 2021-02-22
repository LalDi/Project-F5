using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    public Transform BackGround;
    public GameObject Message;

    public int Num;
    public delegate void TutoDel();
    public TutoDel tutoDel;
    public Transform TutoParent;
    public Transform tutoObj;
    public int TutoObjNum;

    public Transform TopUI;
    public Transform LowerUI;
    public Transform PopupUI;

    public UIManager_02 UIM_2;
    public Transform ForderUI;

    void Start()
    {
        //UIM_2 = GameObject.Find("UIManager").GetComponent<UIManager_02>();

        //TopUI = GameObject.Find("Canvas").transform.GetChild(0);
        //LowerUI = GameObject.Find("Canvas").transform.GetChild(1);
        //PopupUI = GameObject.Find("Canvas").transform.GetChild(2);
    }

    void Update()
    {

    }
    public void Tutorial(int Sequence)
    {
        BackGround.gameObject.SetActive(true);
        Message.SetActive(true);

        switch (Sequence)
        {
            case 0:
                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "지금부터 튜토리얼을 시작하겠습니다.";
                Num++;
                break;
            case 1:
                Message.transform.GetChild(0).GetComponent<Text>().text
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

                Message.transform.localPosition = new Vector3(0, 570);
                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "가장 위의 숫자는 현재 보유중인 금액을 표시해 줍니다.";
                Num++;
                break;
            case 3:
                SetParent(TopUI.transform.Find("Date"));

                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "오른쪽의 달력은 몇 월 며칠인지 표시해 줍니다.";
                Num++;
                break;
            case 4:
                UIM_2.MonthBT();

                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "터치를 통해 준비 기간의 남은 일 수를 볼 수도 있습니다.";
                Num++;
                break;
            case 5:
                SetParent(TopUI.transform.Find("Stat UI"));

                Message.transform.localPosition = new Vector3(0, 200);
                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "왼쪽의 연극 정보는 현재 진행중인 연극의 정보를 보여줍니다.";
                Num++;
                break;
            case 6:
                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "터치를 통해 열고 닫을 수 있습니다.";
                Num++;
                break;
            case 7:
                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "진행 단계는 총 5단계로 시나리오 선택, \n배우 캐스팅, 준비 기간 설정, 공연 준비, \n연극공연 개시 5간계가 있습니다.";
                Num++;
                break;
            case 8:
                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "시나리오 이름은 선택한 시나리오의 이름이 나타납니다.";
                Num++;
                break;
            case 9:
                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "배우는 캐스팅 된 배우의 수 / 필요한 배우의 수로 표기됩니다.";
                Num++;
                break;
            case 10:
                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "준비 기간은 플레이어가 설정한 준비 기간이 표기됩니다.";
                Num++;
                break;
            case 11:
                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "퀄리티, 마케팅, 성공률은 시나리오, 배우, \n준비 기간, 홍보 수단, 발전 레벨, 상점 아이템 및 \n스태프의 레벨에 영향을 받습니다.";
                Num++;
                break;
            case 12:
                SetParent(TopUI.transform.Find("Forder UI"));

                Message.transform.localPosition = new Vector3(-100, 450);
                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "오른쪽의 폴더를 열면 세가지의 버튼이 있습니다.";
                Num++;
                break;
            case 13:
                if (ForderUI.GetComponent<SettingMenu>().IsOpen == false)
                    ForderUI.GetChild(3).GetComponent<Button>().onClick.Invoke();
                for (int i = 0; i < 3; i++)
                    ForderUI.GetChild(i).GetComponent<Button>().enabled = false;

                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "위부터 설정, 랭킹, 일러스트북입니다.";
                Num++;
                break;
            case 14:
                if (ForderUI.GetComponent<SettingMenu>().IsOpen == true)
                    ForderUI.GetChild(3).GetComponent<Button>().onClick.Invoke();

                SetParent(PopupUI.transform.Find("Option PU"));
                UIM_2.Popup_On(0);

                Message.transform.localPosition = new Vector3(0, -550);
                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "설정에서는 음악 설정, 데이터 저장, 닉네임 변경 등의 기능이 있습니다.";
                Num++;
                break;
            case 15:
                SetParent(PopupUI.transform.Find("Rank PU"));
                UIM_2.Popup_Quit();
                UIM_2.Popup_On(1);

                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "랭킹은 각각 퀄리티 점수, 관객 점수, 수익 점수 3가지가 있습니다.";
                Num++;
                break;
            case 16:
                if (ForderUI.GetComponent<SettingMenu>().IsOpen == false)
                    ForderUI.GetChild(3).GetComponent<Button>().onClick.Invoke();
                for (int i = 0; i < 3; i++)
                    ForderUI.GetChild(i).GetComponent<Button>().enabled = false;

                SetParent(TopUI.transform.Find("Forder UI").GetChild(2));
                UIM_2.Popup_Quit();

                Message.transform.localPosition = new Vector3(-100, 450);
                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "일러스트북은 시나리오 각각의 일러스트를 구경할 수 있습니다.";
                Num++;
                break;
            case 17:
                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "일러스트에 해당하는 연극을 일정 조건과 함께 성공 시 얻을 수 있습니다.";
                Num++;
                break;
            case 18:
                SetParent(LowerUI.transform.Find("Shop BT"));
                tutoObj.GetComponent<Button>().enabled = false;

                if (ForderUI.GetComponent<SettingMenu>().IsOpen == true)
                    ForderUI.GetChild(3).GetComponent<Button>().onClick.Invoke();

                Message.transform.localPosition = new Vector3(0, -450);
                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "하단 왼쪽에 위치한 버튼은 상점 버튼입니다.";
                Num++;
                break;
            case 19:
                tutoObj.GetComponent<Button>().enabled = true;
                SetParent(PopupUI.transform.Find("Shop PU"));
                UIM_2.Popup_On(15);

                Message.transform.localPosition = new Vector3(0, -550);
                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "상점에서는 여러 편리한 아이템을 판매합니다.";
                Num++;
                break;
            case 20:
                SetParent(LowerUI.transform.Find("Steff BT"));
                tutoObj.GetComponent<Button>().enabled = false;
                UIM_2.Popup_Quit();

                Message.transform.localPosition = new Vector3(0, -450);
                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "하단 오른쪽에 위치한 버튼은 스태프 버튼입니다.";
                Num++;
                break;
            case 21:
                tutoObj.GetComponent<Button>().enabled = true;
                SetParent(PopupUI.transform.Find("Staff PU"));
                UIM_2.Popup_On(12);

                Transform ScrollObj_1 = tutoObj.Find("Scroll Rect Mask").GetChild(0);
                for (int i = 0; i < ScrollObj_1.childCount; i++)
                {
                    ScrollObj_1.GetChild(i).GetComponent<Button>().enabled = false;
                }

                Message.transform.localPosition = new Vector3(0, -550);
                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "스태프는 고용 비용이 비싼 대신 연극이 끝난 후에도 유지됩니다.";
                Num++;
                break;
            case 22:
                Transform ScrollObj_2 = tutoObj.Find("Scroll Rect Mask").GetChild(0);
                for (int i = 0; i < ScrollObj_2.childCount; i++)
                {
                    ScrollObj_2.GetChild(i).GetComponent<Button>().enabled = true;
                }
                UIM_2.Popup_Quit();
                SetParent(LowerUI.transform.Find("Progress BT"));
                tutoObj.GetComponent<Button>().enabled = false;

                Message.transform.localPosition = new Vector3(0, -450);
                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "하단 가운데에 위치한 버튼은 진행 버튼입니다.";
                Num++;
                break;
            case 23:
                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "현재 진행 단계에 따라 버튼의 용도가 달라집니다.";
                Num++;
                break;
            case 24:
                tutoObj.GetComponent<Button>().enabled = true;
                SetParent(LowerUI.transform.Find("Progress Gauge_BG"));

                Message.transform.localPosition = new Vector3(0, -350);
                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "버튼 위의 게이지는 진행 단계 게이지입니다.";
                Num++;
                break;
            case 25:
                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "현재 진행 단계에 따라 게이지가 올라갑니다.";
                Num++;
                break;
            case 26:
                //SetParent(LowerUI.transform.Find("Progress Gauge_BG"));

                Message.transform.localPosition = new Vector3(0, 0);
                Message.transform.GetChild(0).GetComponent<Text>().text
                    = "그럼 지금부터 본격적으로 연극을 올리기 위한 과정을 설명해드리겠습니다.";
                Num++;
                break;
        }

        tutoDel = () =>
        {
            Tutorial(Num);
        };

        BackGround.GetComponent<Button>().onClick.RemoveAllListeners();
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

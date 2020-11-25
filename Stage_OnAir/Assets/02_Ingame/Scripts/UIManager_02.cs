using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
}

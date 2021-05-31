using BackEnd;
using Define;
using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public GameObject Popup_Shop;
    public GameObject Popup_ShopUp;
    public GameObject Popup_ShopCk;
    [Header("Error")]
    public GameObject Popup_Error;
    public Text Error_Text;
    private string Error_Message;

    public enum PopupList
    {
        Option = 0,  //  0
        Rank,        //  1
        Audition,    //  2
        Period,      //  3
        Prepare,     //  4
        Marketing,   //  5
        MarketingUp, //  6
        MarketingCk, //  7
        Develop,     //  8
        DevelopUp,   //  9
        DevelopCk,   //  10
        Play,        //  11
        Staff,       //  12
        StaffUp,     //  13
        StaffCk,     //  14
        Shop,        //  15
        ShopUp,      //  16
        ShopCk,      //  17
        Error,       //  18
        Warning,     //  19
        LoansCk,     //  20
        Tutorial,    //  21
        Reset,        //  22
        AD         //23
    }

    public void Popup_On(int Popup)
    {
        PopupList Select = (PopupList)Popup;

        switch (Select)
        {
            case PopupList.Shop:
                //SetShopItem();
                Popup_Shop.SetActive(true);
                break;
            case PopupList.ShopUp:
                Popup_Quit((int)PopupList.Shop);
                Popup_ShopUp.SetActive(true);
                break;
            case PopupList.ShopCk:
                Popup_ShopCk.SetActive(true);
                break;
            case PopupList.Error:
                Error_Text.text = Error_Message;
                Popup_Error.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void Popup_Quit()
    {
        Popup_Shop.SetActive(false);
        Popup_ShopUp.SetActive(false);
        Popup_ShopCk.SetActive(false);

        Popup_Error.SetActive(false);
    }

    public void Popup_Quit(int Popup)
    {
        PopupList Select = (PopupList)Popup;

        switch (Select)
        {
            case PopupList.Shop:
                //Close_Item(Popup_Shop);
                Popup_Shop.SetActive(false);
                break;
            case PopupList.ShopUp:
                Popup_On((int)PopupList.Shop);
                Popup_ShopUp.SetActive(false);
                break;
            case PopupList.ShopCk:
                Popup_ShopCk.SetActive(false);
                break;
            case PopupList.Error:
                Popup_Error.SetActive(false);
                break;
            default:
                break;
        }
    }

    #region Shop
    public void Open_Shop_Popup(int ItemCode)
    {
        GameObject obj;
        Button purchaseBtn;
        IAPButton IAPBtn;
        GameObject Item;
        Sprite Icon;
        string Name;
        string Script;

        Popup_On((int)PopupList.ShopUp);
        obj = Popup_ShopUp;
        purchaseBtn = obj.transform.GetChild(6).GetComponent<Button>();
        IAPBtn = obj.transform.GetChild(6).GetComponent<IAPButton>();
        Item = Popup_Shop.transform.GetChild(3).GetChild(0).GetChild(ItemCode).gameObject;
        Icon = Item.transform.Find("Image").GetComponent<Image>().sprite;
        Name = Item.transform.Find("Name").GetComponent<Text>().text;

        purchaseBtn.onClick.RemoveAllListeners();

        IAPBtn.onPurchaseComplete.RemoveAllListeners();
        IAPBtn.onPurchaseFailed.RemoveAllListeners();

        switch (ItemCode)
        {
            case 0:
                Script = "현재 보유 금액의 10% 획득\n"
                        + "광고 시청";
                purchaseBtn.onClick.AddListener(() => Popup_On(23));
                break;
            case 1:
                Script = "공연 후 광고가 더 이상 나오지 않는다.\n"
                        + "₩2,500";
                //obj.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(() => Shop_Item_2());
                IAPBtn.productId = IAPID.ANDROID_REMOVEAD;
                IAPBtn.onPurchaseComplete.AddListener(product => Shop_Item_2());
                IAPBtn.onPurchaseFailed.AddListener((product, call) => Shop_FailPurchasing(call));
                break;
            case 2:
                Script = "+ 10,000,000원\n"
                        + "+ 첫 수익 획득량 100% 증가\n"
                        + "+ 첫 공연 성공률 100% 증가\n"
                        + "₩5,000";
                //obj.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(() => Shop_Item_3());
                IAPBtn.productId = IAPID.ANDROID_PACKAGESTART;
                IAPBtn.onPurchaseComplete.AddListener(call => Shop_Item_3());
                IAPBtn.onPurchaseFailed.AddListener((product, call) => Shop_FailPurchasing(call));
                break;
            case 3:
                Script = "+ 5,000,000원\n"
                        + "₩5,000";
                //obj.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(() => Shop_Item_4());
                IAPBtn.productId = IAPID.ANDROID_MONEY500;
                IAPBtn.onPurchaseComplete.AddListener(call => Shop_Item_4());
                IAPBtn.onPurchaseFailed.AddListener((product, call) => Shop_FailPurchasing(call));
                break;
            case 4:
                Script = "+ 10,000,000원\n"
                        + "₩10,000";
                //obj.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(() => Shop_Item_5());
                IAPBtn.productId = IAPID.ANDROID_MONEY1000;
                IAPBtn.onPurchaseComplete.AddListener(call => Shop_Item_5());
                IAPBtn.onPurchaseFailed.AddListener((product, call) => Shop_FailPurchasing(call));
                break;
            case 5:
                Script = "+ 50,000,000원\n"
                        + "₩30,000";
                //obj.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(() => Shop_Item_6());
                IAPBtn.productId = IAPID.ANDROID_MONEY5000;
                IAPBtn.onPurchaseComplete.AddListener(call => Shop_Item_6());
                IAPBtn.onPurchaseFailed.AddListener((product, call) => Shop_FailPurchasing(call));
                break;
            case 6:
                Script = "+ 100,000,000원\n"
                        + "₩50,000";
                //obj.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(() => Shop_Item_7());
                IAPBtn.productId = IAPID.ANDROID_MONEY10000;
                IAPBtn.onPurchaseComplete.AddListener(call => Shop_Item_7());
                IAPBtn.onPurchaseFailed.AddListener((product, call) => Shop_FailPurchasing(call));
                break;
            default:
                Script = "게임에 벌레가 날아다닌다~";
                break;
        }

        obj.transform.GetChild(6).GetChild(0).GetComponent<Text>().text = "구매";

        obj.transform.GetChild(3).GetChild(0).GetComponent<Image>().sprite = Icon;
        obj.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = Name;
        obj.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = Script;
    }

    public void Shop_Item_1()
    {
        GoogleAdsManager.Instance.RewardAdsShow();

        DateTime OldTime = DateTime.Now;
        string st = OldTime.ToString("yyyyMMddHHmmss"); // string 으로 변환
        Debug.LogError(st);

        PlayerPrefs.SetString(PLAYERPREFSLIST.AD, st);

        Popup_Quit((int)PopupList.ShopUp);
        Popup_Quit((int)PopupList.AD);
    }

    public void Shop_Item_2()
    {
        //if (IAPManager.Instance.HadPruchased(IAPManager.Product_RemoveAd))
        //{
        //    Debug.Log("이미 구매한 상품입니다.");
        //    Error_Message = ERROR_MESSAGE.PURCHASING_DUPLICATE;
        //    Popup_On((int)PopupList.Error);
        //    return;
        //}
        //
        //IAPManager.Instance.Purchase(IAPManager.Product_RemoveAd);
        //
        //if (IAPManager.Instance.IsSuccessPurchase == false)
        //{ 
        //    Shop_FailPurchasing(IAPManager.Instance.FailReason);
        //    return;
        //}

        Popup_ShopCk.transform.GetChild(3).GetComponent<Text>().text
            = "성공적으로 \n『광고 제거』를\n구매하였습니다.";
        Popup_ShopCk.transform.GetChild(4).GetComponent<Text>().text
            = "이제 공연 개시 전에 광고가 나오지 않습니다.";

        Popup_On((int)PopupList.ShopCk);


        string InDate = Backend.BMember.GetUserInfo().GetInDate();
        var Info = Backend.GameSchemaInfo.Get("Shop", InDate);

        Param param = new Param();
        param.Add("Adblock", true);

        string InfoInDate = Info.Rows()[0]["inDate"]["S"].ToString();
        Backend.GameSchemaInfo.Update("Shop", InfoInDate, param); // 동기

        GameManager.Instance.SetShopData();

        Popup_Quit((int)PopupList.ShopUp);
    }

    public void Shop_Item_3()
    {
        //if (IAPManager.Instance.HadPruchased(IAPManager.Product_PackageStart))
        //{
        //    Debug.Log("이미 구매한 상품입니다.");
        //    Error_Message = ERROR_MESSAGE.PURCHASING_DUPLICATE;
        //    Popup_On((int)PopupList.Error);
        //    return;
        //}
        //
        //IAPManager.Instance.Purchase(IAPManager.Product_PackageStart);
        //
        //if (IAPManager.Instance.IsSuccessPurchase == false)
        //{
        //    Shop_FailPurchasing(IAPManager.Instance.FailReason);
        //    return;
        //}

        Popup_ShopCk.transform.GetChild(3).GetComponent<Text>().text
            = "성공적으로 \n『스타트 패키지』를\n구매하였습니다.";
        Popup_ShopCk.transform.GetChild(4).GetComponent<Text>().text
            = "보유금액: " + GameManager.Instance.Money.ToString("N0") + " -> "
            + (GameManager.Instance.Money + 10000000).ToString("N0");

        Popup_On((int)PopupList.ShopCk);

        GameManager.Instance.CostMoney(10000000, false);

        string InDate = Backend.BMember.GetUserInfo().GetInDate();
        var Info = Backend.GameSchemaInfo.Get("Shop", InDate);

        Param param = new Param();
        param.Add("StartPackage", true);
        param.Add("UseStartPackage", true);

        string InfoInDate = Info.Rows()[0]["inDate"]["S"].ToString();
        Backend.GameSchemaInfo.Update("Shop", InfoInDate, param); // 동기

        GameManager.Instance.SetShopData();
        GameManager.Instance.PackageCallback();

        Debug.Log(GameManager.Instance.OnPackage);
        Debug.Log(GameManager.Instance.UsePackage);

        Popup_Quit((int)PopupList.ShopUp);
    }

    public void Shop_Item_4()
    {
        //IAPManager.Instance.Purchase(IAPManager.Product_Money500);
        //
        //if (IAPManager.Instance.IsSuccessPurchase == false)
        //{
        //    Shop_FailPurchasing(IAPManager.Instance.FailReason);
        //    return;
        //}

        Popup_ShopCk.transform.GetChild(3).GetComponent<Text>().text
            = "성공적으로 \n『5,000,000원』을\n구매하였습니다.";
        Popup_ShopCk.transform.GetChild(4).GetComponent<Text>().text
            = "보유금액: " + GameManager.Instance.Money.ToString("N0") + " -> "
            + (GameManager.Instance.Money + 5000000).ToString("N0");

        Popup_On((int)PopupList.ShopCk);

        GameManager.Instance.CostMoney(5000000, false);

        Popup_Quit((int)PopupList.ShopUp);
    }

    public void Shop_Item_5()
    {
        //IAPManager.Instance.Purchase(IAPManager.Product_Money1000);
        //
        //if (IAPManager.Instance.IsSuccessPurchase == false)
        //{
        //    Shop_FailPurchasing(IAPManager.Instance.FailReason);
        //    return;
        //}

        Popup_ShopCk.transform.GetChild(3).GetComponent<Text>().text
            = "성공적으로 \n『10,000,000원』을\n구매하였습니다.";
        Popup_ShopCk.transform.GetChild(4).GetComponent<Text>().text
            = "보유금액: " + GameManager.Instance.Money.ToString("N0") + " -> "
            + (GameManager.Instance.Money + 10000000).ToString("N0");

        Popup_On((int)PopupList.ShopCk);

        GameManager.Instance.CostMoney(10000000, false);

        Popup_Quit((int)PopupList.ShopUp);
    }

    public void Shop_Item_6()
    {
        //IAPManager.Instance.Purchase(IAPManager.Product_Money5000);
        //
        //if (IAPManager.Instance.IsSuccessPurchase == false)
        //{
        //    Shop_FailPurchasing(IAPManager.Instance.FailReason);
        //    return;
        //}

        Popup_ShopCk.transform.GetChild(3).GetComponent<Text>().text
            = "성공적으로 \n『50,000,000원』을\n구매하였습니다.";
        Popup_ShopCk.transform.GetChild(4).GetComponent<Text>().text
            = "보유금액: " + GameManager.Instance.Money.ToString("N0") + " -> "
            + (GameManager.Instance.Money + 50000000).ToString("N0");

        Popup_On((int)PopupList.ShopCk);

        GameManager.Instance.CostMoney(50000000, false);

        Popup_Quit((int)PopupList.ShopUp);
    }

    public void Shop_Item_7()
    {
        //IAPManager.Instance.Purchase(IAPManager.Product_Money10000);
        //
        //if (IAPManager.Instance.IsSuccessPurchase == false)
        //{
        //    Shop_FailPurchasing(IAPManager.Instance.FailReason);
        //    return;
        //}

        Popup_ShopCk.transform.GetChild(3).GetComponent<Text>().text
            = "성공적으로 \n『100,000,000원』을\n구매하였습니다.";
        Popup_ShopCk.transform.GetChild(4).GetComponent<Text>().text
            = "보유금액: " + GameManager.Instance.Money.ToString("N0") + " -> "
            + (GameManager.Instance.Money + 100000000).ToString("N0");

        Popup_On((int)PopupList.ShopCk);

        GameManager.Instance.CostMoney(100000000, false);

        Popup_Quit((int)PopupList.ShopUp);
    }

    public void Shop_FailPurchasing(PurchaseFailureReason reason)
    {
        switch (reason)
        {
            case PurchaseFailureReason.PurchasingUnavailable:
                Error_Message = ERROR_MESSAGE.PURCHASING_FAIL;
                break;
            case PurchaseFailureReason.ExistingPurchasePending:
                Error_Message = ERROR_MESSAGE.PURCHASING_FAIL;
                break;
            case PurchaseFailureReason.ProductUnavailable:
                Error_Message = ERROR_MESSAGE.PURCHASING_FAIL;
                break;
            case PurchaseFailureReason.SignatureInvalid:
                Error_Message = ERROR_MESSAGE.PURCHASING_FAIL;
                break;
            case PurchaseFailureReason.UserCancelled:
                Error_Message = ERROR_MESSAGE.PURCHASING_CANCEL;
                break;
            case PurchaseFailureReason.PaymentDeclined:
                Error_Message = ERROR_MESSAGE.PURCHASING_CANCEL;
                break;
            case PurchaseFailureReason.DuplicateTransaction:
                Error_Message = ERROR_MESSAGE.PURCHASING_FAIL;
                break;
            case PurchaseFailureReason.Unknown:
                Error_Message = ERROR_MESSAGE.PURCHASING_NULL;
                break;
            default:
                Error_Message = ERROR_MESSAGE.PURCHASING_NULL;
                break;
        }

        Popup_On((int)PopupList.Error);
    }
    #endregion

}

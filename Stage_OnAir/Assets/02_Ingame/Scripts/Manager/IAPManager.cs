using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : Singleton<IAPManager>, IStoreListener
{
    public const string Product_RemoveAd = "Remove_Ad";         // NonConsumable
    public const string Product_PackageStart = "Package_Start"; // NonConsumable
    public const string Product_Money500 = "Money_500";         // Consumable
    public const string Product_Money1000 = "Money_1000";       // Consumable
    public const string Product_Money5000 = "Money_5000";       // Consumable
    public const string Product_Money10000 = "Money_10000";     // Consumable


    private const string _android_RemoveAd_ID = "remove_ad";
    private const string _android_PackageStart_ID = "package_start";
    private const string _android_Money500_ID = "money_500";
    private const string _android_Money1000_ID = "money_1000";
    private const string _android_Money5000_ID = "money_5000";
    private const string _android_Money10000_ID = "money_10000";

    private IStoreController storeController;           // 구매 과정을 제어하는 함수 및 정보 제공
    private IExtensionProvider storeExtensionProvider;  // 여러 플랫폼을 위한 확장 처리 제공

    public bool IsInitialized => storeController != null && storeExtensionProvider != null;

    public bool IsSuccessPurchase = false;

    public PurchaseFailureReason FailReason;

    new void Awake()
    {
        base.Awake();

        InitUnityIAP();
    }

    void InitUnityIAP()
    {
        if (IsInitialized) return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        #region Product
        builder.AddProduct(
            Product_RemoveAd, ProductType.NonConsumable, new IDs()
            {
                {_android_RemoveAd_ID, GooglePlay.Name }
            });

        builder.AddProduct(
            Product_PackageStart, ProductType.NonConsumable, new IDs()
            {
                {_android_PackageStart_ID, GooglePlay.Name }
            });

        builder.AddProduct(
            Product_Money500, ProductType.Consumable, new IDs()
            {
                {_android_Money500_ID, GooglePlay.Name }
            });

        builder.AddProduct(
            Product_Money1000, ProductType.Consumable, new IDs()
            {
                {_android_Money1000_ID, GooglePlay.Name }
            });

        builder.AddProduct(
            Product_Money5000, ProductType.Consumable, new IDs()
            {
                {_android_Money5000_ID, GooglePlay.Name }
            });

        builder.AddProduct(
            Product_Money10000, ProductType.Consumable, new IDs()
            {
                {_android_Money10000_ID, GooglePlay.Name }
            });
        #endregion

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("유니티 IAP 초기화");
        storeController = controller;
        storeExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"유니티 IAP 초기화 실패 {error}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log($"구매 성공 - ID : {args.purchasedProduct.definition.id}");
        IsSuccessPurchase = true;

        if (args.purchasedProduct.definition.id == Product_RemoveAd)
        {
            Debug.Log("광고 삭제 구매");
        }
        else if (args.purchasedProduct.definition.id == Product_PackageStart)
        {
            Debug.Log("패키지 구매");
        }
        else if (args.purchasedProduct.definition.id == Product_Money500)
        {
            Debug.Log("500만원 구매");
        }
        else if (args.purchasedProduct.definition.id == Product_Money1000)
        {
            Debug.Log("1000만원 구매");
        }
        else if (args.purchasedProduct.definition.id == Product_Money5000)
        {
            Debug.Log("5000만원 구매");
        }
        else if (args.purchasedProduct.definition.id == Product_Money10000)
        {
            Debug.Log("1억원 구매");
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.LogWarning($"구매 실패 - {product.definition.id}, {reason}");
        IsSuccessPurchase = false;
        FailReason = reason;
    }

    public void Purchase(string productId)
    {
        if (!IsInitialized) return;

        var product = storeController.products.WithID(productId);

        if (product != null && product.availableToPurchase)
        {
            Debug.Log($"구매 시도 - {product.definition.id}");
            storeController.InitiatePurchase(product);
        }
        else
        {
            Debug.LogError($"구매 시도 불가 - {productId}");
        }
    }

    public void RestorePurchase()
    {
        if (!IsInitialized) return;

        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("구매 복구 시도");
        }
    }

    public bool HadPruchased(string productId)
    {
        if (!IsInitialized) return false;

        var product = storeController.products.WithID(productId);

        if (product != null)
        {
            return product.hasReceipt;
        }

        return false;
    }
}

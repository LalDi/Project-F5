using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackageUI : MonoBehaviour
{
    [SerializeField] private Button Button_Package;
    [SerializeField] private Image Image_SoldOut;
    [SerializeField] private Image Image_UsingPackage;

    private void Awake()
    {
        GameManager.Instance.PackageCallback = SetPackageUI;
    }

    void Start()
    {
        SetPackageUI();
    }

    public void SetPackageUI()
    {
        if (GameManager.Instance.OnPackage == true)
        {
            Button_Package.interactable = false;
            Image_SoldOut.gameObject.SetActive(true);

            Image_UsingPackage.gameObject.SetActive(GameManager.Instance.UsePackage);
        }
        else
        {
            Button_Package.interactable = true;
            Image_SoldOut.gameObject.SetActive(false);
            Image_UsingPackage.gameObject.SetActive(false);
        }
    }
}

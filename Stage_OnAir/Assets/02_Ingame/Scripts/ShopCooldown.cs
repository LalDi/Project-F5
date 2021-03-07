using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Define;

public class ShopCooldown : MonoBehaviour
{
    public Button btn;
    public Image Cooldown;
    private DateTime OldTime;

    private void Start()
    {
        string st = DateTime.Now.ToString("yyyyMMddHHmmss"); // string 으로 변환

        string Time = PlayerPrefs.GetString(PLAYERPREFSLIST.AD);
        OldTime = DateTime.ParseExact(Time, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
    }

    private void Update()
    {
        DateTime NowTime = DateTime.Now;

        TimeSpan time = NowTime - OldTime;
        //
        //if (time.TotalMinutes < 30)
        //{
        //    btn.interactable = false;
        //
        //    Cooldown.fillAmount = 1 - ((float)time.TotalMinutes / 30f);
        //}
        //else
        //{
        //    string Time = PlayerPrefs.GetString(PLAYERPREFSLIST.AD);
        //    OldTime = DateTime.ParseExact(Time, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
        //
        //    btn.interactable = true;
        //
        //    Cooldown.fillAmount = 0;
        //}

        if (time.TotalSeconds < 30)
        {
            btn.interactable = false;

            Cooldown.fillAmount = 1 - ((float)time.TotalSeconds / 30f);
        }
        else
        {
            string Time = PlayerPrefs.GetString(PLAYERPREFSLIST.AD);
            OldTime = DateTime.ParseExact(Time, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);

            btn.interactable = true;

            Cooldown.fillAmount = 0;
        }
    }
}

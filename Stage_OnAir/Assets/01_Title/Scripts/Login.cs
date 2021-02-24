using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using Define;

public class Login : MonoBehaviour
{
    [Header("Login")]
    public InputField Login_ID;
    public InputField Login_Password;

    [Header("SignUp")]
    public InputField SignUp_ID;
    public InputField SignUp_Password;
    public InputField SignUp_PWCheck;

    [Header("NickName")]
    public GameObject Nickname_Popup;
    public InputField Nickname_Nickname;

    [Header("Loading")]
    public GameObject Popup_Loading;

    [Header("Error")]
    public GameObject Popup_Error;
    public Text Error_Text;

    private string Error_Message;
    public GameObject TutorialObj;

    public void Control_Error(bool Open)
    {
        Popup_Error.SetActive(Open);
        Error_Text.text = Error_Message;
    }

    public void Try_Login()
    {
        Popup_Loading.SetActive(true);
        var Login = Backend.BMember.CustomLogin(Login_ID.text, Login_Password.text);
        Popup_Loading.SetActive(false);

        Debug.Log(Login);

        switch (Login.GetStatusCode())
        {
            case "200":
                SoundManager.Instance.PlaySound("Prize_Wheel_Spin_2_Reward");
                Backend.Chart.GetAllChartAndSave(true);
                ScenarioData.Instance.SetScenarioData();
                ActorData.Instance.SetActorsData();
                MarketingData.Instance.SetMarketingData();
                Items.Instance.SetStaffData();
                GameManager.Instance.LoadData();
                LoadManager.Load(LoadManager.Scene.Ingame);
                break;
            case "400":
                Error_Message = ERROR_MESSAGE.LOGIN_EMPTY;
                Control_Error(true);
                break;
            case "401":
                Error_Message = ERROR_MESSAGE.LOGIN_DUPLICATE;
                Control_Error(true);
                break;
            case "403":
                Error_Message = Login.GetErrorCode();
                Control_Error(true);
                break;
            default:
                break;
        }
    }

    public void SignUp_Custom()
    {
        if (SignUp_Password.text == SignUp_PWCheck.text && SignUp_Password.text != "")
        {
            Popup_Loading.SetActive(true);

            var SignUp = Backend.BMember.CustomSignUp(SignUp_ID.text, SignUp_Password.text);

            Popup_Loading.SetActive(false);

            Debug.Log(SignUp);

            switch (SignUp.GetStatusCode())
            {
                case "201":
                    SoundManager.Instance.PlaySound("Prize_Wheel_Spin_2_Reward");
                    Backend.BMember.CustomLogin(SignUp_ID.text, SignUp_Password.text);
                    Nickname_Popup.SetActive(true);
                    break;
                case "400":
                    Error_Message = ERROR_MESSAGE.SIGNUP_EMPTY;
                    Control_Error(true);
                    break;
                case "409":
                    Error_Message = ERROR_MESSAGE.SIGNUP_DUPLICATE;
                    Control_Error(true);
                    break;
                default:
                    break;
            }
        }
        else
        {
            Error_Message = ERROR_MESSAGE.SIGNUP_DISCORDANCE;
            Control_Error(true);
        }
    }

    public void SignUp_NickName()
    {
        Popup_Loading.SetActive(true);

        var Nickname = Backend.BMember.UpdateNickname(Nickname_Nickname.text);

        Popup_Loading.SetActive(false);

        Debug.Log(Nickname);

        switch (Nickname.GetStatusCode())
        {
            case "204":
                SoundManager.Instance.PlaySound("Prize_Wheel_Spin_2_Reward");
                Backend.Chart.GetAllChartAndSave(true);
                ScenarioData.Instance.SetScenarioData();
                ActorData.Instance.SetActorsData();
                MarketingData.Instance.SetMarketingData();
                Items.Instance.SetStaffData();
                GameManager.Instance.LoadData();
                GameManager.Instance.Tutorial = true;
                TutorialObj.SetActive(true);
                LoadManager.Load(LoadManager.Scene.Ingame);
                break;
            case "400":
                switch (Nickname.GetErrorCode())
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
}

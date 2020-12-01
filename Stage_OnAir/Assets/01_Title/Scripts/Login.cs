using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

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
                LoadManager.Load(LoadManager.Scene.Ingame);
                break;
            case "400":
                Error_Message = "모든 항목을 빠짐없이 입력하여주십시오.";
                Control_Error(true);
                break;
            case "401":
                Error_Message = "존재하지 않는 계정입니다.\n아이디 혹은 비밀번호를 확인해주세요.";
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
                    Backend.BMember.CustomLogin(SignUp_ID.text, SignUp_Password.text);
                    Nickname_Popup.SetActive(true);
                    break;
                case "400":
                    Error_Message = "모든 항목을 빠짐없이 입력하여주십시오.";
                    Control_Error(true);
                    break;
                case "409":
                    Error_Message = "이미 존재하는 아이디입니다.\n아이디를 바꿔주십시오.";
                    Control_Error(true);
                    break;
                default:
                    break;
            }
        }
        else
        {
            Error_Message = "비밀번호가 일치하지 않습니다.";
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
                LoadManager.Load(LoadManager.Scene.Ingame);
                break;
            case "400":
                switch (Nickname.GetErrorCode())
                {
                    case "UndefinedParameterException":
                        Error_Message = "닉네임은 공백으로 설정할 수 없습니다.";
                        Control_Error(true);
                        break;
                    case "BadParameterException":
                        Error_Message = "닉네임은 20자를 넘기거나\n공백을 포함할 수 없습니다.";
                        Control_Error(true);
                        break;
                }
                break;
            case "409":
                Error_Message = "중복된 닉네임입니다.\n다른 닉네임을 설정해주십시오.";
                Control_Error(true);
                break;
        }

    }
}

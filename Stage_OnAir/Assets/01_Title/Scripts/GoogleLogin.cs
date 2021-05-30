using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using BackEnd;
using Define;

public class GoogleLogin : MonoBehaviour
{
    [Header("NickName")]
    public GameObject Nickname_Popup;
    public InputField Nickname_Inputfield;

    [Header("GameObject")]
    public GameObject Popup_Loading;

    [Header("Error")]
    public GameObject Popup_Error;
    public Text Error_Text;

    private string Error_Message;

    // GPGS 로그인 
    void Start()
    {
        // GPGS 플러그인 설정
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
            .Builder()
            .RequestServerAuthCode(false)
            .RequestEmail() // 이메일 권한을 얻고 싶지 않다면 해당 줄(RequestEmail)을 지워주세요.
            .RequestIdToken()
            .Build();
        //커스텀된 정보로 GPGS 초기화
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true; // 디버그 로그를 보고싶지 않다면 false로 바꿔주세요.
                                                  //GPGS 시작.
        PlayGamesPlatform.Activate();
    }

    public void GPGSLogin()
    {
        Popup_Loading.SetActive(true);

        // 이미 로그인 된 경우
        if (Social.localUser.authenticated == true)
        {
            //BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs");
            Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs", callback =>
            {
                if (callback.IsSuccess())
                {
                    SoundManager.Instance.PlaySound("Prize_Wheel_Spin_2_Reward");
                    GameManager.Instance.Init();
                    LoadManager.Load(LoadManager.Scene.Ingame);
                }
            });
        }
        else
        {
            Social.localUser.Authenticate((bool success) => {
                if (success)
                {
                    // 로그인 성공 -> 뒤끝 서버에 획득한 구글 토큰으로 가입요청
                    //BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs");
                    Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs", callback =>
                    {
                        if (callback.IsSuccess())
                        {
                            SoundManager.Instance.PlaySound("Prize_Wheel_Spin_2_Reward");
                            Popup_Loading.SetActive(false);
                            Nickname_Popup.SetActive(true);
                        }
                    });
                }
                else
                {
                    // 로그인 실패
                    Debug.Log("Login failed for some reason");
                }
            });
        }
    }

    public void SignUp_NickName()
    {
        Popup_Loading.SetActive(true);
        SoundManager.Instance.PlaySound("Prize_Wheel_Spin_2_Reward");

        Backend.BMember.UpdateNickname(Nickname_Inputfield.text, callback =>
        {
            Popup_Loading.SetActive(false);

            if (callback.IsSuccess())
            {
                GameManager.Instance.Init();
                LoadManager.Load(LoadManager.Scene.Ingame);
            }
            else
            {
                switch(callback.GetErrorCode())
                {
                    case "DuplicatedParameterException":
                        Error_Message = ERROR_MESSAGE.SETNICK_DUPLICATE;
                        Control_Error(true);
                        break;
                    case "UndefinedParameterException":
                        Error_Message = ERROR_MESSAGE.SETNICK_EMPTY;
                        Control_Error(true);
                        break;
                    case "BadParameterException":
                        Error_Message = ERROR_MESSAGE.SETNICK_BAD;
                        Control_Error(true);
                        break;

                }
            }
        });
    }

    // 구글 토큰 받아옴
    public string GetTokens()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            // 유저 토큰 받기 첫번째 방법
            string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();
            // 두번째 방법
            // string _IDtoken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
            return _IDtoken;
        }
        else
        {
            Debug.Log("접속되어있지 않습니다. PlayGamesPlatform.Instance.localUser.authenticated :  fail");
            return null;
        }
    }
    
    public void Control_Error(bool Open)
    {
        Popup_Error.SetActive(Open);
        Error_Text.text = Error_Message;
    }

}

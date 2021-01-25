using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UnityEngine.UI;

public class MemberSampleDotnet4 : MonoBehaviour
{

    //비동기로 가입, 로그인을 할때에는 Update()에서 처리를 해야합니다. 이 값은 Update에서 구현하기 위한 플래그 값 입니다.
    BackendReturnObject bro = new BackendReturnObject();
    bool isSuccess = false;

    string updatedPW = "c8xtwbeu";
    string email = "초기화된 비밀번호 받을 이메일 주소";

    public InputField idInput;
    public InputField PWInput;
    public InputField NicknameInput;

    void Start()
    {
        //Backend.Initialize(BRO =>
        //{
        //    Debug.Log("Backend.Initialize "+BRO);
        //    // 성공
        //    if(BRO.IsSuccess())
        //    {
        //        // 구글 해시키 획득 
        //        if (!Backend.Utils.GetGoogleHash().Equals(""))
        //            Debug.Log(Backend.Utils.GetGoogleHash());

        //        // 서버시간 획득
        //        Debug.Log(Backend.Utils.GetServerTime());
        //    }
        //    // 실패
        //    else
        //    {
        //        Debug.LogError("Failed to initialize the backend");
        //    }
        //});

        Backend.Initialize(HandleBackendCallback);
    }

    void HandleBackendCallback()
    {
        if (Backend.IsInitialized)
        {
            // 구글 해시키 획득 
            if (!Backend.Utils.GetGoogleHash().Equals(""))
                Debug.Log(Backend.Utils.GetGoogleHash());

            // 서버시간 획득
            Debug.Log(Backend.Utils.GetServerTime());
        }
        // 실패
        else
        {
            Debug.LogError("Failed to initialize the backend");
        }
    }

    bool InputFieldEmptyCheck(InputField inputField)
    {
        return inputField != null && !string.IsNullOrEmpty(inputField.text);
    }

    // 커스텀 가입
    public void CustomSignUp()
    {
        Debug.Log("-------------CustomSignUp-------------");
        if (InputFieldEmptyCheck(idInput) && InputFieldEmptyCheck(PWInput))
        {
            Debug.Log(Backend.BMember.CustomSignUp(idInput.text, PWInput.text, "tester").ToString());
        }
        else
        {
            Debug.Log("check IDInput or PWInput");
        }
    }

    public void ACustomSignUp()
    {
        Debug.Log("-------------ACustomSignUp-------------");
        if (InputFieldEmptyCheck(idInput) && InputFieldEmptyCheck(PWInput))
        {
           Backend.BMember.CustomSignUp( idInput.text, PWInput.text, "tester", isComplete =>
           {
               Debug.Log(isComplete.ToString());

           });
        }
        else
        {
            Debug.Log("check IDInput or PWInput");
        }
    }

    // 커스텀 로그인
    public void CustomLogin()
    {
        Debug.Log("-------------CustomLogin-------------");
        if (InputFieldEmptyCheck(idInput) && InputFieldEmptyCheck(PWInput))
        {
            Debug.Log(Backend.BMember.CustomLogin(idInput.text, PWInput.text, "tester").ToString());
        }
        else
        {
            Debug.Log("check IDInput or PWInput");
        }

    }

    public void ACustomLogin()
    {
        Debug.Log("-------------ACustomLogin-------------");
        if (InputFieldEmptyCheck(idInput) && InputFieldEmptyCheck(PWInput))
        {
            Backend.BMember.CustomLogin( idInput.text, PWInput.text, "tester", isComplete =>
           {
               Debug.Log(isComplete.ToString());

           });
        }
        else
        {
            Debug.Log("check IDInput or PWInput");
        }
    }

    // 기기에 저장된 뒤끝 AccessToken으로 로그인 (페더레이션, 커스텀 회원가입 또는 로그인 이후에 시도 가능)
    public void LoginWithTheBackendToken()
    {
        Debug.Log("-------------LoginWithTheBackendToken-------------");
        Debug.Log(Backend.BMember.LoginWithTheBackendToken().ToString());
    }

    public void ALoginWithTheBackendToken()
    {
        Debug.Log("-------------ALoginWithTheBackendToken-------------");
        Backend.BMember.LoginWithTheBackendToken( isComplete =>
       {
           Debug.Log(isComplete.ToString());
       });
    }


    //뒤끝 RefreshToken 을 통해 뒤끝 AccessToken 을 재발급 받습니다
    public void RefreshTheBackendToken()
    {
        Debug.Log("-------------RefreshTheBackendToken-------------");
        Debug.Log(Backend.BMember.RefreshTheBackendToken().ToString());
    }

    public void ARefreshTheBackendToken()
    {
        Debug.Log("-------------ARefreshTheBackendToken-------------");
        // RefreshTheBackendToken 대신 RefreshTheBackendTokenAsync 사용
        Backend.BMember.RefreshTheBackendToken( isComplete =>
       {
           // 성공시 - Update() 문에서 토큰 저장
           Debug.Log(isComplete.ToString());
           isSuccess = isComplete.IsSuccess();
           bro = isComplete;
       });
    }

    // 서버에서 뒤끝 access_token과 refresh_token을 삭제
    public void Logout()
    {
        Debug.Log("-------------Logout-------------");
        Debug.Log(Backend.BMember.Logout().ToString());
    }

    public void ALogout()
    {
        Debug.Log("-------------ALogout-------------");
        Backend.BMember.Logout( isComplete =>
       {
           Debug.Log(isComplete.ToString());
       });
    }

    // 회원 탈퇴 
    public void SignOut()
    {
        Debug.Log("-------------SignOut-------------");
        Debug.Log(Backend.BMember.SignOut("탈퇴 사유").ToString());
    }

    public void ASignOut()
    {
        Debug.Log("-------------ASignOut-------------");
        Backend.BMember.SignOut( "탈퇴 사유", isComplete =>
        {
            Debug.Log(isComplete.ToString());
        });
    }

    public void CheckNicknameDuplication()
    {
        Debug.Log("-------------CheckNicknameDuplication-------------");
        if (InputFieldEmptyCheck(NicknameInput))
        {
            Debug.Log(Backend.BMember.CheckNicknameDuplication(NicknameInput.text).ToString());
        }
        else
        {
            Debug.Log("check NicknameInput");
        }
    }

    public void ACheckNicknameDuplication()
    {
        Debug.Log("-------------A CheckNicknameDuplication-------------");

        if (InputFieldEmptyCheck(NicknameInput))
        {
            Backend.BMember.CheckNicknameDuplication( NicknameInput.text, bro =>
            {
                Debug.Log(bro);
            });
        }
        else
        {
            Debug.Log("check NicknameInput");
        }
    }


    // 닉네임 생성 
    public void CreateNickname()
    {
        Debug.Log("-------------CreateNickname-------------");
        if (InputFieldEmptyCheck(NicknameInput))
        {
            Debug.Log(Backend.BMember.CreateNickname(NicknameInput.text).ToString());
        }
        else
        {
            Debug.Log("check NicknameInput");
        }
    }

    public void ACreateNickname()
    {
        Debug.Log("-------------ACreateNickname-------------");
        if (InputFieldEmptyCheck(NicknameInput))
        {
            Backend.BMember.CreateNickname( NicknameInput.text, isComplete =>
           {
               Debug.Log(isComplete.ToString());
           });
        }
        else
        {
            Debug.Log("check NicknameInput");
        }
    }

    // 닉네임 수정
    public void UpdateNickname()
    {
        Debug.Log("-------------UpdateNickname-------------");
        if (InputFieldEmptyCheck(NicknameInput))
        {
            Debug.Log(Backend.BMember.UpdateNickname(NicknameInput.text).ToString());
        }
        else
        {
            Debug.Log("check NicknameInput");
        }
    }

    public void AUpdateNickname()
    {
        Debug.Log("-------------AUpdateNickname-------------");
        if (InputFieldEmptyCheck(NicknameInput))
        {
            Backend.BMember.UpdateNickname( NicknameInput.text, isComplete =>
           {
               Debug.Log(isComplete.ToString());
           });
        }
        else
        {
            Debug.Log("check NicknameInput");
        }
    }

    // 유저 정보 받아오기 - nickname
    public void GetUserInfo()
    {
        Debug.Log("-------------GetUserInfo-------------");
        BackendReturnObject userinfo = Backend.BMember.GetUserInfo();
        Debug.Log(userinfo);

        //text.text = userinfo.ToString();


        if (userinfo.IsSuccess())
        {
            JsonData Userdata = userinfo.GetReturnValuetoJSON()["row"];
            JsonData nicknameJson = Userdata["nickname"];

            // 닉네임 여부를 확인 하는 로직
            if (nicknameJson != null)
            {
                string nick = nicknameJson.ToString();
                Debug.Log("NickName is NOT null which is " + nick);
            }
            else
            {
                Debug.Log("NickName is null");
            }
        }

    }

    public void AGetUserInfo()
    {
        Debug.Log("-------------AGetUserInfo-------------");
        Backend.BMember.GetUserInfo( userinfo =>
       {
           Debug.Log(userinfo.ToString());
           JsonData Userdata = userinfo.GetReturnValuetoJSON()["row"];
           JsonData nicknameJson = Userdata["nickname"];

           // 닉네임 여부를 확인 하는 로직
           if (nicknameJson != null)
           {
               string nick = nicknameJson.ToString();
               Debug.Log("NickName is NOT null which is " + nick);
           }
           else
           {
               Debug.Log("NickName is null");
           }
       });
    }

    // 푸시 토큰 입력
    public void PutDeviceToken()
    {
        Debug.Log("-------------PutDeviceToken-------------");
#if UNITY_ANDROID
        try{
        bro = Backend.Android.PutDeviceToken();
            Debug.Log(bro);
        //text.text = bro.ToString();
        }catch(Exception e){
            Debug.Log(e);
        }
#else
        Debug.Log(Backend.iOS.PutDeviceToken(isDevelopment.iosDev));
#endif
    }

    public void APutDeviceToken()
    {
        Debug.Log("-------------APutDeviceToken-------------");
#if UNITY_ANDROID
        Backend.Android.PutDeviceToken( Backend.Android.GetDeviceToken(), bro =>
        {
            Debug.Log(bro);
        });
#else
        Backend.iOS.PutDeviceToken(isDevelopment.iosDev, bro =>
        {
            Debug.Log(bro);
        });
#endif
    }

    // 푸시 토큰 삭제
    public void DeleteDeviceToken()
    {
        Debug.Log("-------------DeleteDeviceToken-------------");
#if UNITY_ANDROID
        Debug.Log(Backend.Android.DeleteDeviceToken());
#else
        Debug.Log(Backend.iOS.DeleteDeviceToken());
#endif
    }

    public void ADeleteDeviceToken()
    {
        Debug.Log("-------------ADeleteDeviceToken-------------");
#if UNITY_ANDROID
        Backend.Android.DeleteDeviceToken( bro =>
        {
            Debug.Log(bro);
        });
#else
        Backend.iOS.DeleteDeviceToken(bro =>
        {
            Debug.Log(bro);
        });
#endif
    }


    public void IsAccessTokenAlive()
    {
        Debug.Log("-------------IsAccessTokenAlive-------------");
        Debug.Log(Backend.BMember.IsAccessTokenAlive().ToString());
    }


    public void AIsAccessTokenAlive()
    {
        Debug.Log("-------------A IsAccessTokenAlive-------------");
        Backend.BMember.IsAccessTokenAlive( callback =>
        {
            Debug.Log(callback);
        });
    }



    public void UpdatePasswordResetEmail()
    {
        Debug.Log("-------------UpdatePasswordResetEmail-------------");
        bro = Backend.BMember.UpdateCustomEmail(email);
        Debug.Log(bro);
    }

    public void AUpdatePasswordResetEmail()
    {
        Debug.Log("-------------A UpdatePasswordResetEmail-------------");
        Backend.BMember.UpdateCustomEmail( email, callback =>
        {
            Debug.Log(callback);
        });
    }


    public void ResetPassword()
    {
        Debug.Log("-------------ResetPassword-------------");
        if (InputFieldEmptyCheck(idInput))
        {
            bro = Backend.BMember.ResetPassword(idInput.text, email);
            Debug.Log(bro);
        }
        else
        {
            Debug.Log("check IDInput");
        }
    }

    public void AResetPassword()
    {
        Debug.Log("-------------A ResetPassword-------------");
        if (InputFieldEmptyCheck(idInput))
        {
            Backend.BMember.ResetPassword( idInput.text, email, callback =>
           {
               Debug.Log(callback);
           });
        }
        else
        {
            Debug.Log("check IDInput");
        }
    }

    public void UpdatePassword()
    {
        Debug.Log("-------------UpdatePassword-------------");
        if (InputFieldEmptyCheck(PWInput))
        {
            bro = Backend.BMember.UpdatePassword(updatedPW, PWInput.text);
            Debug.Log(bro);
        }
        else
        {
            Debug.Log("check PWInput");
        }

    }

    public void AUpdatePassword()
    {
        Debug.Log("-------------A UpdatePassword-------------");
        if (InputFieldEmptyCheck(PWInput))
        {
            Backend.BMember.UpdatePassword( updatedPW, PWInput.text, callback =>
            {
                Debug.Log(callback);
            });
        }
        else
        {
            Debug.Log("check PWInput");
        }

    }

    public void GuestLogin()
    {
        Debug.Log("-------------GuestLogin-------------");

        bro = Backend.BMember.GuestLogin();
        Debug.Log(bro);
    }
    public void AGuestLogin()
    {
        Debug.Log("-------------A GuestLogin-------------");

        Backend.BMember.GuestLogin(callback =>
       {
           Debug.Log(callback);
       });

    }
    public void GetGuestID()
    {
        Debug.Log("-------------GetGuestID-------------");
        Debug.Log("게스트 아이디 : " + Backend.BMember.GetGuestID());
    }

    public void DeleteGuestInfo()
    {
        Debug.Log("-------------DeleteGuestInfo-------------");
        Backend.BMember.DeleteGuestInfo();
    }
}
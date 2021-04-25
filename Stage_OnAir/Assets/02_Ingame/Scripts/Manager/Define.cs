namespace Define
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PLAYER
    {
        public const int DEFAULT_MONEY = 5000000;
        public const int DEFAULT_YEAR = 2000;
        public const int DEFAULT_MONTH = 1;
        public const int DEFAULT_SUCCESS = 70;
    }

    public class AD
    {
        public const string APPID = "ca-app-pub-5708876822263347~1141167025";

        public const string INTERSAD = "ca-app-pub-5708876822263347/3693525789";
        public const string REWARDAD = "ca-app-pub-5708876822263347/1351267624";
        public const string BANNERAD = "ca-app-pub-5708876822263347/5714085828";

        public const string TEST_INTERS = "ca-app-pub-3940256099942544/1033173712";
        public const string TEST_REWARD = "ca-app-pub-3940256099942544/5224354917";
        public const string TEST_BANNER = "ca-app-pub-3940256099942544/6300978111";
    }


    public class PLAYERPREFSLIST
    {
        public const string AD = "Ad";
    }

    public class ERROR_MESSAGE
    {
        public const string LOGIN_EMPTY = "모든 항목을 빠짐없이 입력하여주십시오.";
        public const string LOGIN_DUPLICATE = "존재하지 않는 계정입니다.\n아이디 혹은 비밀번호를 확인해주세요.";

        public const string SIGNUP_EMPTY = "모든 항목을 빠짐없이 입력하여주십시오.";
        public const string SIGNUP_DUPLICATE = "이미 존재하는 아이디입니다.\n아이디를 바꿔주십시오.";
        public const string SIGNUP_DISCORDANCE = "비밀번호가 일치하지 않습니다.";

        public const string SETNICK_EMPTY = "닉네임은 공백으로 설정할 수 없습니다.";
        public const string SETNICK_BAD = "닉네임은 20자를 넘기거나\n앞,뒤에 공백을 넣을 수 없습니다.\n(닉네임의 중간에는 공백을 포함할 수 있습니다.)";
        public const string SETNICK_DUPLICATE = "중복된 닉네임입니다.\n다른 닉네임을 설정해주십시오.";
    }

    public class MANAGERDATA
    {
        public enum DATALIST
        {
            QUALITY,
            MARKETING,
            SUCCESS,
            ACTING,
            SCENARIO,
            DIRECTION
        }
    }

    public class RANKING
    {
        public enum RANK
        {
            QUALITY = 1,
            AUDIENCE = 2,
            PROFIT = 3
        };
        public const int RANKCOUNT = 50;

        public const string QUALITY_UUID = "e660b500-36da-11eb-8cf7-ad6ee90f9a15";
        public const string AUDIENCE_UUID = "96cdd2b0-3acd-11eb-acb5-9f1ffd73b1e0";
        public const string PROFIT_UUID = "a08fafd0-3acd-11eb-9b6e-f165d70dce45";

        public static Vector2 SELECT_BT = new Vector2(330, 170);
        public static Vector2 NONSELECT_BT = new Vector2(240, 140);
    }

    public class SCENARIO
    {
        public int SCENARIO_ILLUST_QUALITY(int Quality)
        {
            return Quality * 5;
        }
    }

    public class AUDITION
    {
        public const int AUDITION_PRICE = 50000;
    }

    public class Math
    {
        static public List<T> ShuffleList<T>(List<T> InputList)
        {
            List<T> RandomList = new List<T>();

            System.Random r = new System.Random();
            int RandomIndex = 0;

            while (InputList.Count > 0)
            {
                RandomIndex = r.Next(0, InputList.Count);
                RandomList.Add(InputList[RandomIndex]);
                InputList.RemoveAt(RandomIndex);
            }

            return RandomList;
        }

        static public float DPToPixel(float fFixedResoulutionHeight, float fdpHeight)
        {
            float fNowDpi = (Screen.dpi * fFixedResoulutionHeight) / Screen.height;
            float scale = fNowDpi / 160;
            float pixel = fdpHeight * scale;
            return pixel;
        }

        /**
         *  @return  최종 Quality점수를 계산
         */
        static public float FINALQUALITY()
        {
            float Scenario = GameManager.Instance.Quality_Scenario;     // 시나리오 퀄리티 점수
            float Acting = GameManager.Instance.Quality_Acting;         // 배우 연기력 총합
            float Direction = GameManager.Instance.Quality_Direction;   // 스태프 연출력 총합

            int Actors = GameManager.Instance.NowActor;                 // 시나리오 배우 수

            float result;
            if (Actors != 0)
                result = Scenario + (Scenario * Direction * 0.01f) + (Acting + (Acting * (Actors - 2) * 0.1f) / Actors);
            else
                result = Scenario + (Scenario * Direction * 0.01f);
            //(시나리오 퀄리티) + (시나리오 퀄리티 * 스태프 기술력 * 0.01) + ((배우 연기력 총합 + 배우 연기력 총합 * (배우 수 - 2) * 0.1) / 배우 수)

            return result;
        }

        /**
         *  @return  마케팅 점수를 Ratio당 1.0으로 반환
         */
        static public float MARKETING()
        {
            float Ratio = 500f;
            return GameManager.Instance.Play_Marketing / Ratio;
        }

        /**
         *  @return  최종 수익을 계산
         */
        static public float RESULT()
        {
            //float Quality = GameManager.Instance.Play_Quality;
            float Quality = FINALQUALITY();
            float Marketing = MARKETING();

            float result;
            result = Quality * Marketing * 10000;

            return result;
        }
    }

    [System.Serializable]
    public class Tutorial
    {
        public List<Sprite> Sprites;
    }
    public class StaffMonthly
    {
        //스태프 월급 계산
        static public int MONTHLY()
        {
            //Icon = StaffData.Instance.StaffIcon[Data.Code - 1];

            int Pay = 0;
            foreach (Staff item in GameManager.Instance.Staffs)
            {
                if(item.IsPurchase)
                {
                    Pay += item.Pay;
                }
            }
            return Pay;
        }
    }
}
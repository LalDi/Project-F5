namespace Define
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ERROR_MESSAGE
    {
        public const string LOGIN_EMPTY = "모든 항목을 빠짐없이 입력하여주십시오.";
        public const string LOGIN_DUPLICATE = "존재하지 않는 계정입니다.\n아이디 혹은 비밀번호를 확인해주세요.";

        public const string SIGNUP_EMPTY = "모든 항목을 빠짐없이 입력하여주십시오.";
        public const string SIGNUP_DUPLICATE = "이미 존재하는 아이디입니다.\n아이디를 바꿔주십시오.";
        public const string SIGNUP_DISCORDANCE = "비밀번호가 일치하지 않습니다.";

        public const string SETNICK_EMPTY = "닉네임은 공백으로 설정할 수 없습니다.";
        public const string SETNICK_BAD = "닉네임은 20자를 넘기거나\n공백을 포함할 수 없습니다.";
        public const string SETNICK_DUPLICATE = "중복된 닉네임입니다.\n다른 닉네임을 설정해주십시오.";
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
    }

}
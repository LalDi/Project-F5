namespace Define
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    class SCENARIO
    {
        public int SCENARIO_ILLUST_QUALITY(int Quality)
        {
            return Quality * 5;
        }
    }

    class AUDITION
    {
        public const int AUDITION_PRICE = 50000;
    }

    class Math
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
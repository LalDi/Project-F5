#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("K5kaOSsWHRIxnVOd7BYaGhoeGxiHMjYeX7WTvzRtXWbjlT/DsXLCOZkaFBsrmRoRGZkaGhuBsHLH/ZTK6fCSF2uZGg1r2GZ9gxljQ09tGnckEECpWn5d8C1KVBWZ8qxS/2Ime1qrgxoPtVBzevZkyTkztJXVX/V5288RQgq8/Y+sdm9C1FuSih98ccAeqItqVw64xKK81fbfgZF370ehDCEaKKgcyhboLUABqj83ya4OovjCLhrdHJau6KpO7MDT36lRT+kL/dyUkLo1eTTrXEuloV5khCKgwXxkKnjhdv3HT1R3RjOmjfRxilHlB+NNpt0dmlnrBprUT5EBz9vMd9I1jIs/ouy8xCNUKEKzAc8m3NpQKHIFbVvTluXn3qd01hkYGhsa");
        private static int[] order = new int[] { 0,2,2,5,7,9,7,8,8,10,13,13,12,13,14 };
        private static int key = 27;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif

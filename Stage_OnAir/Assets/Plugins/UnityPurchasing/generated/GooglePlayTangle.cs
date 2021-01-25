#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("uMMDhEf1GITKUY8f0cXSacwrkpX37owJdYcEE3XGeGOdB31dUXMEaUS1nQQRq05tZOh61yctqovLQetnMATDAoiw9rRQ8t7NwbdPUfcV48Jm/2jj2VFKaVgtuJPqb5RP+xn9U4cECgU1hwQPB4cEBAWfrmzZ44rUmSwoAEGrjaEqc0N4/Ysh3a9s3CeKjqQrZyr1QlW7v0B6mjy+32J6NDoOXrdEYEPuM1RKC4fsskzhfDhlALaVdEkQptq8osvowZ+PafFZvxI1hwQnNQgDDC+DTYPyCAQEBAAFBj8ENrYC1Aj2M14ftCEp17AQvObcIbzyoto9SjZcrR/ROMLETjZsG3PF0Q9cFKLjkbJocVzKRYyUAWJv3kXNiPv5wLlqyAcGBAUE");
        private static int[] order = new int[] { 12,5,9,10,10,5,9,13,13,13,12,13,13,13,14 };
        private static int key = 5;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif

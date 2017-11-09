namespace Stardust.Sparrow.Player
{
    public static class SDEConstants
    {
        private const string EMITTER_NAME_PREFIX = "stardustEmitter_";
        public const string ATLAS_IMAGE_NAME = "atlas_0.png";
        public const string ATLAS_XML_NAME = "atlas_0.xml";

        public static string GetXmlName(string id)
        {
            return EMITTER_NAME_PREFIX + id + ".xml";
        }

        public static string GetParticleSnapshotName(string id)
        {
            return "emitterSnapshot_" + id + ".bytearray";
        }

        public static bool IsEmitterXmlName(string filename)
        {
            return filename.StartsWith(EMITTER_NAME_PREFIX);
        }

        public static string GetEmitterId(string xmlFilename)
        {
            return xmlFilename.Substring(16).Split('.')[0];
        }

        /// <summary>
        /// Returns the prefix for all textures used by emitterId in the atlas.
        /// </summary>
        public static string GetSubtexturePrefix(string emitterId)
        {
            return "emitter_" + emitterId + "_image_";
        }

        /// <summary>
        /// Returns names for subTextures .sde files.
        /// </summary>
        public static string GetSubTextureName(string emitterId, int imageNumber, int numberOfImagesInAtlas)
        {
            return GetSubtexturePrefix(emitterId) + IntToSortableStr(imageNumber, numberOfImagesInAtlas);
        }
        
        /// <summary>
        /// Convert an integer to a string that can be sorted with Array.CASEINSENSITIVE
        /// </summary>
        private static string IntToSortableStr(int val, int maxValue)
        {
            // Get the number of digits in the string and the value of the most-significant digit
            int digitValue = 1;
            int digits = 0;
            while (maxValue > 0)
            {
                digits++;
                digitValue *= 10;
                maxValue /= 10;
            }
            digitValue /= 10;

            // Build the string from most-significant to least-significant digit
            string ret = "";
            for (int i = 0; i < digits; ++i)
            {
                int digit = val / digitValue;
                ret += digit;
                val -= digit * digitValue;
                digitValue /= 10;
            }
            return ret;
        }
        
    }
}
using UnityEngine;

namespace TileMatch.Scripts.Managers
{
    public class AdaptiveScaleManager : MonoBehaviour
    {
        private float ScaleFactor { get; set; } = 1F;
        private float AspectRatio { get; set; } = 1F;
        private static float ReferenceRatio => 1.77F;
        private static float BaseThreshold => .65F;

        private void Awake()
        {
            // Get the current screen resolution
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            // Calculate the current aspect ratio
            AspectRatio = screenWidth / screenHeight;
            ScaleFactor = Mathf.Clamp(AspectRatio * ReferenceRatio, AspectRatio, ReferenceRatio);
            transform.localScale = Vector3.one * ScaleFactor;
        }

        public float ThresholdByScaleFactor()
        {
            return ScaleFactor * BaseThreshold;
        }
    }
}
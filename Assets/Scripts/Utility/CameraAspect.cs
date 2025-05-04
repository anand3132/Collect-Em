using RedGaintGames.CollectEM.Core.Extensions;
using UnityEngine;

namespace RedGaintGames.CollectEM.Core
{
    /// <summary>
    /// Adjusts the attached Camera to ensure a minimum scene width is always visible,
    /// regardless of the screen aspect ratio. Adds letterboxing if needed.
    /// </summary>
    [RequireComponent(typeof(Camera)), ExecuteInEditMode]
    public class CameraAspect : MonoBehaviour
    {
        [Header("Minimum Camera Width")]
        [Tooltip("Minimum scene width to be visible in world units.")]
        [SerializeField] 
        private float minimumVisibleWidth = 6.75f;

        /// <summary>
        /// Reference to the Camera component. Lazily initialized in ApplyAspectRatio.
        /// </summary>
        private Camera CachedCamera { get; set; }

        /// <summary>
        /// Stores the previous screen dimensions to detect resolution changes.
        /// </summary>
        private Vector2 lastScreenResolution = Vector2.zero;

        private void Start()
        {
            UpdateCameraAspect();
        }

        private void Update()
        {
            // Only update if resolution has changed
            if (Screen.width != lastScreenResolution.x || Screen.height != lastScreenResolution.y)
            {
                UpdateCameraAspect();
            }
        }

        /// <summary>
        /// Updates camera rect and aspect ratio based on the current screen size.
        /// </summary>
        private void UpdateCameraAspect()
        {
            lastScreenResolution = new Vector2(Screen.width, Screen.height);
            ApplyAspectRatio();
        }

        /// <summary>
        /// Ensures the camera shows at least the minimum scene width.
        /// Applies letterboxing if needed.
        /// </summary>
        private void ApplyAspectRatio()
        {
            // Ensure Camera reference is cached
            CachedCamera = GetComponent<Camera>();
            CachedCamera.rect = new Rect(0, 0, 1, 1);
            CachedCamera.ResetAspect();

            float currentCameraWidth = CachedCamera.GetWidth();

            // If the visible width is smaller than required, add vertical letterbox
            if (currentCameraWidth < minimumVisibleWidth)
            {
                float visibleRatio = currentCameraWidth / minimumVisibleWidth;
                ApplyLetterbox(visibleRatio);
            }
        }

        /// <summary>
        /// Adds a vertical letterbox to preserve horizontal field of view.
        /// </summary>
        /// <param name="heightRatio">Ratio of visible height to target height</param>
        private void ApplyLetterbox(float heightRatio)
        {
            Rect rect = new Rect(0, 0, 1, 1);

            // Reduce the height to match the target aspect ratio
            rect.height *= heightRatio;
            rect.y = (1f - rect.height) / 2f;

            // Round values to avoid rendering issues
            rect.y = (float)System.Math.Round(rect.y, 5);
            rect.height = (float)System.Math.Round(rect.height, 5);

            CachedCamera.rect = rect;
        }
    }
}

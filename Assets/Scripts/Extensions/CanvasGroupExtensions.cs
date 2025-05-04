namespace RedGaintGames.CollectEM.Core.Extensions
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Extension methods for fading CanvasGroup UI elements in and out.
    /// </summary>
    public static class CanvasGroupExtensions
    {
        /// <summary>
        /// Fades in the CanvasGroup's alpha over time (0 → 1).
        /// Also enables interaction after fade-in completes.
        /// </summary>
        /// <param name="canvasGroup">CanvasGroup to fade in</param>
        /// <param name="duration">Fade duration in seconds</param>
        public static IEnumerator FadeInCoroutine(this CanvasGroup canvasGroup, float duration)
        {
            float progress = 0f;
            float elapsed = 0f;

            // Disable interactions during fade
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            while (progress < 1f)
            {
                elapsed += Time.unscaledDeltaTime;

                progress = duration <= 0f 
                    ? 1f 
                    : Mathf.Clamp01(elapsed / duration);

                canvasGroup.alpha = Mathf.Lerp(0f, 1f, progress);
                yield return null;
            }

            // Enable interaction after fade-in completes
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        /// <summary>
        /// Fades out the CanvasGroup's alpha over time (1 → 0).
        /// Leaves interaction disabled after fading out.
        /// </summary>
        /// <param name="canvasGroup">CanvasGroup to fade out</param>
        /// <param name="duration">Fade duration in seconds</param>
        public static IEnumerator FadeOutCoroutine(this CanvasGroup canvasGroup, float duration)
        {
            float progress = 0f;
            float elapsed = 0f;

            // Disable interactions during fade
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            while (progress < 1f)
            {
                elapsed += Time.unscaledDeltaTime;

                progress = duration <= 0f 
                    ? 1f 
                    : Mathf.Clamp01(elapsed / duration);

                canvasGroup.alpha = Mathf.Lerp(1f, 0f, progress);
                yield return null;
            }
        }
    }
}

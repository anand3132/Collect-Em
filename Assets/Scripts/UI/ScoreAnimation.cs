using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

namespace RedGaintGames.CollectEM.Game
{
    public class ScoreAnimation : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject scorePopupPrefab;
        
        [Header("Animation Settings")]
        [SerializeField] private float animationDuration = 1f;
        [SerializeField] private float maxOffset = 0.5f;
        [SerializeField] private AnimationCurve movementCurve;
        [SerializeField] private AnimationCurve scaleCurve;
        [SerializeField] private float startScale = 1.5f;
        [SerializeField] private float endScale = 0.7f;
        [SerializeField] private Color[] scoreColorsBySize;

        [Header("References")]
        [SerializeField] private RectTransform scoreCounterRect;
        [SerializeField] private Camera gameCamera;

        private void Awake()
        {
            if (!gameCamera) gameCamera = Camera.main;
            GameEvents.OnScoreAnimationRequested.AddListener(OnScoreAnimationRequested);
        }

        private void OnDestroy()
        {
            GameEvents.OnScoreAnimationRequested.RemoveListener(OnScoreAnimationRequested);
        }

        private void OnScoreAnimationRequested(int score, List<Vector3> worldPositions)
        {
            if (worldPositions.Count == 0 || score <= 0) return;

            // For small matches, show one popup at center
            if (worldPositions.Count <= 4)
            {
                Vector3 centerPos = CalculateCenter(worldPositions);
                CreatePopup(score, centerPos);
            }
            else // For large matches, show multiple popups
            {
                int popupCount = Mathf.Min(3, worldPositions.Count);
                int scorePerPopup = score / popupCount;
                int remainder = score % popupCount;

                for (int i = 0; i < popupCount; i++)
                {
                    // Distribute remainder
                    int thisPopupScore = scorePerPopup + (i == 0 ? remainder : 0);
                    Vector3 randomPos = worldPositions[Random.Range(0, worldPositions.Count)];
                    CreatePopup(thisPopupScore, randomPos);
                }
            }
        }

        private Vector3 CalculateCenter(List<Vector3> positions)
        {
            Vector3 center = Vector3.zero;
            foreach (Vector3 pos in positions)
            {
                center += pos;
            }
            return center / positions.Count;
        }

        private void CreatePopup(int scoreValue, Vector3 worldPosition)
        {
            GameObject popup = Instantiate(scorePopupPrefab, transform);
            TextMeshProUGUI scoreText = popup.GetComponent<TextMeshProUGUI>();
            RectTransform rectTransform = popup.GetComponent<RectTransform>();

            // Set initial properties
            scoreText.text = $"+{scoreValue}";
            scoreText.color = GetScoreColor(scoreValue);
            rectTransform.position = gameCamera.WorldToScreenPoint(worldPosition);
            rectTransform.localScale = Vector3.one * startScale;

            // Add random offset for visual interest
            Vector2 randomOffset = Random.insideUnitCircle * maxOffset;
            rectTransform.anchoredPosition += randomOffset;

            StartCoroutine(AnimatePopup(popup, rectTransform));
        }

        private IEnumerator AnimatePopup(GameObject popup, RectTransform rectTransform)
        {
            Vector2 startPosition = rectTransform.position;
            Vector2 endPosition = scoreCounterRect.position;
            TextMeshProUGUI text = popup.GetComponent<TextMeshProUGUI>();
            Color startColor = text.color;
            Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);

            float elapsed = 0f;

            while (elapsed < animationDuration)
            {
                float t = elapsed / animationDuration;
                float curveT = movementCurve.Evaluate(t);

                // Position
                rectTransform.position = Vector2.Lerp(
                    startPosition, 
                    endPosition, 
                    curveT
                );

                // Scale
                float scaleT = scaleCurve.Evaluate(t);
                rectTransform.localScale = Vector3.one * Mathf.Lerp(startScale, endScale, scaleT);

                // Fade (last 30% of animation)
                if (t > 0.7f)
                {
                    float fadeT = (t - 0.7f) / 0.3f;
                    text.color = Color.Lerp(startColor, endColor, fadeT);
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            // Notify when score reaches counter (for counter effects)
            GameEvents.OnScoreReachedCounter.Invoke();
            Destroy(popup);
        }

        private Color GetScoreColor(int scoreValue)
        {
            if (scoreColorsBySize == null || scoreColorsBySize.Length == 0)
                return Color.white;

            int index = Mathf.Clamp(scoreValue / 3, 0, scoreColorsBySize.Length - 1);
            return scoreColorsBySize[index];
        }
    }
}
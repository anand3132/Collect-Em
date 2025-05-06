using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RedGaintGames.CollectEM.Game
{
    public class ScoreCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private float countDuration = 0.5f;
        [SerializeField] private float punchScale = 1.2f;
        [SerializeField] private float punchDuration = 0.3f;

        private Vector3 originalScale;

        private void Awake()
        {
            originalScale = text.transform.localScale;
            GameEvents.OnScoreChanged.AddListener(OnScoreChanged);
        }

        private void OnDestroy()
        {
            GameEvents.OnScoreChanged.RemoveListener(OnScoreChanged);
        }

        private void OnScoreChanged(int oldScore, int newScore)
        {
            StartCoroutine(AnimateScoreChange(oldScore, newScore));
        }

        private IEnumerator AnimateScoreChange(int from, int to)
        {
            // Wait for score popup to reach counter
            yield return new WaitForSeconds(0.8f);

            float elapsed = 0f;
            while (elapsed < countDuration)
            {
                float t = elapsed / countDuration;
                int currentValue = (int)Mathf.Lerp(from, to, t);
                text.text = currentValue.ToString();
                elapsed += Time.deltaTime;
                yield return null;
            }

            text.text = to.ToString();

            // Punch effect
            elapsed = 0f;
            while (elapsed < punchDuration)
            {
                float t = elapsed / punchDuration;
                float scale = Mathf.Lerp(punchScale, 1f, t);
                text.transform.localScale = originalScale * scale;
                elapsed += Time.deltaTime;
                yield return null;
            }

            text.transform.localScale = originalScale;
        }

        /// <summary>
        /// Smoothly updates the score text component over the given time
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator UpdateTextCoroutine(int from, int to, float time)
        {
            float currentTime = Time.timeSinceLevelLoad;
            float elapsedTime = 0.0f;
            float lastTime = currentTime;

            while (time > 0 && elapsedTime < time)
            {
                //Update Time
                currentTime = Time.timeSinceLevelLoad;
                elapsedTime += currentTime - lastTime;
                lastTime = currentTime;

                //Update text component with the interpolated value for the score
                float value = Mathf.Lerp(from, to, elapsedTime / time);
                this.text.text = ((int)value).ToString();

                yield return null;
            }

            this.text.text = to.ToString();
        }

    }
}
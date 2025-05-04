using UnityEngine;
using UnityEngine.Audio;

namespace RedGaintGames.CollectEM.Core
{
    /// <summary>
    /// Abstract base class for persistent audio managers (e.g., music, SFX).
    /// Handles volume control, saving/loading preferences, and mixer interaction.
    /// </summary>
    /// <typeparam name="T">Type of the concrete audio manager inheriting this class.</typeparam>
    public abstract class AudioManager<T> : Singleton<T> where T : AudioManager<T>
    {
        [SerializeField] private AudioMixer audioMixer = null;

        // Internal volume scale in [0, 1], mapped to dB range on the AudioMixer
        private float volumeScale = 0.0f;

        /// <summary>
        /// Volume scale in the normalized range [0, 1].
        /// Automatically updates the AudioMixer when changed.
        /// </summary>
        public float VolumeScale
        {
            get => volumeScale;
            set
            {
                volumeScale = Mathf.Clamp01(value);
                SetVolume(volumeScale);
            }
        }

        /// <summary>
        /// Unique PlayerPrefs key used to store volume for this audio manager type.
        /// Format: "[ManagerType].Volume"
        /// </summary>
        protected string PlayerPrefsVolumeKey => $"{GetType().Name}.Volume";

        /// <summary>
        /// Must be implemented by derived classes to set volume based on volumeScale.
        /// Typically applies the value to a specific AudioMixer parameter.
        /// </summary>
        /// <param name="volumeScale">Normalized volume [0, 1]</param>
        protected abstract void SetVolume(float volumeScale);

        /// <summary>
        /// Converts a normalized volume [0, 1] to dB and applies it to the given AudioMixer parameter.
        /// </summary>
        /// <param name="volumeParameterName">AudioMixer exposed parameter name</param>
        /// <param name="volumeScale">Normalized volume value</param>
        protected void SetVolume(string volumeParameterName, float volumeScale)
        {
            volumeScale = Mathf.Clamp01(volumeScale);

            // Convert normalized volume to linear scale [min, max]
            float min = 0.0001f; // Approximates -80dB (near silence)
            float max = 1.0f;    // 0dB (full volume)
            float linearValue = Mathf.Lerp(min, max, volumeScale);

            // Convert to decibels
            float dBValue = 20f * Mathf.Log10(linearValue);

            // Apply to AudioMixer
            audioMixer.SetFloat(volumeParameterName, dBValue);
        }

        /// <summary>
        /// Saves the current volume setting to PlayerPrefs for persistence.
        /// </summary>
        public virtual void SaveSettings()
        {
            PlayerPrefs.SetFloat(PlayerPrefsVolumeKey, VolumeScale);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Loads the saved volume setting from PlayerPrefs if available.
        /// Defaults to full volume (1.0f) if no saved value is found.
        /// </summary>
        protected virtual void LoadSettings()
        {
            VolumeScale = PlayerPrefs.HasKey(PlayerPrefsVolumeKey)
                ? PlayerPrefs.GetFloat(PlayerPrefsVolumeKey)
                : 1.0f;
        }
    }
}

using UnityEngine;
using RedGaintGames.CollectEM.Core;

namespace RedGaintGames.CollectEM.Game
{
    /// <summary>
    /// Singleton that manages and plays common game sound effects (SFX).
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class GameSFX : Singleton<GameSFX>
    {
        [Header("Sound Effects")]
        [SerializeField] private AudioClip selectionClip; // Played when a player makes a selection
        [SerializeField] private AudioClip despawnClip;   // Played when bubbles are removed
        [SerializeField] private AudioClip spawnClip;     // Played when bubbles are spawned

        private AudioSource audioSource;

        /// <summary>
        /// Exposes the selection sound effect.
        /// </summary>
        public AudioClip SelectionClip => selectionClip;

        /// <summary>
        /// Exposes the despawn sound effect.
        /// </summary>
        public AudioClip DespawnClip => despawnClip;

        /// <summary>
        /// Exposes the spawn sound effect.
        /// </summary>
        public AudioClip SpawnClip => spawnClip;

        /// <summary>
        /// Initializes the internal AudioSource reference.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Plays a sound effect using the global SFX manager.
        /// Use this for standard playback without pitch modification.
        /// </summary>
        /// <param name="clip">The AudioClip to play.</param>
        public void Play(AudioClip clip)
        {
            SFXManager.Instance.PlayOneShot(clip);
        }

        /// <summary>
        /// Plays a sound effect using the local AudioSource with a custom pitch.
        /// Use this when you need pitch variation (e.g., combo effects).
        /// </summary>
        /// <param name="clip">The AudioClip to play.</param>
        /// <param name="pitch">The pitch to apply when playing the clip.</param>
        public void Play(AudioClip clip, float pitch)
        {
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(clip);
        }
    }
}

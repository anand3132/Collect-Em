using RedGaintGames.CollectEM.Core.Extensions;
using UnityEngine;

namespace RedGaintGames.CollectEM.Game
{
    /// <summary>
    /// Represents a single tile or element in the grid.
    /// Handles visual behavior such as movement, spawn/despawn animations, and appearance.
    /// </summary>
    public class GridElement : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer = null; // Renders the visual sprite of the grid element
        private IGridElementSettings settings; // Settings for spawn/despawn animation durations and scale

        /// <summary>
        /// Whether the element is currently moving.
        /// </summary>
        public bool IsMoving { get; private set; }

        /// <summary>
        /// Whether the element is currently active (spawned) in the scene.
        /// </summary>
        public bool IsSpawned => gameObject.activeSelf;

        /// <summary>
        /// Color of the grid element, forwarded to its SpriteRenderer.
        /// </summary>
        public Color Color
        {
            get => spriteRenderer.color;
            set => spriteRenderer.color = value;
        }

        /// <summary>
        /// Initializes the grid element with provided settings or default values.
        /// </summary>
        /// <param name="settings">Optional animation settings. Uses default if null.</param>
        public void Initialize(IGridElementSettings settings = null)
        {
            this.settings = settings ?? new DefaultElementSettings();
        }

        /// <summary>
        /// Moves the element to a target position over time with a coroutine.
        /// </summary>
        /// <param name="targetPos">Destination world position.</param>
        /// <param name="time">Time to complete the movement.</param>
        public void Move(Vector3 targetPos, float time)
        {
            IsMoving = true;
            StartCoroutine(transform.MoveCoroutine(targetPos, time, () =>
            {
                IsMoving = false;
            }));
        }

        /// <summary>
        /// Activates and visually spawns the element using a scaling animation.
        /// </summary>
        public void Spawn()
        {
            gameObject.SetActive(true);
            Vector2 startScale = Vector2.one * settings.SpawnStartScale;
            Vector2 endScale = Vector2.one;
            StartCoroutine(transform.ScaleCoroutine(startScale, endScale, settings.SpawnAnimationDuration));
        }

        /// <summary>
        /// Plays a scaling animation to "shrink" the element and disables the game object.
        /// </summary>
        public void Despawn()
        {
            Vector2 startScale = Vector2.one;
            Vector2 endScale = Vector2.one * settings.DespawnEndScale;
            StartCoroutine(transform.ScaleCoroutine(startScale, endScale, settings.DespawnAnimationDuration, () =>
            {
                gameObject.SetActive(false);
            }));
        }

        /// <summary>
        /// Default implementation of IGridElementSettings for fallback animation values.
        /// </summary>
        private class DefaultElementSettings : IGridElementSettings
        {
            public float SpawnAnimationDuration => 0.25f;
            public float DespawnAnimationDuration => 0.25f;
            public float SpawnStartScale => 0.1f;
            public float DespawnEndScale => 0.1f;
        }
    }
}

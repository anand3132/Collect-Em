using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RedGaintGames.CollectEM.Game.Designer
{
    public class GridDesignerUI : MonoBehaviour, IGridMovementSettings, IGridSpawningSettings,IGridElementSettings
    {
        [Header("Movement Settings UI")]
        [SerializeField] private Slider checkIntervalSlider;
        [SerializeField] private Slider moveDurationSlider;
        [SerializeField] private Slider positionThresholdSlider;
        [SerializeField] private TMP_Text checkIntervalText;
        [SerializeField] private TMP_Text moveDurationText;
        [SerializeField] private TMP_Text positionThresholdText;

        [Header("Spawning Settings UI")]
        [SerializeField] private Slider respawnDelaySlider;
        [SerializeField] private Slider despawnDelaySlider;
        [SerializeField] private TMP_Text respawnDelayText;
        [SerializeField] private TMP_Text despawnDelayText;

        [Header("Default Movement Values")]
        [SerializeField] private float defaultCheckInterval = 0.05f;
        [SerializeField] private float defaultMoveDuration = 0.4f;
        [SerializeField] private float defaultPositionThreshold = 0.01f;

        [Header("Default Spawning Values")]
        [SerializeField] private float defaultRespawnDelay = 0.4f;
        [SerializeField] private float defaultDespawnDelay = 0.4f;

        [Header("Gravity Control")]
        [SerializeField] private Slider gravityMultiplierSlider;
        [SerializeField] private TMP_Text gravityMultiplierText;
        [SerializeField] private float defaultGravityMultiplier = 1f;

        // Add to interface implementation
        public float GravityMultiplier { get; private set; }
        // Movement Settings Implementation
        public float MovementCheckInterval { get; private set; }
        public float ElementMoveDuration { get; private set; }
        public float PositionUpdateThreshold { get; private set; }

        // Spawning Settings Implementation
        public float RespawnDelay { get; private set; }
        public float DespawnDelay { get; private set; }
        
        [Header("Element Animation Settings")]
        [SerializeField] private Slider spawnDurationSlider;
        [SerializeField] private Slider despawnDurationSlider;
        [SerializeField] private Slider spawnStartScaleSlider;
        [SerializeField] private Slider despawnEndScaleSlider;
        [SerializeField] private TMP_Text spawnDurationText;
        [SerializeField] private TMP_Text despawnDurationText;
        [SerializeField] private TMP_Text spawnStartScaleText;
        [SerializeField] private TMP_Text despawnEndScaleText;

        [Header("Default Element Values")]
        [SerializeField] private float defaultSpawnDuration = 0.25f;
        [SerializeField] private float defaultDespawnDuration = 0.25f;
        [SerializeField] [Range(0.01f, 0.5f)] private float defaultSpawnStartScale = 0.1f;
        [SerializeField] [Range(0.01f, 0.5f)] private float defaultDespawnEndScale = 0.1f;

        // IGridElementSettings implementation
        public float SpawnAnimationDuration { get; private set; }
        public float DespawnAnimationDuration { get; private set; }
        public float SpawnStartScale { get; private set; }
        public float DespawnEndScale { get; private set; }
   
        private void Awake()
        {
            InitializeMovementSettings();
            InitializeSpawningSettings();
            InitializeGravityControl();
            InitializeElementSettings();
            
        }
      

        private void UpdateElementText()
        {
            spawnDurationText.text = $"Spawn Duration: {defaultSpawnDuration:F2}s";
            despawnDurationText.text = $"Despawn Duration: {defaultDespawnDuration:F2}s";
            spawnStartScaleText.text = $"Spawn Start Scale: {defaultSpawnStartScale:F2}";
            despawnEndScaleText.text = $"Despawn End Scale: {defaultDespawnEndScale:F2}";
        }
        
        private void InitializeElementSettings()
        {
            SpawnAnimationDuration = defaultSpawnDuration;
            DespawnAnimationDuration = defaultDespawnDuration;
            SpawnStartScale = defaultSpawnStartScale;
            DespawnEndScale = defaultDespawnEndScale;

            spawnDurationSlider.value = defaultSpawnDuration;
            despawnDurationSlider.value = defaultDespawnDuration;
            spawnStartScaleSlider.value = defaultSpawnStartScale;
            despawnEndScaleSlider.value = defaultDespawnEndScale;

            spawnDurationSlider.onValueChanged.AddListener(value => {
                SpawnAnimationDuration = value;
                spawnDurationText.text = $"Spawn Duration: {value:F2}s";
            });

            despawnDurationSlider.onValueChanged.AddListener(value => {
                DespawnAnimationDuration = value;
                despawnDurationText.text = $"Despawn Duration: {value:F2}s";
            });

            spawnStartScaleSlider.onValueChanged.AddListener(value => {
                SpawnStartScale = value;
                spawnStartScaleText.text = $"Spawn Start Scale: {value:F2}";
            });

            despawnEndScaleSlider.onValueChanged.AddListener(value => {
                DespawnEndScale = value;
                despawnEndScaleText.text = $"Despawn End Scale: {value:F2}";
            });

            UpdateElementText();
        }

        private void InitializeMovementSettings()
        {
            MovementCheckInterval = defaultCheckInterval;
            ElementMoveDuration = defaultMoveDuration;
            PositionUpdateThreshold = defaultPositionThreshold;

            checkIntervalSlider.value = defaultCheckInterval;
            moveDurationSlider.value = defaultMoveDuration;
            positionThresholdSlider.value = defaultPositionThreshold;

            checkIntervalSlider.onValueChanged.AddListener(value => {
                MovementCheckInterval = value;
                checkIntervalText.text = $"Check Interval: {value:F3}s";
            });

            moveDurationSlider.onValueChanged.AddListener(value => {
                ElementMoveDuration = value;
                moveDurationText.text = $"Move Duration: {value:F3}s";
            });

            positionThresholdSlider.onValueChanged.AddListener(value => {
                PositionUpdateThreshold = value;
                positionThresholdText.text = $"Position Threshold: {value:F3}";
            });

            UpdateMovementText();
        }

        
        private void InitializeGravityControl()
        {
            GravityMultiplier = defaultGravityMultiplier;
            gravityMultiplierSlider.value = defaultGravityMultiplier;
            
            gravityMultiplierSlider.onValueChanged.AddListener(value => {
                GravityMultiplier = Mathf.Clamp(value, 0.1f, 5f); // Limit between 0.1x and 5x speed
                gravityMultiplierText.text = $"Gravity: {value:F1}x";
            });

            gravityMultiplierText.text = $"Gravity: {defaultGravityMultiplier:F1}x";
        }
        private void InitializeSpawningSettings()
        {
            RespawnDelay = defaultRespawnDelay;
            DespawnDelay = defaultDespawnDelay;

            respawnDelaySlider.value = defaultRespawnDelay;
            despawnDelaySlider.value = defaultDespawnDelay;

            respawnDelaySlider.onValueChanged.AddListener(value => {
                RespawnDelay = value;
                respawnDelayText.text = $"Respawn Delay: {value:F3}s";
            });

            despawnDelaySlider.onValueChanged.AddListener(value => {
                DespawnDelay = value;
                despawnDelayText.text = $"Despawn Delay: {value:F3}s";
            });

            UpdateSpawningText();
        }

        private void UpdateMovementText()
        {
            checkIntervalText.text = $"Check Interval: {defaultCheckInterval:F3}s";
            moveDurationText.text = $"Move Duration: {defaultMoveDuration:F3}s";
            positionThresholdText.text = $"Position Threshold: {defaultPositionThreshold:F3}";
        }

        private void UpdateSpawningText()
        {
            respawnDelayText.text = $"Respawn Delay: {defaultRespawnDelay:F3}s";
            despawnDelayText.text = $"Despawn Delay: {defaultDespawnDelay:F3}s";
        }

        private void OnDestroy()
        {
            checkIntervalSlider.onValueChanged.RemoveAllListeners();
            moveDurationSlider.onValueChanged.RemoveAllListeners();
            positionThresholdSlider.onValueChanged.RemoveAllListeners();
            respawnDelaySlider.onValueChanged.RemoveAllListeners();
            despawnDelaySlider.onValueChanged.RemoveAllListeners();
        }
    }
}
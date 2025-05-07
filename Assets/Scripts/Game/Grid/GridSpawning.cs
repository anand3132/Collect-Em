using RedGaintGames.CollectEM.Core.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedGaintGames.CollectEM.Game
{
    public class GridSpawning
    {
        private readonly Grid grid;
        private readonly IGridSpawningSettings settings;

        public GridSpawning(Grid grid, IGridSpawningSettings settings = null)
        {
            this.grid = grid;
            this.settings = settings ?? new DefaultSpawningSettings();
            
            #if UNITY_EDITOR
            if(settings == null)
                Debug.Log("Using default spawning settings");
            #endif
        }

        /// <summary>
        /// Respawns all despawned elements with animation
        /// </summary>
        public IEnumerator RespawnElements()
        {
           // GameSFX.Instance.Play(GameSFX.Instance.SpawnClip);
            bool anyRespawned = false;

            foreach (GridElement element in grid.Elements)
            {
                if (!element.IsSpawned)
                {
                    element.Color = grid.Colors.GetRandom();
                    element.Spawn();
                    anyRespawned = true;
                }
            }

            if (anyRespawned)
            {
                yield return new WaitForSeconds(settings.RespawnDelay);
            }
        }

        /// <summary>
        /// Despawns elements with animation and triggers score effects
        /// </summary>
        public IEnumerator Despawn(List<GridElement> elements)
        {
            if (elements == null || elements.Count == 0)
                yield break;

            GameSFX.Instance.Play(GameSFX.Instance.DespawnClip);
            int validElements = 0;
            List<Vector3> matchedPositions = new List<Vector3>();

            // First pass: Validate elements and collect positions
            foreach (GridElement element in elements)
            {
                if (element != null && element.IsSpawned)
                {
                    matchedPositions.Add(element.transform.position);
                    validElements++;
                }
            }

            if (validElements > 0)
            {
                // Trigger score animation before despawning
                int scoreValue = CalculateScore(validElements);
                GameEvents.OnScoreAnimationRequested.Invoke(scoreValue, matchedPositions);

                // Second pass: Actually despawn the elements
                foreach (GridElement element in elements)
                {
                    if (element != null && element.IsSpawned)
                    {
                        element.Despawn();
                    }
                }

                yield return new WaitForSeconds(settings.DespawnDelay);

                // Update game state
                GameManager.Instance.MovesAvailable--;
                GameEvents.OnElementsDeSpawned.Invoke(validElements);
            }
        }

        /// <summary>
        /// Calculates score based on match size
        /// </summary>
        private int CalculateScore(int matchedCount)
        {
            // Example scoring formula: n*(n-1) where n = number of matched elements
            return matchedCount * (matchedCount - 1);
        }

        /// <summary>
        /// Default spawning settings
        /// </summary>
        private class DefaultSpawningSettings : IGridSpawningSettings
        {
            public float RespawnDelay => 0.4f;
            public float DespawnDelay => 0.4f;
        }
    }
}
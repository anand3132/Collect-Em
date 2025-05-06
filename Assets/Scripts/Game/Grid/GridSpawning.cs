using RedGaintGames.CollectEM.Core.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
            if(settings==null)
                Debug.Log("Default DefaultSpawningSettings  settings");
        }

        public IEnumerator RespawnElements()
        {
            GameSFX.Instance.Play(GameSFX.Instance.SpawnClip);
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

        public IEnumerator Despawn(List<GridElement> elements)
        {
            if (elements == null || elements.Count == 0)
                yield break;

            GameSFX.Instance.Play(GameSFX.Instance.DespawnClip);
            int validElements = 0;

            foreach (GridElement element in elements)
            {
                if (element != null && element.IsSpawned)
                {
                    element.Despawn();
                    validElements++;
                }
            }

            if (validElements > 0)
            {
                yield return new WaitForSeconds(settings.DespawnDelay);
                GameManager.Instance.MovesAvailable--;
                GameEvents.OnElementsDeSpawned.Invoke(validElements);
            }
        }

        // Default implementation
        private class DefaultSpawningSettings : IGridSpawningSettings
        {
            public float RespawnDelay => 0.4f;
            public float DespawnDelay => 0.4f;
        }
    }
}
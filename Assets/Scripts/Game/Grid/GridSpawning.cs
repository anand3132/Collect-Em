namespace RedGaintGames.CollectEM.Game
{
    using CollectEM.Core.Extensions;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Despawns and respawns elements in the game grid
    /// </summary>
    public class GridSpawning
    {
        private Grid grid;

        public GridSpawning(Grid grid)
        {
            this.grid = grid;
        }

        /// <summary>
        /// Respawns all despawned elements
        /// </summary>
        /// <returns></returns>
        public IEnumerator RespawnElements()
        {
            GameSFX.Instance.Play(GameSFX.Instance.SpawnClip);

            foreach (GridElement element in this.grid.Elements)
            {
                if (element.IsSpawned == false)
                {
                    element.Color = this.grid.Colors.GetRandom();

                    element.Spawn();
                }
            }

            yield return new WaitForSeconds(0.4f);
        }

        /// <summary>
        /// Despawns the given list of elements
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public IEnumerator Despawn(List<GridElement> elements)
        {
            GameSFX.Instance.Play(GameSFX.Instance.DespawnClip);

            foreach (GridElement gridElement in elements)
            {
                gridElement.Despawn();
            }

            yield return new WaitForSeconds(0.4f);

            GameManager.Instance.MovesAvailable--;

            GameEvents.OnElementsDeSpawned.Invoke(elements.Count);
        }
    }
}
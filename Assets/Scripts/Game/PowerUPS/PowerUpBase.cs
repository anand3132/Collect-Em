using System.Collections;
using UnityEngine;
using System.Collections.Generic;
namespace RedGaintGames.CollectEM.Game.PowerUps
{
    public abstract class PowerUpBase : IPowerUp
    {
        protected Grid _grid;
        protected Vector2Int _origin;

        protected PowerUpBase(Grid grid, Vector2Int origin)
        {
            _grid = grid;
            _origin = origin;
        }

        public abstract IEnumerator Execute();

        protected IEnumerator DespawnElements(List<GridElement> elements)
        {
            yield return _grid.Despawn(elements);
        }
    }
}
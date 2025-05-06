namespace RedGaintGames.CollectEM.Game
{
    using UnityEngine;

    public interface IGridPositionProvider
    {
        Vector2Int GetGridPosition(GridElement element);
    }
}
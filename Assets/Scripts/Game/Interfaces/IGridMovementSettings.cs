// IGridMovementSettings.cs
namespace RedGaintGames.CollectEM.Game
{
    public interface IGridMovementSettings
    {
        float MovementCheckInterval { get; }
        float ElementMoveDuration { get; }
        float PositionUpdateThreshold { get; }
        float GravityMultiplier { get; } // New property
    }
}
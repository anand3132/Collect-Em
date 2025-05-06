// IGridElementSettings.cs
namespace RedGaintGames.CollectEM.Game
{
    public interface IGridElementSettings
    {
        float SpawnAnimationDuration { get; }
        float DespawnAnimationDuration { get; }
        float SpawnStartScale { get; }
        float DespawnEndScale { get; }
    }
}
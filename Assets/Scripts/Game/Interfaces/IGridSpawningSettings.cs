// IGridSpawningSettings.cs
namespace RedGaintGames.CollectEM.Game
{
    public interface IGridSpawningSettings
    {
        float RespawnDelay { get; }
        float DespawnDelay { get; }
    }
}
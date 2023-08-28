using Feudal.MessageBuses;

namespace Feudal.Interfaces
{
    public enum Terrain
    {
        Plain,
        Hill
    }

    public interface ITerrainItem
    {
        (int x, int y) Position { get; }
        Terrain Terrain { get; }
    }

    public class Message_AddTerrainItem : Message
    {
        public readonly (int x, int y) position;
        public readonly Terrain terrainType;

        public Message_AddTerrainItem((int x, int y) position, Terrain terrainType)
        {
            this.position = position;
            this.terrainType = terrainType;
        }
    }
}
using Feudal.MessageBuses;
using System;

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

    public class Message_AddTask : Message
    {
        public readonly Type taskType;
        public object[] parameters;

        public Message_AddTask(Type taskType, object[] parameters)
        {
            this.taskType = taskType;
            this.parameters = parameters;
        }
    }

    public class Message_NextTurn : Message
    {
        public int a;
    }
}
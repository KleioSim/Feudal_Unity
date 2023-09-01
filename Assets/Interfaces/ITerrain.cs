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
        bool IsDiscovered { get; }
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

    public class Message_TerrainItemDiscoverChanged : Message
    {
        public readonly (int x, int y) position;
        public readonly bool discoverd;

        public Message_TerrainItemDiscoverChanged((int x, int y) position, bool discoverd)
        {
            this.position = position;
            this.discoverd = discoverd;
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

    }

    public class Message_CancelTask : Message
    {
        public readonly string taskId;

        public Message_CancelTask(string taskId)
        {
            this.taskId = taskId;
        }
    }

    public interface ITask
    {
        public (int x, int y) Position { get; }
        public string Id { get; }
        public string Desc { get; }
        public int Percent { get; }
    }
}
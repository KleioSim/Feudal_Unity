using Feudal.MessageBuses;
using System;
using System.Collections.Generic;

namespace Feudal.Interfaces
{
    public enum Terrain
    {
        Plain,
        Hill,
        Mountion,
        Lake,
        Marsh
    }

    public enum TerrainTrait
    {
        [VaildEstate(EstateType.Farm)]
        FatSoil,

        [VaildEstate(EstateType.CopperMine)]
        CopperLode
    }

    public class VaildEstateAttribute : Attribute
    {
        public readonly EstateType estateType;

        public VaildEstateAttribute(EstateType estateType)
        {
            this.estateType = estateType;
        }
    }

    public interface ITerrainItem
    {
        (int x, int y) Position { get; }
        Terrain Terrain { get; }
        bool IsDiscovered { get; }

        IEnumerable<TerrainTrait> Traits { get; }
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

    public class Message_AddEstate : Message
    {
        public readonly (int x, int y) position;
        public readonly EstateType estateType;

        public Message_AddEstate((int x, int y) position, EstateType estateType)
        {
            this.position = position;
            this.estateType = estateType;
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
        public readonly string clanId;

        public object[] parameters;

        public Message_AddTask(Type taskType, string clanId, object[] parameters)
        {
            this.taskType = taskType;
            this.clanId = clanId;

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

    public class Message_QueryTasksInClan : Message
    {
        public readonly string clanId;

        public Message_QueryTasksInClan(string clanId)
        {
            this.clanId = clanId;
        }
    }

    public interface ITask
    {
        public (int x, int y) Position { get; }
        public string Id { get; }
        public string Desc { get; }
        public int Percent { get; }

        public string ClanId { get; }
    }
}
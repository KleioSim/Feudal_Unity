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
        public readonly string ownerId;

        public Message_AddEstate((int x, int y) position, EstateType estateType, string ownerId)
        {
            this.position = position;
            this.estateType = estateType;
            this.ownerId = ownerId;
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

    public class Message_SetEstateOwner : Message
    {
        public readonly string estateId;
        public readonly string clanId;

        public Message_SetEstateOwner(string estateId, string clanId)
        {
            this.estateId = estateId;
            this.clanId = clanId;
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
    
    public class Message_QueryEstatesByOwner : Message
    {
        public readonly string clanId;

        public Message_QueryEstatesByOwner(string clanId)
        {
            this.clanId = clanId;
        }
    }

    public class Message_EstateStartProducting : Message
    {
        public readonly ProductType productType;
        public readonly decimal productValue;
        public readonly string ownerId;
        public readonly string estateId;

        public Message_EstateStartProducting(ProductType productType, decimal productValue, string ownerId, string estateId)
        {
            this.productType = productType;
            this.productValue = productValue;
            this.ownerId = ownerId;
            this.estateId = estateId;
        }
    }

    public class Message_EstateStopProducting : Message
    {
        public readonly string ownerId;
        public readonly string estateId;

        public Message_EstateStopProducting(string ownerId, string estateId)
        {
            this.ownerId = ownerId;
            this.estateId = estateId;
        }
    }

    public class Message_FindEstateById : Message
    {
        public readonly string estateId;

        public Message_FindEstateById(string estateId)
        {
            this.estateId = estateId;
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
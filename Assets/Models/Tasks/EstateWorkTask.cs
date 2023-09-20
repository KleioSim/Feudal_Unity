using Feudal.Interfaces;
using Feudal.MessageBuses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feudal.Tasks
{
    public class EstateWorkTask : Task
    {
        public static Func<string, IEstate> funcGetEstate;

        public readonly string estateId;

        public ProductType ProductType { get; }
        public decimal ProductValue { get; }
        public string OwnerId { get; }

        public EstateWorkTask(string clanId, object[] parameters) : base(clanId)
        {
            IsContinuous = true;

            Position = (((int x, int y))parameters[0]);

            estateId = ((string)parameters[1]);

            var estate = funcGetEstate(estateId);

            ProductType = estate.ProductType;
            ProductValue = estate.ProductValue;
            OwnerId = estate.OwnerId;
        }

        internal override IEnumerable<Message> OnStart()
        {
            return new Message[]
            {
                new Message_EstateStartProducting(ProductType, ProductValue, OwnerId, estateId)
            };
        }

        internal override IEnumerable<Message> OnCancel()
        {
            return new Message[]
            {
                new Message_EstateStopProducting(OwnerId, estateId)
            };
        }

        internal override IEnumerable<Message> OnFinished()
        {
            throw new Exception();
        }
    }
}
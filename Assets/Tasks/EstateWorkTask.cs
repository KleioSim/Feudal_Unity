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

        private readonly string estateId;

        public EstateWorkTask(string clanId, object[] parameters) : base(clanId)
        {
            Position = (((int x, int y))parameters[0]);

            estateId = ((string)parameters[1]);

            IsContinuous = true;
        }

        internal override IEnumerable<Message> OnFinished()
        {
            var estate = funcGetEstate(estateId);

            return new Message[] { new Message_Producting(estate.ProductType, estate.ProductValue, estate.OwnerId, estate.Id) };
        }
    }
}
using Feudal.Interfaces;
using Feudal.MessageBuses;
using System.Collections.Generic;

namespace Feudal.Tasks
{
    public class EstateBuildTask : Task
    {
        public readonly EstateType estateType;
        public readonly string ownerId;

        public EstateBuildTask(string clanId, object[] parameters) : base(clanId)
        {
            Position = (((int x, int y))parameters[0]);
            estateType = ((EstateType)parameters[1]);
            ownerId = ((string)parameters[2]);
        }

        internal override IEnumerable<Message> OnFinished()
        {
            return new Message[] { new Message_AddEstate(Position, estateType, ownerId) };
        }
    }
}
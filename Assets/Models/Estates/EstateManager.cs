using Feudal.Interfaces;
using Feudal.MessageBuses;
using System.Collections.Generic;
using System.Linq;

namespace Feudal.Estates
{
    public partial class EstateManager 
    {
        private Dictionary<(int x, int y), Estate> estates = new Dictionary<(int x, int y), Estate>();
        private IMessageBus messageBus;

        public EstateManager(IMessageBus messageBus)
        {
            this.messageBus = messageBus;
            messageBus.Register(this);

            var estate = new Estate((0, 0), EstateType.Farm);
            estates.Add(estate.Position, estate);
        }

        [MessageProcess]
        public void OnMessage_AddEstate(Message_AddEstate message)
        {
            var estate = new Estate(message.position, message.estateType);
            estate.OwnerId = message.ownerId;

            estates.Add(estate.Position, estate);
        }

        [MessageProcess]
        public IEstate[] OnMessage_QueryEstatesByOwner(Message_QueryEstatesByOwner message)
        {
            return estates.Values.Where(x => x.OwnerId == message.clanId).ToArray();
        }

        [MessageProcess]
        public void OnMessage_SetEstateOwner(Message_SetEstateOwner message)
        {
            var estate = estates.Values.Single(x => x.Id == message.estateId);
            estate.OwnerId = message.clanId;
        }

        [MessageProcess]
        public IEstate OnMessage_FindEstateById(Message_FindEstateById message)
        {
            return estates.Values.Single(x => x.Id == message.estateId); 
        }
    }
}
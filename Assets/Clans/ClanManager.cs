using Feudal.Interfaces;
using Feudal.MessageBuses;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Feudal.Clans
{
    public class ClanManager : IEnumerable<Clan>
    {
        private List<Clan> list = new List<Clan>();

        private IMessageBus messageBus;

        public IEnumerator<Clan> GetEnumerator()
        {
            return ((IEnumerable<Clan>)list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)list).GetEnumerator();
        }

        public Clan Player { get; private set; }

        public ClanManager(IMessageBus messageBus)
        {
            this.messageBus = messageBus;
            messageBus.Register(this);

            Clan.funcQueryTasks = (clanId) =>
            {
                return messageBus.PostMessage(new Message_QueryTasksInClan(clanId))
                    .WaitAck<ITask[]>();
            };
            Clan.funcQueryEstates = (clanId) =>
            {
                return messageBus.PostMessage(new Message_QueryEstatesByOwner(clanId))
                    .WaitAck<IEstate[]>();
            };

            Player = new Clan();

            list.Add(Player);
            list.Add(new Clan());
            list.Add(new Clan());
        }

        [MessageProcess]
        void OnMessage_Producting(Message_Producting message)
        {
            var clan = list.SingleOrDefault(x => x.Id == message.ownerId);

            ((ProductData)clan.ProductMgr[message.productType]).Current += message.productValue;
        }
    }
}
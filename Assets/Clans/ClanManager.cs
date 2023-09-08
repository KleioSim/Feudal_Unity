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
        void OnMessage_EstateStartProducting(Message_EstateStartProducting message)
        {
            var clan = list.SingleOrDefault(x => x.Id == message.ownerId);

            clan.productMgr[message.productType].estateWorkOuputs.Add(message.estateId, message.productValue);
        }

        [MessageProcess]
        void OnMessage_EstateStopProducting(Message_EstateStopProducting message)
        {
            var clan = list.SingleOrDefault(x => x.Id == message.ownerId);
            foreach(var estateWorkOuput in clan.productMgr.Values.Select(x=>x.estateWorkOuputs))
            {
                estateWorkOuput.Remove(message.estateId);
            }
        }

        [MessageProcess]
        void OnMessage_NextTurn(Message_NextTurn message)
        {
            foreach(var productData in list.SelectMany(clan => clan.productMgr.Values))
            {
                productData.Settle();
            }
        }
    }
}
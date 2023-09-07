using Feudal.Interfaces;
using Feudal.MessageBuses;
using System;
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
    }

    public class Clan : IClan
    {
        internal static Func<string, ITask[]> funcQueryTasks;
        internal static Func<string, IEstate[]> funcQueryEstates;

        private static int clanId;

        public string Id { get; }

        public string Name { get; }

        public int TotalLaborCount { get; }

        public ITask[] tasks
        {
            get
            {
                return funcQueryTasks(Id);
            }
        }

        public IEstate[] estates
        {
            get
            {
                return funcQueryEstates(Id);
            }
        }

        public int PopCount { get; set; }

        private Dictionary<ProductType, IProductData> productMgr;
        public IReadOnlyDictionary<ProductType, IProductData> ProductMgr => productMgr;

        public Clan()
        {
            Id = clanId++.ToString();
            Name = Id;

            TotalLaborCount = 3;

            PopCount = 0;

            productMgr = Enum.GetValues(typeof(ProductType)).OfType<ProductType>().ToDictionary(k => k, v=> new ProductData() as IProductData);

            ((ProductData)productMgr[ProductType.Food]).Current = 100;
        }
    }

    public class ProductData : IProductData
    {
        public decimal Current { get; set; }
    }
}
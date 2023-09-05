using Feudal.Interfaces;
using Feudal.MessageBuses;
using System;
using System.Collections;
using System.Collections.Generic;

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

        public ClanManager(IMessageBus messageBus)
        {
            this.messageBus = messageBus;
            messageBus.Register(this);

            Clan.funcQueryTasks = (clanId) =>
            {
                return messageBus.PostMessage(new Message_QueryTasksInClan(clanId))
                    .WaitAck<ITask[]>();
            };

            list.Add(new Clan());
            list.Add(new Clan());
            list.Add(new Clan());
        }
    }

    public class Clan : IClan
    {
        internal static Func<string, ITask[]> funcQueryTasks;
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

        public Clan()
        {
            Id = clanId++.ToString();
            Name = Id;

            TotalLaborCount = 3;
        }
    }
}
using Feudal.Interfaces;
using Feudal.MessageBuses;
using System.Collections.Generic;
using System.Linq;

namespace Feudal.Tasks
{

    public abstract class Task : ITask
    {
        public IMessageBus messageBus { get; set; }
        public (int x, int y) Position { get; set; }

        private static int TaskId = 0;

        private readonly string id;
        public string Id => id;

        private string desc;
        public string Desc => desc;

        private int percent;
        public int Percent
        {
            get => percent;
            set
            {
                percent = value;
                if (percent >= 100)
                {
                    OnFinished();
                }
            }
        }

        public bool IsContinuous { get; set; }
        public string ClanId { get; set; }

        public Task(string clanId)
        {
            this.ClanId = clanId;

            id = TaskId++.ToString();
            desc = id;
        }

        internal virtual IEnumerable<Message> OnStart()
        {
            return Enumerable.Empty<Message>();
        }

        internal virtual IEnumerable<Message> OnCancel()
        {
            return Enumerable.Empty<Message>();
        }

        internal virtual IEnumerable<Message> OnFinished()
        {
            return Enumerable.Empty<Message>();
        }
    }
}
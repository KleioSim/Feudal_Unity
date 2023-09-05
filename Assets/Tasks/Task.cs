using Feudal.Interfaces;
using Feudal.MessageBuses;
using System;
using System.Collections;
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

        public string ClanId { get; set; }

        public Task(string clanId)
        {
            this.ClanId = clanId;

            id = TaskId++.ToString();
            desc = id;
        }

        abstract protected void OnFinished();

        abstract public void OnCancel();
    }

    public class DiscoverTask : Task
    {

        public DiscoverTask(string clanId, object[] parameters) : base(clanId)
        {
            Position = (((int x, int y))parameters[0]);
        }

        public override void OnCancel()
        {

        }

        protected override void OnFinished()
        {
            messageBus.PostMessage(new Message_TerrainItemDiscoverChanged(Position, true));

            for (int i = Position.x - 1; i <= Position.x + 1; i++)
            {
                for (int j = Position.y - 1; j <= Position.y + 1; j++)
                {
                    var pos = (i, j);
                    //if (_terrainItems.ContainsKey(pos))
                    //{
                    //    continue;
                    //}

                    messageBus.PostMessage(new Message_AddTerrainItem(pos, Terrain.Hill));
                }
            }
        }
    }

    public class TaskManager : IEnumerable<ITask>
    {
        private List<Task> list = new List<Task>();
        private IMessageBus messageBus;

        public TaskManager(IMessageBus messageBus)
        {
            this.messageBus = messageBus;
            messageBus.Register(this);
        }

        public IEnumerator<ITask> GetEnumerator()
        {
            return ((IEnumerable<Task>)list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)list).GetEnumerator();
        }

        [MessageProcess]
        public void OnMessage_AddTask(Message_AddTask message)
        {
            var task = Activator.CreateInstance(message.taskType, new object[] { message.clanId, message.parameters }) as Task;
            task.messageBus = messageBus;

            list.Add(task);
        }

        [MessageProcess]
        public void OnMessage_NextTurn(Message_NextTurn message)
        {
            if (message != null)
            {
                foreach (var task in list)
                {
                    task.Percent += 10;
                }
                list.RemoveAll(x => x.Percent >= 100);
            }
        }

        [MessageProcess]
        public void OnMessage_CancelTask(Message_CancelTask message)
        {
            var task = list.Single(x => x.Id == message.taskId);
            task.OnCancel();

            list.Remove(task);
        }

        [MessageProcess]
        public ITask[] OnMessage_QueryTasksInClan(Message_QueryTasksInClan message)
        {
            return list.Where(x => x.ClanId == message.clanId).ToArray();
        }
    }
}
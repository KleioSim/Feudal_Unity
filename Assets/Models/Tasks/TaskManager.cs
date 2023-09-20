using Feudal.Interfaces;
using Feudal.MessageBuses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Feudal.Tasks
{
    public class TaskManager : IEnumerable<ITask>
    {
        private List<Task> list = new List<Task>();
        private IMessageBus messageBus;

        public TaskManager(IMessageBus messageBus)
        {
            this.messageBus = messageBus;
            messageBus.Register(this);

            EstateWorkTask.funcGetEstate = (estateId) =>
            {
                return messageBus.PostMessage(new Message_FindEstateById(estateId)).WaitAck<IEstate>();
            };
        }

        public IEnumerator<ITask> GetEnumerator()
        {
            return ((IEnumerable<ITask>)list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)list).GetEnumerator();
        }

        [MessageProcess]
        public void OnMessage_AddTask(Message_AddTask message)
        {
            var task = Activator.CreateInstance(message.taskType, new object[] { message.clanId, message.parameters }) as Task;
            foreach(var startMessage in task.OnStart())
            {
                messageBus.PostMessage(startMessage);
            }

            list.Add(task);
        }

        [MessageProcess]
        public void OnMessage_NextTurn(Message_NextTurn message)
        {
            foreach (var task in list.Where(x=>!x.IsContinuous))
            {
                task.Percent += 10;
            }

            var finishedTask = list.Where(x => x.Percent >= 100).ToArray();
            foreach(var task in finishedTask)
            {
                foreach (var finshedMessage in task.OnFinished())
                {
                    messageBus.PostMessage(finshedMessage);
                }

                list.Remove(task);
            }
        }

        [MessageProcess]
        public void OnMessage_CancelTask(Message_CancelTask message)
        {
            var task = list.OfType<Task>().Single(x => x.Id == message.taskId);
            foreach (var cancelMessage in task.OnCancel())
            {
                messageBus.PostMessage(cancelMessage);
            }

            list.Remove(task);
        }

        [MessageProcess]
        public ITask[] OnMessage_QueryTasksInClan(Message_QueryTasksInClan message)
        {
            return list.Where(x => x.ClanId == message.clanId).ToArray();
        }
    }
}
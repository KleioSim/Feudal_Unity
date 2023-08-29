using Feudal.Interfaces;
using Feudal.MessageBuses;
using Feudal.Terrains;
using System;
using System.Collections;
using System.Collections.Generic;

public class Session
{
    public IEnumerable<ITerrainItem> terrainItems => terrainMgr;
    public IEnumerable<ITask> tasks => taskMgr;

    private IMessageBus messageBus;

    private TerrainManager terrainMgr;
    private TaskManager taskMgr;

    public void ExecUICmd(UICommand uiCmd)
    {
        switch(uiCmd)
        {
            case DiscoverCommand command:
                messageBus.PostMessage(new Message_AddTask(typeof(DiscoverTask), new object[] { command.position }));
                break;
            case NexTurnCommand command:
                messageBus.PostMessage(new Message_NextTurn());
                break;
            default:
                throw new Exception();
        }
    }

    public Session()
    {
        messageBus = new MessageBus();

        terrainMgr = new TerrainManager(messageBus);
        taskMgr = new TaskManager(messageBus);
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
        var task = Activator.CreateInstance(message.taskType, new object[] { message.parameters }) as Task;
        task.messageBus = messageBus;

        list.Add(task);
    }

    [MessageProcess]
    public void OnMessage_NextTurn(Message_NextTurn message)
    {
        if(message != null)
        {
            foreach (var task in list)
            {
                task.Percent += 10;
            }
            list.RemoveAll(x => x.Percent >= 100);
        }
    }
}

public interface ITask
{
    public string Id { get; }
    public string Desc { get; }
    public int Percent { get; }
}

abstract class Task : ITask
{
    public IMessageBus messageBus { get; set; }

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
            if(percent >= 100)
            {
                OnFinished();
            }
        }
    }

    public Task()
    {
        id = TaskId++.ToString();
        desc = id;
    }

    abstract protected void OnFinished();
}

class DiscoverTask : Task
{
    (int x, int y) position;

    public DiscoverTask(object[] parameters)
    {
        position = (((int x, int y)) parameters[0]);
    }

    protected override void OnFinished()
    {
        for (int i = position.x - 1; i <= position.x + 1; i++)
        {
            for (int j = position.y - 1; j <= position.y + 1; j++)
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
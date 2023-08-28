using Feudal.Interfaces;
using Feudal.MessageBuses;
using Feudal.Terrains;
using System;
using System.Collections.Generic;

public class Session
{
    public IEnumerable<ITerrainItem> terrainItems => terrainMgr;
    public IEnumerable<Task> tasks => _tasks;


    private List<Task> _tasks = new List<Task>();

    private IMessageBus messageBus;
    private TerrainManager terrainMgr;

    public void ExecUICmd(UICommand uiCmd)
    {
        switch(uiCmd)
        {
            case DiscoverCommand command:
                _tasks.Add(new Task(()=>
                {
                    for (int i = command.position.x - 1; i <= command.position.x + 1; i++)
                    {
                        for (int j = command.position.y - 1; j <= command.position.y + 1; j++)
                        {
                            var pos = (i, j);
                            //if (_terrainItems.ContainsKey(pos))
                            //{
                            //    continue;
                            //}

                            messageBus.PostMessage(new Message_AddTerrainItem(pos, Terrain.Hill));
                        }
                    }
                }));
                break;
            case NexTurnCommand command:
                foreach(var task in _tasks)
                {
                    task.Percent += 10;
                }
                _tasks.RemoveAll(x => x.Percent >= 100);
                break;
            default:
                throw new Exception();
        }
    }

    public Session()
    {
        messageBus = new MessageBus();

        terrainMgr = new TerrainManager(messageBus);
    }
}



public class Task
{
    public static int TaskId = 0;

    public readonly string taskId;
    public string desc;

    private int percent;
    public int Percent
    {
        get => percent;
        set
        {
            percent = value;
            if(percent >= 100)
            {
                finishAction?.Invoke();
            }
        }
    }

    private Action finishAction;

    public Task(Action finishAction)
    {
        taskId = TaskId++.ToString();

        desc = taskId;

        this.finishAction = finishAction;
    }
}

//public class DisCoverTask : Task
//{
//    private (int x, int y) position;

//    private Session session;

//    public DisCoverTask((int x, int y) position)
//    {
//        this.position = position;
//    }

//    protected override void OnFinished()
//    {
//        for (int i = position.x - 1; i <= position.x + 1; i++)
//        {
//            for (int j = position.y - 1; j <= position.y + 1; j++)
//            {
//                var pos = (i, j);
//                if (session._terrainItems.ContainsKey(pos))
//                {
//                    continue;
//                }

//                session._terrainItems.Add(pos, new TerrainItem(pos, Terrain.Hill));
//            }
//        }
//    }
//}


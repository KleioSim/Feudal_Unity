using System;
using System.Collections.Generic;

public class Session
{
    public IEnumerable<TerrainItem> terrainItems => _terrainItems.Values;
    public IEnumerable<Task> tasks => _tasks;

    private Dictionary<(int x, int y), TerrainItem> _terrainItems = new Dictionary<(int x, int y), TerrainItem>();
    private List<Task> _tasks = new List<Task>();

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
                            if (_terrainItems.ContainsKey(pos))
                            {
                                continue;
                            }

                            _terrainItems.Add(pos, new TerrainItem(pos, Terrain.Hill));
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
        _terrainItems.Add((0, 0), new TerrainItem((0, 0), Terrain.Hill));
        _terrainItems.Add((0, 1), new TerrainItem((0, 1), Terrain.Hill));
        _terrainItems.Add((1, 0), new TerrainItem((1, 0), Terrain.Hill));
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

public class TerrainItem
{
    public readonly (int x, int y) position;
    public Terrain terrain;

    public TerrainItem((int, int) position, Terrain terrain)
    {
        this.position = position;
        this.terrain = terrain;
    }
}

public enum Terrain
{
    Plain,
    Hill
}
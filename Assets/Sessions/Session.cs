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
                _tasks.Add(new DisCoverTask(command.position));
                //for (int i = command.position.x-1; i <= command.position.x+1; i++)
                //{
                //    for (int j = command.position.y-1; j <= command.position.y +1; j++)
                //    {
                //        var pos = (i,j);
                //        if (_terrainItems.ContainsKey(pos))
                //        {
                //            continue;
                //        }

                //        _terrainItems.Add(pos, new TerrainItem(pos, Terrain.Hill));
                //    }
                //}
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
    public int percent;

    public Task()
    {
        taskId = TaskId++.ToString();

        desc = taskId;
    }
}

public class DisCoverTask : Task
{
    private (int x, int y) position;

    public DisCoverTask((int x, int y) position)
    {
        this.position = position;
    }
}

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
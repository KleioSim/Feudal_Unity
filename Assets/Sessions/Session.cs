using System;
using System.Collections.Generic;

public class Session
{
    public IEnumerable<TerrainItem> terrainItems;

    public void ExecUICmd(object obj)
    {
        throw new NotImplementedException();
    }
}

public class TerrainItem
{
    public readonly (int x, int y) position;
    public string terrain;
}
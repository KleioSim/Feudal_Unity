using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainPawn : UIView
{
    public GameObject resource;
    public Grid grid;
    public TerrainPawnWorkHood workHood;

    private (int x, int y) position;
    public (int x, int y) Position
    {
        get => position;
        set
        {
            position = value;

            this.transform.position = grid.GetCellCenterWorld(new Vector3Int(position.x, position.y));
        }
    }
}

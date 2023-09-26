using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TerrainPawn : UIView
{
    public Tilemap terrainMap;
    public Text title;

    private (int x, int y) position;
    public (int x, int y) Position
    {
        get => position;
        set
        {
            position = value;

            this.transform.position = terrainMap.GetCellCenterWorld(new Vector3Int(position.x, position.y));
        }
    }
}

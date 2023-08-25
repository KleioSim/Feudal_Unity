using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using DataItem = KleioSim.Tilemaps.TilemapObservable.DataItem;

[RequireComponent(typeof(Tilemap))]
public class TilemapMask : MonoBehaviour
{
    public Tilemap terrainMap;
    public Sprite tileImage;

    private Tile _tile;
    public Tile tile
    {
        get
        {
            if (_tile == null)
            {
                _tile = ScriptableObject.CreateInstance<Tile>();
            }

            if (_tile.sprite != tileImage)
            {
                _tile.sprite = tileImage;
            }

            return _tile;
        }
    }

    Tilemap tilemap => GetComponent<Tilemap>();

    private void Start()
    {
        OnRefresh();
    }

    public void OnRefresh()
    {
        var c0 = tilemap.WorldToCell(Camera.main.ViewportToWorldPoint(new Vector3(0, 0)));
        var c1 = tilemap.WorldToCell(Camera.main.ViewportToWorldPoint(new Vector3(1, 0)));
        var c2 = tilemap.WorldToCell(Camera.main.ViewportToWorldPoint(new Vector3(0, 1)));
        var c3 = tilemap.WorldToCell(Camera.main.ViewportToWorldPoint(new Vector3(1, 1)));

        var array = new Vector3Int[] { c0, c1, c2, c3 };

        var minX = array.Select(c => c.x).Min();
        var minY = array.Select(c => c.y).Min();
        var maxX = array.Select(c => c.x).Max();
        var maxY = array.Select(c => c.y).Max();

        for(int x=minX; x<=maxX; x++)
        {
            for(int y=minY; y<=maxY; y++)
            {
                var pos = new Vector3Int(x, y);

                tilemap.SetTile(pos, terrainMap.HasTile(pos) ? null : tile);
            }
        }
    }

    public void OnTerrainMapAddItem(DataItem dataItem)
    {
        tilemap.SetTile(dataItem.Position, null);
    }

    public void OnTerrainMapRemoveItem(DataItem dataItem)
    {
        tilemap.SetTile(dataItem.Position, null);
    }
}

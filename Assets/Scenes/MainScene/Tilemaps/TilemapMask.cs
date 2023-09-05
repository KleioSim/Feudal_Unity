using KleioSim.Tilemaps;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using DataItem = KleioSim.Tilemaps.TilemapObservable.DataItem;

[RequireComponent(typeof(Tilemap))]
public class TilemapMask : MonoBehaviour
{
    public Camera camera;
    public TilemapObservable terrainMap;
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

    void Start()
    {
        terrainMap.Itemsource.CollectionChanged += TerrainMap_CollectionChanged;

        OnRefresh();
    }

    private void OnDestroy()
    {
        terrainMap.Itemsource.CollectionChanged -= TerrainMap_CollectionChanged;
    }

    private void TerrainMap_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if(e.NewItems != null)
        {
            foreach (DataItem newItem in e.NewItems)
            {
                tilemap.SetTile(newItem.Position, null);
            }
        }

        if(e.OldItems != null)
        {
            foreach (DataItem oldItem in e.OldItems)
            {
                tilemap.SetTile(oldItem.Position, tile);
            }
        }
    }


    public void OnRefresh()
    {
        var c0 = tilemap.WorldToCell(camera.ViewportToWorldPoint(new Vector3(0, 0)));
        var c1 = tilemap.WorldToCell(camera.ViewportToWorldPoint(new Vector3(1, 0)));
        var c2 = tilemap.WorldToCell(camera.ViewportToWorldPoint(new Vector3(0, 1)));
        var c3 = tilemap.WorldToCell(camera.ViewportToWorldPoint(new Vector3(1, 1)));

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

                tilemap.SetTile(pos, terrainMap.GetComponent<Tilemap>().HasTile(pos) ? null : tile);
            }
        }
    }
}

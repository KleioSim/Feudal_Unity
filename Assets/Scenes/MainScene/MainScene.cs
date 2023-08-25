using KleioSim.Tilemaps;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using DataItem = KleioSim.Tilemaps.TilemapObservable.DataItem;

namespace Feudal.Scenes.Main
{
    class MainScene : MonoBehaviour
    {
        public UnityEvent OnRefresh;

        public NoesisView noesisView;

        public TilemapObservable terrainMap;

        internal MainViewModelUnity mainViewMode;


        // Start is called before the first frame update
        void Awake()
        {
            mainViewMode = new MainViewModelUnity();
            noesisView.Content.DataContext = mainViewMode;

            terrainMap.Itemsource = mainViewMode.TerrainItems;
            terrainMap.OnClickTile.AddListener(OnTerrainMapClick);
        }


        public void OnTerrainMapClick(DataItem item)
        {
            if (!noesisView.IsHitted)
            {
                Debug.Log($"{item.Position} {item.TileKey}");

                mainViewMode.CreateMapItemDetail.Execute(null);

                for (int x=item.Position.x-1; x <= item.Position.x +1; x++)
                {
                    for (int y = item.Position.y - 1; y <= item.Position.y + 1; y++)
                    {
                        if(mainViewMode.TerrainItems.Any(i=> i.Position.x == x && i.Position.y == y))
                        {
                            continue;
                        }

                        mainViewMode.TerrainItems.Add(new DataItem() { Position = new Vector3Int(x, y), TileKey = Terrain.Plain });
                    }
                }

                OnRefresh.Invoke();
            }
        }
    }
}

[TileSetEnum]
public enum Terrain
{
    Plain,
    Hill
}
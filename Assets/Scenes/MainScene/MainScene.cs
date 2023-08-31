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
        public NoesisView noesisView;

        public TilemapObservable terrainMap;

        private MainViewModel mainViewModel;
        internal MainViewModel MainViewModel
        {
            get => mainViewModel;
            set
            {
                mainViewModel = value;
                mainViewModel.TerrainItems = terrainMap.Itemsource;

                noesisView.Content.DataContext = mainViewModel;
            }
        }

        void Awake()
        {
            MainViewModel = MainViewModel.Default;
            terrainMap.OnClickTile.AddListener(OnTerrainMapClick);
        }


        public void OnTerrainMapClick(DataItem item)
        {
            if (!noesisView.IsHitted)
            {
                Debug.Log($"{item.Position} {item.TileKey}");

                mainViewModel.ShowMapItemPanel.Execute(item);
                mainViewModel.testClickTerrainItem.Execute(item);
            }
        }
    }
}

[TileSetEnum]
public enum TerrainDataType
{
    Plain,
    Hill,

    Plain_Not_Dicovered,
    Hill_Not_Discovered
}
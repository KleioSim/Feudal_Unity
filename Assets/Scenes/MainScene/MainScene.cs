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

                var mapItemDetail = new MapDetailViewModel();
                mapItemDetail.Position = (item.Position.x, item.Position.y);

                mainViewModel.DetailPanel.Add(mapItemDetail);

                //mainViewModel.CreateMapItemDetail.Execute(null);
                mainViewModel.testClickTerrainItem.Execute(item);
            }
        }
    }
}

[TileSetEnum]
public enum TerrainDataType
{
    Plain,
    Hill
}
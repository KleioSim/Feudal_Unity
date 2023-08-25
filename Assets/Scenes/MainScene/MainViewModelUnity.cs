using System;
using System.Collections.ObjectModel;
using UnityEngine;
using DataItem = KleioSim.Tilemaps.TilemapObservable.DataItem;

namespace Feudal.Scenes.Main
{
    internal class MainViewModelUnity : MainViewModel
    {
        public Action<object> ExecUICmd;

        public ObservableCollection<DataItem> TerrainItems { get; } = new ObservableCollection<DataItem>();

        public RelayCommand<DataItem> testClickTerrainItem { get; }

        public MainViewModelUnity()
        {
            testClickTerrainItem = new RelayCommand<DataItem>((item) => 
            {
                ExecUICmd?.Invoke((item.Position.x, item.Position.y));
            });

            for (int i=0; i<3; i++)
            {
                for(int j=0; j<3; j++)
                {
                    TerrainItems.Add(new DataItem() { Position = new Vector3Int(i, j), TileKey = Terrain.Hill });
                }
            }

        }
    }
}
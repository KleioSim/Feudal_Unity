using System.Collections.ObjectModel;
using UnityEngine;
using DataItem = KleioSim.Tilemaps.TilemapObservable.DataItem;

namespace Feudal.Scenes.Main
{
    internal class MainViewModelUnity : MainViewModel
    {
        public ObservableCollection<DataItem> TerrainItems { get; } = new ObservableCollection<DataItem>();

        public MainViewModelUnity()
        {
            for(int i=0; i<3; i++)
            {
                for(int j=0; j<3; j++)
                {
                    TerrainItems.Add(new DataItem() { Position = new Vector3Int(i, j), TileKey = Terrain.Hill.ToString() });
                }
            }

        }
    }
}
using KleioSim.Tilemaps;
using System.Collections.ObjectModel;
using UnityEngine;

using DataItem = KleioSim.Tilemaps.TilemapObservable.DataItem;

namespace Fedual.MainScene
{
    class MainViewModel : ViewModelBehaviour
    {
        private ObservableCollection<DataItem> terrainMap;
        public  ObservableCollection<DataItem> TerrainMap
        {
            get => terrainMap;
            set => SetProperty(ref terrainMap, value);
        }

        void Start()
        {
            TerrainMap = new ObservableCollection<DataItem>(new DataItem[] { new DataItem() { Position = new Vector3Int(0,0), TileKey = Terrain.Plain.ToString() } });
        }
    }

    [TileSetEnum]
    public enum Terrain
    {
        Plain,
        Hill,
        Mountion
    }
}
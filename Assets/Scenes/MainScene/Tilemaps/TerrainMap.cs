using KleioSim.Tilemaps;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;
using DataItem = KleioSim.Tilemaps.TilemapObservable.DataItem;

namespace Feudal.Scenes.Main
{
    [RequireComponent(typeof(TilemapObservable))]
    public class TerrainMap : UIView
    {
        public ObservableCollection<DataItem> terrainItems => GetComponent<TilemapObservable>().Itemsource;
        public UnityEvent<DataItem> OnTerrainClick => GetComponent<TilemapObservable>().OnClickTile;
    }
}

using KleioSim.Tilemaps;
using System;
using System.Linq;
using UnityEngine;
using DataItem = KleioSim.Tilemaps.TilemapObservable.DataItem;

namespace Feudal.Scenes.Main
{
    class MainScene : MonoBehaviour
    {
        public TerrainMap terrainMap;
        public TestPanel testPanel;

        void Awake()
        {
            terrainMap.OnTerrainClick.AddListener(OnTerrainMapClick);
        }

        public void OnTerrainMapClick(DataItem item)
        {
            testPanel.gameObject.SetActive(true);
            testPanel.ObjId = item;

            UIView.ExecUICmd?.Invoke(new UpdateViewCommand());
        }
    }
}

[TileSetEnum]
public enum TerrainDataType
{
    Plain,
    Hill,
    Mountion,
    Lake,
    Marsh,

    Plain_Unknown,
    Hill_Unknown,
    Mountion_Unknown,
    Lake_Unknown,
    Marsh_Unknown,
}
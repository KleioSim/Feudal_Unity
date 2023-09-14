using KleioSim.Tilemaps;
using System;
using System.Linq;
using UnityEngine;
using DataItem = KleioSim.Tilemaps.TilemapObservable.DataItem;

namespace Feudal.Scenes.Main
{
    class MainScene : MonoBehaviour
    {
        public TilemapObservable terrainMap;
        public RightPanel rightPanel;

        void Awake()
        {
            terrainMap.OnClickTile.AddListener(OnTerrainMapClick);
        }

        public void OnTerrainMapClick(DataItem item)
        {
            var terrainDetail = rightPanel.SetCurrentMain<TerrainDetailPanel>();
            terrainDetail.Position = (item.Position.x, item.Position.y);

            UIView.ExecUICmd(new UpdateViewCommand());
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
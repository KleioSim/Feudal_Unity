using KleioSim.Tilemaps;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DataItem = KleioSim.Tilemaps.TilemapObservable.DataItem;

namespace Feudal.Scenes.Main
{
    class MainScene : MonoBehaviour
    {
        public TerrainMap terrainMap;
        public RightPanel rightPanel;

        void Awake()
        {
            terrainMap.ObjId = "";
            terrainMap.OnTerrainClick.AddListener(OnTerrainMapClick);
        }

        public void OnTerrainMapClick(DataItem item)
        {
            var terrainDetail = rightPanel.SetCurrent<TerrainDetailPanel>();
            terrainDetail.ObjId = (item.Position.x, item.Position.y);

            UIView.ExecUICmd?.Invoke(new UpdateViewCommand());
        }
    }

    public class WorkHood : MonoBehaviour
    {
        public (int x, int y) position { get; set; }
    }

    public class DisoverWorkHood : WorkHood
    {
        public Text title;
        public Slider percent;
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
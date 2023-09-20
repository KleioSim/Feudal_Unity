using KleioSim.Tilemaps;
using System;
using System.Linq;
using UnityEngine;
using DataItem = KleioSim.Tilemaps.TilemapObservable.DataItem;

namespace Feudal.Scenes.Main
{
    public class MainScene : UIView
    {
        public TilemapObservable terrainMap;
        public RightPanel rightPanel;

        void Awake()
        {
            terrainMap.OnClickTile.AddListener(OnTerrainMapClick);
        }

        public void OnTerrainMapClick(DataItem item)
        {
            var terrainDetail = rightPanel.OnShowMainView<TerrainDetailPanel>();
            terrainDetail.Position = (item.Position.x, item.Position.y);

            ExecUICmd(new UpdateViewCommand());
        }

        public void OnNextTurn()
        {
            ExecUICmd(new NexTurnCommand());
        }
    }
}

public class UIView : MonoBehaviour
{
    public static Action<UICommand> ExecUICmd = (command) =>
    {
        Debug.Log($"trigger command:{command}");
    };
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
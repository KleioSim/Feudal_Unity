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
            var position = (item.Position.x, item.Position.y);
            var terrainDetail = rightPanel.OnShowMainView<TerrainDetailPanel>(position);

            ExecUICmd(new UpdateViewCommand());
        }

        public void OnShowEstates()
        {
            var estateStaticsPanel = rightPanel.OnShowMainView<EstateStaticsPanel>();
            estateStaticsPanel.onClickEstateItem = (position) =>
            {
                var terrainDetail = rightPanel.OnShowMainView<TerrainDetailPanel>(position);
                ExecUICmd(new UpdateViewCommand());
            };

            ExecUICmd(new UpdateViewCommand());
        }

        public void OnShowClans()
        {
            var clanStaticsPanel = rightPanel.OnShowMainView<ClanStaticsPanel>();
            //estateStaticsPanel.onClickEstateItem = (position) =>
            //{
            //    var terrainDetail = rightPanel.OnShowMainView<TerrainDetailPanel>(position);
            //    ExecUICmd(new UpdateViewCommand());
            //};

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
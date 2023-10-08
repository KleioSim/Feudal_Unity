using KleioSim.Tilemaps;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DataItem = KleioSim.Tilemaps.TilemapObservable.DataItem;

namespace Feudal.Scenes.Main
{
    public class MainScene : UIView
    {
        public TilemapObservable terrainMap;
        public RightPanel rightPanel;

        public string PlayerClanId { get; set; }

        void Awake()
        {
            terrainMap.OnClickTile.AddListener(item=>OnTerrainMapClick(item.Position));
        }

        public void OnTerrainMapClick(Vector3Int position)
        {
            rightPanel.OnShowMainView<TerrainDetailPanel>((position.x, position.y));
        }

        public void OnShowEstates()
        {
            var clanDetail = ShowClanDetailPanel(PlayerClanId);
            clanDetail.CollapseAll();

            clanDetail.GetComponentInChildren<TotalEstateView>().toggle.isOn = true;
        }

        public void OnShowClans()
        {
            var clanStaticsPanel = rightPanel.OnShowMainView<ClanStaticsPanel>();
            clanStaticsPanel.onClickClanItem = (clanId) =>
            {
                ShowClanDetailPanel(clanId);
            };
        }

        private ClanDetailPanel ShowClanDetailPanel(string clanId)
        {
            var clanDetail = rightPanel.OnShowMainView<ClanDetailPanel>(clanId);
            clanDetail.onClickEstateItem = (position) =>
            {
                rightPanel.OnShowMainView<TerrainDetailPanel>(position);
            };

            return clanDetail;
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

    public static Action<UIView> OnEnableAction;

    private bool firstUpdate = true;

    protected virtual void Update()
    {
        if(firstUpdate)
        {
            OnEnableAction?.Invoke(this);

            firstUpdate = false;
        }
    }

    protected virtual void OnDisable()
    {
        firstUpdate = true;
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
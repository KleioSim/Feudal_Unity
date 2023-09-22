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
            terrainMap.OnClickTile.AddListener(OnTerrainMapClick);
        }

        public void OnTerrainMapClick(DataItem item)
        {
            var position = (item.Position.x, item.Position.y);
            rightPanel.OnShowMainView<TerrainDetailPanel>(position);
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


    protected virtual void OnEnable()
    {
        if(didStart)
        {
            OnEnableAction?.Invoke(this);
        }
    }

    //TODO show replace with buildin didStart in Untiy2023.1
    private bool didStart;
    protected virtual void Start()
    {
        didStart = true;

        OnEnableAction?.Invoke(this);
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
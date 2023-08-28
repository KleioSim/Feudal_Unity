using System;
using System.Collections.ObjectModel;
using UnityEngine;
using DataItem = KleioSim.Tilemaps.TilemapObservable.DataItem;

namespace Feudal.Scenes.Main
{
    internal class MainViewModelUnity : MainViewModel
    {
        public Action<UICommand> ExecUICmd;

        public ObservableCollection<DataItem> TerrainItems { get; } = new ObservableCollection<DataItem>();

        public RelayCommand<DataItem> testClickTerrainItem { get; }

        public MainViewModelUnity()
        {
            testClickTerrainItem = new RelayCommand<DataItem>((item) => 
            {
                ExecUICmd?.Invoke(new DiscoverCommand(item.Position.x, item.Position.y));
            });
        }
    }
}
#if UNITY_5_3_OR_NEWER
#define NOESIS
using DataItem = KleioSim.Tilemaps.TilemapObservable.DataItem;
#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
#endif

using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Feudal.Scenes.Main
{
    internal partial class MainViewModel : ViewModel
    {
        public DetailPanelViewModel DetailPanel { get; } = new DetailPanelViewModel();

        //public RelayCommand<DataItem> CreateMapItemDetail { get; }
        public RelayCommand RemoveMapItemDetail { get; }
        public RelayCommand NexTurn { get; }
        public RelayCommand ShowLaborsPanel { get; }
        public RelayCommand ShowMapItemPanel { get; }

        private MapDetailViewModel mapItemDetail;
        public MapDetailViewModel MapItemDetail
        {
            get => mapItemDetail;
            private set => SetProperty(ref mapItemDetail, value);
        }

        public ObservableCollection<TaskViewModel> Tasks { get; }

        public MainViewModel()
        {
            //CreateMapItemDetail = new RelayCommand<DataItem> ((dataItem) =>
            //{
            //    MapItemDetail = new MapDetailViewModel();
            //    MapItemDetail.Position = (dataItem.Position.x, dataItem.Position.y);
            //});

            RemoveMapItemDetail = new RelayCommand(() =>
            {
                MapItemDetail = null;
            });

            Tasks = new ObservableCollection<TaskViewModel>();

            NexTurn = new RelayCommand(() =>
            {
#if UNITY_5_3_OR_NEWER
                ExecUICmd?.Invoke(new NexTurnCommand());
#endif
            });

#if UNITY_5_3_OR_NEWER
            testClickTerrainItem = new RelayCommand<DataItem>((item) =>
            {
                ExecUICmd?.Invoke(new DiscoverCommand(item.Position.x, item.Position.y));
            });

            TaskViewModel.CancelAction = (taskId) =>
            {
                ExecUICmd?.Invoke(new CancelTaskCommand(taskId));
            };
#endif
        }

#if UNITY_5_3_OR_NEWER

        public Action<UICommand> ExecUICmd;

        public ObservableCollection<DataItem> TerrainItems { get; internal set; }

        public RelayCommand<DataItem> testClickTerrainItem { get; }
#endif

    }

    public class DetailPanelViewModel : ViewModel
    {
        private List<ViewModel> list = new List<ViewModel>();

        public RelayCommand ClosePanel { get; }

        private ViewModel current;
        public ViewModel Current
        {
            get => current;
            set => SetProperty(ref current, value);
        }

        public DetailPanelViewModel()
        {
            ClosePanel = new RelayCommand(() => 
            {
                list.Clear();
                Current = null;
            });
        }

        internal void Add(ViewModel mapItemDetail)
        {
            list.Add(mapItemDetail);

            Current = mapItemDetail;
        }
    }
}
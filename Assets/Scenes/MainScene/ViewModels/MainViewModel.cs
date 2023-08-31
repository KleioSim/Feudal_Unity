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

namespace Feudal.Scenes.Main
{
    internal partial class MainViewModel : ViewModel
    {
        public DetailPanelViewModel DetailPanel { get; } = new DetailPanelViewModel();

        public RelayCommand NexTurn { get; }
        public RelayCommand ShowClansPanel { get; }

#if UNITY_5_3_OR_NEWER
        public RelayCommand<DataItem> ShowMapItemPanel { get; }
#endif

        public ObservableCollection<TaskViewModel> Tasks { get; }

        public MainViewModel()
        {
            ShowClansPanel = new RelayCommand(() =>
            {
                var viewModel = new ClansPanelViewModel();
                DetailPanel.Add(viewModel);

#if UNITY_5_3_OR_NEWER
                ExecUICmd?.Invoke(new UpdateViewCommand());
#endif
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
                ExecUICmd?.Invoke(new DiscoverCommand((item.Position.x, item.Position.y)));
            });

            TaskViewModel.CancelAction = (taskId) =>
            {
                ExecUICmd?.Invoke(new CancelTaskCommand(taskId));
            };
            
            ShowMapItemPanel = new RelayCommand<DataItem>((item) =>
            {
                var viewModel = new MapDetailViewModel();
                viewModel.Position = (item.Position.x, item.Position.y);
                viewModel.ExecUICmd = ExecUICmd;

                DetailPanel.Add(viewModel);

                ExecUICmd?.Invoke(new UpdateViewCommand());
            });
#endif
        }

#if UNITY_5_3_OR_NEWER

        public Action<UICommand> ExecUICmd;

        public ObservableCollection<DataItem> TerrainItems { get; internal set; }

        public RelayCommand<DataItem> testClickTerrainItem { get; }
#endif

    }
}
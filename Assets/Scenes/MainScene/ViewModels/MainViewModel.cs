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
        public ClanViewModel PlayerClan { get; } = new ClanViewModel();
        public DetailPanelViewModel DetailPanel { get; } = new DetailPanelViewModel();

        public RelayCommand NexTurn { get; }
        public RelayCommand ShowClansPanel { get; }
        public RelayCommand ShowPlayerClanPanel { get; }

        public ObservableCollection<TaskViewModel> Tasks { get; }

#if UNITY_5_3_OR_NEWER
        public ObservableCollection<DataItem> TerrainItems { get; internal set; }

        public RelayCommand<DataItem> ShowMapItemPanel { get; }
#endif

        public MainViewModel()
        {
            ShowClansPanel = new RelayCommand(() =>
            {
                var viewModel = new ClansPanelViewModel();
                DetailPanel.Add(viewModel);

                ExecUICmd?.Invoke(new UpdateViewCommand());
            });

            ShowPlayerClanPanel = new RelayCommand(() =>
            {
                var viewModel = new ClanPanelViewModel();
                viewModel.ClanViewModel = PlayerClan;

                DetailPanel.Add(viewModel);

                ExecUICmd?.Invoke(new UpdateViewCommand());
            });

            Tasks = new ObservableCollection<TaskViewModel>();

            NexTurn = new RelayCommand(() =>
            {
                ExecUICmd?.Invoke(new NexTurnCommand());
            });

#if UNITY_5_3_OR_NEWER

            TaskViewModel.CancelAction = (taskId) =>
            {
                ExecUICmd?.Invoke(new CancelTaskCommand(taskId));
            };
            
            ShowMapItemPanel = new RelayCommand<DataItem>((item) =>
            {
                var viewModel = new MapDetailViewModel();
                viewModel.Position = (item.Position.x, item.Position.y);

                DetailPanel.Add(viewModel);

                ExecUICmd?.Invoke(new UpdateViewCommand());
            });
#endif
        }

    }
}
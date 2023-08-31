#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
#endif

using System;
using System.Collections.ObjectModel;

namespace Feudal.Scenes.Main
{
    internal partial class MapDetailViewModel : ViewModel
    {
#if UNITY_5_3_OR_NEWER
        public Action<UICommand> ExecUICmd;
#endif
        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private (int x, int y) position;
        public (int x, int y) Position
        {
            get => position;
            set => SetProperty(ref position, value);
        }

        private MapWorkerPanelViewModel mapWorkerPanel;
        public MapWorkerPanelViewModel MapWorkerPanel
        {
            get => mapWorkerPanel;
            set => SetProperty(ref mapWorkerPanel, value);
        }

        private string desc;
        public string Desc
        {
            get => desc;
            set => SetProperty(ref desc, value);
        }

        public ObservableCollection<MapItemCommand> Commands { get; } = new ObservableCollection<MapItemCommand>();

        public MapDetailViewModel()
        {
            Commands.Add(new MapItemCommand(
                "Discover",
                new RelayCommand(() => 
                {
#if UNITY_5_3_OR_NEWER
                    ExecUICmd?.Invoke(new DiscoverCommand(Position)); 
#endif
                })));
        }


    }

    public class MapItemCommand : ViewModel
    {
        private string desc;
        public string Desc
        {
            get => desc;
            set => SetProperty(ref desc, value);
        }

        public RelayCommand Command { get; }

        public MapItemCommand(string desc, RelayCommand command)
        {
            Desc = desc;
            Command = command;
        }
    }

    class MapWorkerPanelViewModel : ViewModel
    {
        private string workClanName;
        public string WorkClanName
        {
            get => workClanName;
            set => SetProperty(ref workClanName, value);
        }
    }
}
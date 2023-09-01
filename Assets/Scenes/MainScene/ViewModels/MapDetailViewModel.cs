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

        private DiscoverPanelViewModel discoverPanel;
        public DiscoverPanelViewModel DiscoverPanel
        {
            get => discoverPanel;
            set
            {
                SetProperty(ref discoverPanel, value);
                if(discoverPanel != null)
                {
#if UNITY_5_3_OR_NEWER
                    discoverPanel.ExecUICmd = ExecUICmd;
#endif
                }
            }
        }

        private string desc;
        public string Desc
        {
            get => desc;
            set => SetProperty(ref desc, value);
        }


        public MapDetailViewModel()
        {

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

    class DiscoverPanelViewModel : ViewModel
    {
#if UNITY_5_3_OR_NEWER
        public Action<UICommand> ExecUICmd;
#endif

        private (int x, int y) position;
        public (int x, int y) Position
        {
            get => position;
            set => SetProperty(ref position, value);
        }

        private int percent;
        public int Percent
        {
            get => percent;
            set => SetProperty(ref percent, value);
        }

        private WorkerLaborViewModel workerLabor;
        public WorkerLaborViewModel WorkerLabor
        {
            get => workerLabor;
            set => SetProperty(ref workerLabor, value);
        }

        public RelayCommand Start { get; }
        public RelayCommand Cancel { get; }

        public DiscoverPanelViewModel()
        {
            Start = new RelayCommand(() => 
            {
#if UNITY_5_3_OR_NEWER
                ExecUICmd.Invoke(new DiscoverCommand(Position)); 
#endif
            });

            Cancel = new RelayCommand(() => 
            {
#if UNITY_5_3_OR_NEWER
                ExecUICmd.Invoke(new CancelTaskCommand(WorkerLabor.TaskId)); 
#endif
            });
        }
    }

    class WorkerLaborViewModel : ViewModel
    {
        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string taskId;
        public string TaskId
        {
            get => taskId;
            set => SetProperty(ref taskId, value);
        }
    }
}
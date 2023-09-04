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
    internal partial class MapDetailViewModel : PanelViewModel
    {
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
                if(discoverPanel == null)
                {
                    return;
                }

                discoverPanel.ShowLaborSeletor = new RelayCommand(() =>
                {
                    var laborSelectorViewModel = new LaborSelectorViewModel();

                    laborSelectorViewModel.Confirm = new RelayCommand(() =>
                    {
                        ExecUICmd?.Invoke(new DiscoverCommand(position));
                        SubViewModel = null;
                    },
                    () =>
                    {
                        return laborSelectorViewModel.SelectedLabor != null;
                    });

                    laborSelectorViewModel.SelectedLabor = null;


                    SubViewModel = laborSelectorViewModel;
                    ExecUICmd?.Invoke(new UpdateViewCommand());
                });
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
            SubPanelClose = new RelayCommand(() =>
            {
                SubViewModel = null;
            });
        }
    }

    class DiscoverPanelViewModel : ViewModel
    {
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

        public RelayCommand showLaborSeletor;
        public RelayCommand ShowLaborSeletor
        {
            get => showLaborSeletor;
            set => SetProperty(ref showLaborSeletor, value);
        }

        public RelayCommand Cancel { get; }

        public DiscoverPanelViewModel()
        {
            Cancel = new RelayCommand(() => 
            {
                ExecUICmd.Invoke(new CancelTaskCommand(WorkerLabor.TaskId));
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

    class LaborSelectorViewModel : ViewModel
    {
        private static LaborSelectorViewModel @default;
        public static LaborSelectorViewModel Default
        {
            get
            {
                if(@default== null)
                {
                    @default = new LaborSelectorViewModel();

                    @default.Labors.Add(new LaborViewModel("CLAN0") { Title = "CLAN0" });
                    @default.Labors.Add(new LaborViewModel("CLAN1") { Title = "CLAN1" });
                    @default.Labors.Add(new LaborViewModel("CLAN2") { Title = "CLAN2" });
                    @default.Labors.Add(new LaborViewModel("CLAN3") { Title = "CLAN3" });
                }

                return @default;
            }
        }

        public ObservableCollection<LaborViewModel> Labors { get; } = new ObservableCollection<LaborViewModel>();

        private RelayCommand confirm;
        public RelayCommand Confirm
        {
            get => confirm;
            set => SetProperty(ref confirm, value);
        }

        private LaborViewModel selectedLabor;
        public LaborViewModel SelectedLabor
        {
            get => selectedLabor;
            set
            {
                SetProperty(ref selectedLabor, value);
                Confirm.RaiseCanExecuteChanged();
            }
        }
    }

    class LaborViewModel : ViewModel
    {
        public readonly string clanId;

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public LaborViewModel(string clanId)
        {
            this.clanId = clanId;
        }
    }
}
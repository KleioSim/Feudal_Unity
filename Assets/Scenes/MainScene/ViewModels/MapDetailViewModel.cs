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
    class TraitViewModel : ViewModel
    {
        public readonly Enum trait;
        
        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public TraitViewModel()
        {

        }

        public TraitViewModel(Enum trait)
        {
            this.trait = trait;
            Title = trait.ToString();
        }
    }

    internal partial class MapDetailViewModel : DetailPanelViewModel
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

        private WorkHoodViewModel workHood;
        public WorkHoodViewModel WorkHood
        {
            get => workHood;
            set => SetProperty(ref workHood, value);
        }

        private LaborViewModel labor;
        public LaborViewModel Labor
        {
            get => labor;
            set => SetProperty(ref labor, value);
        }

        private string desc;
        public string Desc
        {
            get => desc;
            set => SetProperty(ref desc, value);
        }

        public ObservableCollection<TraitViewModel> Traits { get; } = new ObservableCollection<TraitViewModel>();

        public RelayCommand OccupyLabor { get; }
        public RelayCommand FreeLabor { get; }

        public MapDetailViewModel()
        {
            OccupyLabor = new RelayCommand(() =>
            {
                var laborSelector = new LaborSelectorViewModel();
                SubViewModel = laborSelector;

                laborSelector.Confirm = new RelayCommand(() =>
                {
                    WorkHood.OccupyLabor.Execute(laborSelector.SelectedLabor);
                    SubViewModel = null;
                },
                () =>
                {
                    return laborSelector.SelectedLabor != null;
                });

                ExecUICmd?.Invoke(new UpdateViewCommand());
            });

            FreeLabor = new RelayCommand(() =>
            {

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
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

        private WorkViewModel workViewModel;
        public WorkViewModel WorkViewModel
        {
            get => workViewModel;
            set
            {
                SetProperty(ref workViewModel, value);
                if (workViewModel == null)
                {
                    return;
                }

                workViewModel.ShowLaborSeletor = new RelayCommand(() =>
                {
                    var laborSelectorViewModel = new LaborSelectorViewModel();

                    laborSelectorViewModel.Confirm = new RelayCommand(() =>
                    {
                        workViewModel.Start.Execute(laborSelectorViewModel.SelectedLabor);
                        SubViewModel = null;
                    },
                    () =>
                    {
                        return laborSelectorViewModel.SelectedLabor != null;
                    });

                    SubViewModel = laborSelectorViewModel;
                    ExecUICmd?.Invoke(new UpdateViewCommand());
                });
            }
        }

        public ObservableCollection<TraitViewModel> Traits { get; } = new ObservableCollection<TraitViewModel>();

        //private DiscoverPanelViewModel discoverPanel;
        //public DiscoverPanelViewModel DiscoverPanel
        //{
        //    get => discoverPanel;
        //    set
        //    {
        //        SetProperty(ref discoverPanel, value);
        //        if(discoverPanel == null)
        //        {
        //            return;
        //        }

        //        discoverPanel.ShowLaborSeletor = new RelayCommand(() =>
        //        {
        //            var laborSelectorViewModel = new LaborSelectorViewModel();

        //            laborSelectorViewModel.Confirm = new RelayCommand(() =>
        //            {
        //                ExecUICmd?.Invoke(new DiscoverCommand(laborSelectorViewModel.SelectedLabor.clanId, position));
        //                SubViewModel = null;
        //            },
        //            () =>
        //            {
        //                return laborSelectorViewModel.SelectedLabor != null;
        //            });

        //            SubViewModel = laborSelectorViewModel;
        //            ExecUICmd?.Invoke(new UpdateViewCommand());
        //        });
        //    }
        //}

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
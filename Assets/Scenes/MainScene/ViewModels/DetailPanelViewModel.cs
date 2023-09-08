#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
#endif

using System.Collections.Generic;

namespace Feudal.Scenes.Main
{
    public class DetailPanelViewModel : ViewModel
    {
        private List<ViewModel> list = new List<ViewModel>();


        private ViewModel current;
        public ViewModel Current
        {
            get => current;
            set => SetProperty(ref current, value);
        }

        internal void Add(PanelViewModel panelViewModel)
        {
            panelViewModel.ClosePanel = new RelayCommand(() =>
            {
                Current = null;
                list.Clear();
            });

            list.Add(panelViewModel);
            Current = panelViewModel;
        }
    }

    public interface IPanelViewModel
    {
        RelayCommand ClosePanel { get; }
        RelayCommand SubPanelClose { get; }

        ViewModel SubViewModel { get; set; }

    }

    public class PanelViewModel : ViewModel, IPanelViewModel
    {
        public RelayCommand ClosePanel { get; internal set; }

        private ViewModel subViewModel;
        public ViewModel SubViewModel
        {
            get => subViewModel;
            set => SetProperty(ref subViewModel, value);
        }

        public RelayCommand SubPanelClose { get; internal set; }

        public PanelViewModel()
        {
            SubPanelClose = new RelayCommand(() => SubViewModel = null);
        }
    }
}
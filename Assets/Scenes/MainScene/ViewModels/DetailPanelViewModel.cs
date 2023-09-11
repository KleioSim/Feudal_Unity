#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
#endif

using System.Collections.Generic;
using System.Linq;

namespace Feudal.Scenes.Main
{
    public partial class DetailPanelViewModel : ViewModel
    {
        private List<ViewModel> list = new List<ViewModel>();

        private ViewModel current;
        public ViewModel Current
        {
            get => current;
            set => SetProperty(ref current, value);
        }

        private ViewModel subViewModel;
        public ViewModel SubViewModel
        {
            get => subViewModel;
            set => SetProperty(ref subViewModel, value);
        }

        private int index;
        public int Index
        {
            get => index;
            set
            {
                SetProperty(ref index, value);
                Current = list[index];
            }
        }

        public RelayCommand NextPanel { get; internal set; }
        public RelayCommand PrevPanel { get; internal set; }
        public RelayCommand ClosePanel { get; internal set; }
        public RelayCommand<ViewModel> AddPanel { get; internal set; }

        public DetailPanelViewModel()
        {
            ClosePanel = new RelayCommand(() =>
            {
                Current = null;
                list.Clear();
            });

            PrevPanel = new RelayCommand(() =>
            {
                if (Index > 0)
                {
                    Index--;
                }
            });

            NextPanel = new RelayCommand(() =>
            {
                if (Index < list.Count() - 1)
                {
                    Index++;
                }
            });

            AddPanel = new RelayCommand<ViewModel>((panel) =>
            {
                list.Add(panel);

                Index = list.Count() - 1;

                ExecUICmd?.Invoke(new UpdateViewCommand());
            });
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
        public RelayCommand<PanelViewModel> AddPanel { get; internal set; }

        public RelayCommand NextPanel { get; internal set; }
        public RelayCommand PrevPanel { get; internal set; }
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
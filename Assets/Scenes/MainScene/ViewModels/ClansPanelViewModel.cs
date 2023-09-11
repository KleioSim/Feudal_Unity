#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
#endif

using System.Collections.ObjectModel;

namespace Feudal.Scenes.Main
{
    partial class ClansPanelViewModel : PanelViewModel
    {

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public ObservableCollection<ClanViewModel> ClanItems { get; } = new ObservableCollection<ClanViewModel>();
    }
}
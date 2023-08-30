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
    partial class ClansPanelViewModel : ViewModel
    {

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public ObservableCollection<ClansItemViewModel> ClanItems { get; } = new ObservableCollection<ClansItemViewModel>();
    }

    class ClansItemViewModel : ViewModel
    {
        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
    }
}
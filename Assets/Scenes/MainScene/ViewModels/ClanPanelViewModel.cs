#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
#endif

namespace Feudal.Scenes.Main
{
    partial class ClanPanelViewModel : PanelViewModel
    {
        private ClanViewModel clanViewModel;
        public ClanViewModel ClanViewModel
        {
            get => clanViewModel;
            set => SetProperty(ref clanViewModel, value);
        }
    }
}

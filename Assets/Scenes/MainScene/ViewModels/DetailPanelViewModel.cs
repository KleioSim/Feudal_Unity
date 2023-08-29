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

        public RelayCommand ClosePanel { get; }

        private ViewModel current;
        public ViewModel Current
        {
            get => current;
            set => SetProperty(ref current, value);
        }

        public DetailPanelViewModel()
        {
            ClosePanel = new RelayCommand(() => 
            {
                list.Clear();
                Current = null;
            });
        }

        internal void Add(ViewModel mapItemDetail)
        {
            list.Add(mapItemDetail);

            Current = mapItemDetail;
        }
    }
}
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

    class ClanViewModel : ViewModel
    {
        private string clanId;
        public string ClanId
        {
            get => clanId;
            set => SetProperty(ref clanId, value);
        }

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private int popCount;
        public int PopCount
        {
            get => popCount;
            set => SetProperty(ref popCount, value);
        }

        public ObservableCollection<EstateViewModel> Estates { get; } = new ObservableCollection<EstateViewModel>();

        private decimal food;
        public decimal Food
        {
            get => food;
            set => SetProperty(ref food, value);
        }

        private decimal foodSurplus;
        public decimal FoodSurplus
        {
            get => foodSurplus;
            set => SetProperty(ref foodSurplus, value);
        }
    }

    public class EstateViewModel : ViewModel
    {
        private string estateId;
        public string EstateId
        {
            get => estateId;
            set => SetProperty(ref estateId, value);
        }
    }
}
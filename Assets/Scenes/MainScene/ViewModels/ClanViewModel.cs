#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
#endif

using System.Collections.ObjectModel;

namespace Feudal.Scenes.Main
{
    partial class ClanViewModel : ViewModel
    {

        public ObservableCollection<EstateViewModel> Estates { get; } = new ObservableCollection<EstateViewModel>();

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

        private ViewModel selectedItemViewModel;
        public ViewModel SelectedItemViewModel
        {
            get => selectedItemViewModel;
            set => SetProperty(ref selectedItemViewModel, value);
        }

        public RelayCommand ShowPlayerClanPanel { get; }
    }
}
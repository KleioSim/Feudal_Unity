#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
#endif

using System.Collections.ObjectModel;

namespace Feudal.Scenes.Main
{
    class LaborSelectorViewModel : ViewModel
    {
        private static LaborSelectorViewModel @default;
        public static LaborSelectorViewModel Default
        {
            get
            {
                if(@default== null)
                {
                    @default = new LaborSelectorViewModel();

                    @default.Labors.Add(new LaborViewModel("CLAN0") { Title = "CLAN0", IdleCount = 3, TotalCount = 10 });
                    @default.Labors.Add(new LaborViewModel("CLAN1") { Title = "CLAN1", IdleCount = 0, TotalCount = 1 });
                    @default.Labors.Add(new LaborViewModel("CLAN2") { Title = "CLAN2", IdleCount = 1, TotalCount = 1 });
                    @default.Labors.Add(new LaborViewModel("CLAN3") { Title = "CLAN3", IdleCount = 0, TotalCount = 0 });
                }

                return @default;
            }
        }

        public ObservableCollection<LaborViewModel> Labors { get; } = new ObservableCollection<LaborViewModel>();

        private RelayCommand confirm;
        public RelayCommand Confirm
        {
            get => confirm;
            set => SetProperty(ref confirm, value);
        }

        private LaborViewModel selectedLabor;
        public LaborViewModel SelectedLabor
        {
            get => selectedLabor;
            set
            {
                SetProperty(ref selectedLabor, value);
                Confirm.RaiseCanExecuteChanged();
            }
        }
    }
}
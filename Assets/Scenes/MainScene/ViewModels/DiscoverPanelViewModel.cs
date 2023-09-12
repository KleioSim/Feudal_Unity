#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
#endif


namespace Feudal.Scenes.Main
{
    partial class DiscoverPanelViewModel : WorkHoodViewModel
    {
        private int percent;
        public int Percent
        {
            get => percent;
            set => SetProperty(ref percent, value);
        }

        public override RelayCommand<LaborViewModel> OccupyLabor { get; }

        public DiscoverPanelViewModel()
        {
            OccupyLabor = new RelayCommand<LaborViewModel>((laborViewModel) =>
            {
                ExecUICmd?.Invoke(new DiscoverCommand(laborViewModel.clanId, Position));
            });
        }
    }
}
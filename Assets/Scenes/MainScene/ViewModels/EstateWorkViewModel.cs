#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
#endif


namespace Feudal.Scenes.Main
{
    partial class EstateWorkViewModel : WorkHoodViewModel
    {
        private EstateViewModel estate;
        public EstateViewModel Estate
        {
            get => estate;
            set => SetProperty(ref estate, value);
        }

        public override RelayCommand<LaborViewModel> OccupyLabor { get; }

        public EstateWorkViewModel()
        {
            OccupyLabor = new RelayCommand<LaborViewModel>((laborViewModel) =>
            {
                ExecUICmd?.Invoke(new EstateStartWorkCommand(laborViewModel.clanId, Estate.EstateId, Position));
            });
        }
    }
}
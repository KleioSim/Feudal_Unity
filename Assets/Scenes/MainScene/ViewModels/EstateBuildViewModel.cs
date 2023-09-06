#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
#endif

using System;

namespace Feudal.Scenes.Main
{
    partial class EstateBuildViewModel : WorkViewModel
    {
        private int percent;
        public int Percent
        {
            get => percent;
            set => SetProperty(ref percent, value);
        }

        private Enum estateType;
        public Enum EstateType
        {
            get => estateType;
            set => SetProperty(ref estateType, value);
        }

        public override RelayCommand<LaborViewModel> Start { get; }

        public EstateBuildViewModel()
        {
            Start = new RelayCommand<LaborViewModel>((laborViewModel) =>
            {
                ExecUICmd?.Invoke(new EstateBuildStartCommand(laborViewModel.clanId, EstateType, Position));
            });
        }
    }
}
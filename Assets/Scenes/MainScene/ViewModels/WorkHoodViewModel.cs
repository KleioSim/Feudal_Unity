#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
#endif


namespace Feudal.Scenes.Main
{
    class WorkHoodViewModel : ViewModel
    {
        private (int x, int y) position;
        public (int x, int y) Position
        {
            get => position;
            set => SetProperty(ref position, value);
        }

        public RelayCommand<LaborViewModel> FreeLabor { get; }
        public virtual RelayCommand<LaborViewModel> OccupyLabor { get; }

        public WorkHoodViewModel()
        {
            FreeLabor = new RelayCommand<LaborViewModel>((labor) =>
            {
                ExecUICmd.Invoke(new CancelTaskCommand(labor.clanId));
            });
        }
    }
}
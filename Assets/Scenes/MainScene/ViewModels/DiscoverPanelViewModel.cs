#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
#endif


namespace Feudal.Scenes.Main
{
    partial class DiscoverPanelViewModel : WorkViewModel
    {
        private int percent;
        public int Percent
        {
            get => percent;
            set => SetProperty(ref percent, value);
        }

        public override RelayCommand<LaborViewModel> Start { get; }

        public DiscoverPanelViewModel()
        {
            Start = new RelayCommand<LaborViewModel>((laborViewModel)=>
            {
                ExecUICmd?.Invoke(new DiscoverCommand(laborViewModel.clanId, Position));
            });
        }
    }

    abstract class WorkViewModel : ViewModel
    {
        private (int x, int y) position;
        public (int x, int y) Position
        {
            get => position;
            set => SetProperty(ref position, value);
        }

        private WorkerLaborViewModel workerLabor;
        public WorkerLaborViewModel WorkerLabor
        {
            get => workerLabor;
            set => SetProperty(ref workerLabor, value);
        }

        public RelayCommand showLaborSeletor;
        public RelayCommand ShowLaborSeletor
        {
            get => showLaborSeletor;
            set => SetProperty(ref showLaborSeletor, value);
        }

        public RelayCommand Cancel { get; }
        public abstract RelayCommand<LaborViewModel> Start { get; }

        public WorkViewModel()
        {
            Cancel = new RelayCommand(() =>
            {
                ExecUICmd.Invoke(new CancelTaskCommand(WorkerLabor.TaskId));
            });
        }
    }
}
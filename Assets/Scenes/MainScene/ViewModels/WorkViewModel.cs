#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
#endif


namespace Feudal.Scenes.Main
{
    partial class WorkViewModel : ViewModel
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
        public virtual RelayCommand<LaborViewModel> Start { get; }


        private ViewModel workHood;
        public ViewModel WorkHood
        {
            get => workHood;
            set => SetProperty(ref workHood, value);
        }

        public WorkViewModel()
        {
            Cancel = new RelayCommand(() =>
            {
                ExecUICmd.Invoke(new CancelTaskCommand(WorkerLabor.TaskId));
            });
        }
    }
}
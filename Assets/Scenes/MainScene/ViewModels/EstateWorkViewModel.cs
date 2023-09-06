#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
#endif


namespace Feudal.Scenes.Main
{
    partial class EstateWorkViewModel : WorkViewModel
    {
        private string outputType;
        public string OutputType
        {
            get => outputType;
            set => SetProperty(ref outputType, value);
        }

        private decimal outputValue;
        public decimal OutputValue
        {
            get => outputValue;
            set => SetProperty(ref outputValue, value);
        }

        private bool isOutputEnable;
        public bool IsOutputEnable
        {
            get => isOutputEnable;
            set => SetProperty(ref isOutputEnable, value);
        }

        private string estateId;
        public string EstateId
        {
            get => estateId;
            set => SetProperty(ref estateId, value);
        }

        public override RelayCommand<LaborViewModel> Start { get; }

        public EstateWorkViewModel()
        {
            PropertyChanged += EstateWorkViewModel_PropertyChanged;

            Start = new RelayCommand<LaborViewModel>((laborViewModel) =>
            {
                ExecUICmd?.Invoke(new EstateStartWorkCommand(laborViewModel.clanId, EstateId, Position));
            });
        }

        private void EstateWorkViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(WorkerLabor))
            {
                IsOutputEnable = WorkerLabor != null;
            }
        }
    }
}
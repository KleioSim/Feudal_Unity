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

    partial class ClanViewModel : ViewModel
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

        public RelayCommand ShowPlayerClanPanel { get; }
    }

    class EstateViewModel : WorkViewModel
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

        private string estateName;
        public string EstateName
        {
            get => estateName;
            set => SetProperty(ref estateName, value);
        }

        private string workerLaborName;
        public string WorkerLaborName
        {
            get => workerLaborName;
            set => SetProperty(ref workerLaborName, value);
        }

        private string estateId;
        public string EstateId
        {
            get => estateId;
            set => SetProperty(ref estateId, value);
        }

        public override RelayCommand<LaborViewModel> Start { get; }

        public EstateViewModel()
        {
            WorkLaborUpateTrigger();

            PropertyChanged += EstateWorkViewModel_PropertyChanged;

            Start = new RelayCommand<LaborViewModel>((laborViewModel) =>
            {
                ExecUICmd?.Invoke(new EstateStartWorkCommand(laborViewModel.clanId, EstateId, Position));
            });
        }

        private void EstateWorkViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(WorkerLabor))
            {
                WorkLaborUpateTrigger();
            }
        }

        private void WorkLaborUpateTrigger()
        {
            if (WorkerLabor == null)
            {
                IsOutputEnable = false;
                WorkerLaborName = "--";
            }
            else
            {
                WorkerLaborName = WorkerLabor.Name;
            }
        }
    }
}
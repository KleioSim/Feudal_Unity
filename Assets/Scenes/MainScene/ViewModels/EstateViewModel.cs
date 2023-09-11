#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
#endif

using System;
using System.Globalization;

namespace Feudal.Scenes.Main
{
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

    public class EstateToMapItem : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mapItemViewModel = new MapDetailViewModel();
            mapItemViewModel.Position = ((EstateViewModel)value).Position;

            return mapItemViewModel;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
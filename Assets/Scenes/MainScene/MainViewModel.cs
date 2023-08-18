#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
#endif

using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Feudal.Scenes.Main
{
    internal class MapItemDetail
    {

    }

    internal class MainViewModel : ViewModel
    {
        public RelayCommand CreateMapItemDetail { get; }
        public RelayCommand RemoveMapItemDetail { get; }

        private MapItemDetail mapItemDetail;
        public MapItemDetail MapItemDetail
        {
            get => mapItemDetail;
            private set => SetProperty(ref mapItemDetail, value);
        }

        public MainViewModel()
        {
            CreateMapItemDetail = new RelayCommand(() =>
            {
                MapItemDetail = new MapItemDetail();
            });

            RemoveMapItemDetail = new RelayCommand(() =>
            {
                MapItemDetail = null;
            });
        }
    }

    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
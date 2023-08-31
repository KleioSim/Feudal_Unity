#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
#endif


namespace Feudal.Scenes.Main
{
    internal class MapDetailViewModel : ViewModel
    {
        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private (int x, int y) position;
        public (int x, int y) Position
        {
            get => position;
            set => SetProperty(ref position, value);
        }

        private MapWorkerPanelViewModel mapWorkerPanel;
        public MapWorkerPanelViewModel MapWorkerPanel
        {
            get => mapWorkerPanel;
            set => SetProperty(ref mapWorkerPanel, value);
        }

    }

    class MapWorkerPanelViewModel : ViewModel
    {
        private string workClanName;
        public string WorkClanName
        {
            get => workClanName;
            set => SetProperty(ref workClanName, value);
        }

    }
}
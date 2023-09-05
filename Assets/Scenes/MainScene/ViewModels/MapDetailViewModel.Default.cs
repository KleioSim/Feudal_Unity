#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
#endif


namespace Feudal.Scenes.Main
{
    internal partial class MapDetailViewModel
    {
        private static MapDetailViewModel @default;
        public static MapDetailViewModel Default
        {
            get
            {
                if(@default == null)
                {
                    @default = new MapDetailViewModel();
                    @default.Position = (0, 0);
                    @default.Title = "MapItemTitle";

                    @default.DiscoverPanel = new DiscoverPanelViewModel();
                    @default.DiscoverPanel.Position = (0, 0);
                    @default.DiscoverPanel.Percent = 33;

                    @default.SubViewModel = LaborSelectorViewModel.Default;
                    LaborSelectorViewModel.Default.Confirm = new RelayCommand(() =>
                    {
                        @default.SubViewModel = null;
                    },
                    () =>
                    {
                        return LaborSelectorViewModel.Default.SelectedLabor != null;
                    });

                    //@default.DiscoverPanel.WorkerLabor = new WorkerLaborViewModel();
                    //@default.DiscoverPanel.WorkerLabor.Name = "TEST_LABOR_NAME";
                }
                return @default;
            }
        }
    }
}
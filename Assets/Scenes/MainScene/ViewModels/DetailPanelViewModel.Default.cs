#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
#endif


namespace Feudal.Scenes.Main
{
    public partial class DetailPanelViewModel
    {
        private static DetailPanelViewModel @default;
        public static DetailPanelViewModel Default
        {
            get
            {
                if(@default == null)
                {
                    @default = new DetailPanelViewModel();
                    @default.AddPanel.Execute(MapDetailViewModel.Default);

                    @default.SubViewModel = LaborSelectorViewModel.Default;

                }

                return @default;
            }
        }
    }
}
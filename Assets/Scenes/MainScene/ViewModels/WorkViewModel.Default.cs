#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
#endif


namespace Feudal.Scenes.Main
{
    partial class WorkViewModel
    {
        private static WorkViewModel @default;
        public static WorkViewModel Default
        {
            get
            {
                if(@default == null)
                {
                    @default = new WorkViewModel();
                    @default.workHood = DiscoverPanelViewModel.Default;
                }
                return @default;
            }
        }
    }
}
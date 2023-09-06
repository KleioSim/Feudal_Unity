#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
#endif

namespace Feudal.Scenes.Main
{
    partial class DiscoverPanelViewModel
    {
        private static DiscoverPanelViewModel @default;
        public static DiscoverPanelViewModel Default
        {
            get
            {
                if(@default == null)
                {
                    @default = new DiscoverPanelViewModel();
                    @default.Position = (0, 0);
                    @default.percent = 33;
                }

                return @default;
            }
        }
    }
}
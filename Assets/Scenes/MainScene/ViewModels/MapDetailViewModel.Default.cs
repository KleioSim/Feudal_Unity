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
                }
                return @default;
            }
        }
    }
}
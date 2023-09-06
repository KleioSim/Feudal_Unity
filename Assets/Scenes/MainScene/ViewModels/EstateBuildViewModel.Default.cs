#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
#endif


namespace Feudal.Scenes.Main
{
    partial class EstateBuildViewModel
    {
        enum Estate
        {
            Test,
        }

        private static EstateBuildViewModel @default;
        public static EstateBuildViewModel Default
        {
            get
            {
                if(@default == null)
                {
                    @default = new EstateBuildViewModel();
                    @default.Position = (0, 0);
                    @default.EstateType = Estate.Test;
                }

                return @default;
            }
        }
    }
}
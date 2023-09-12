#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
#else
#endif


namespace Feudal.Scenes.Main
{
    partial class EstateViewModel
    {
        private static EstateViewModel @default;
        public static EstateViewModel Default
        {
            get
            {
                if(@default == null)
                {
                    @default = new EstateViewModel();
                    @default.EstateName = "EstateName";
                    @default.OutputType = "OutputType";
                    @default.OutputValue = +10;
                }

                return @default;
            }
        }
    }
}
#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
#endif


namespace Feudal.Scenes.Main
{
    partial class EstateWorkViewModel
    {
        private static EstateWorkViewModel @default;
        public static EstateWorkViewModel Default
        {
            get
            {
                if(@default == null)
                {
                    @default = new EstateWorkViewModel() { Estate = new EstateViewModel() };
                    @default.estate.EstateName = "EstateName";
                    @default.estate.OutputType = "OutputType";
                    @default.estate.OutputValue = +10;
                }
                return @default;
            }
        }
    }
}
#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
#endif


namespace Feudal.Scenes.Main
{

    partial class ClanViewModel
    {
        private static ClanViewModel @default;
        public static ClanViewModel Default
        {
            get
            {
                if(@default == null)
                {
                    @default = new ClanViewModel();

                    @default.Name = "ClanTitle";
                    @default.PopCount = 1000;
                    @default.Food = 10;
                    @default.FoodSurplus = 2;

                    @default.Estates.Add(new EstateViewModel()
                    {
                        EstateName = "Estate0",
                        OutputType = "Product0",
                        OutputValue = 0
                    });
                    @default.Estates.Add(new EstateViewModel()
                    {
                        EstateName = "Estate1",
                        OutputType = "Product1",
                        OutputValue = 1
                    });
                    @default.Estates.Add(new EstateViewModel()
                    {
                        EstateName = "Estate2",
                        OutputType = "Product2",
                        OutputValue = 2
                    });
                }
                return @default;
            }
        }
    }
}
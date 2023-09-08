#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
#endif

namespace Feudal.Scenes.Main
{
    partial class ClanPanelViewModel
    {
        private static ClanPanelViewModel @default;
        public static ClanPanelViewModel Default
        {
            get
            {
                if(@default == null)
                {
                    @default = new ClanPanelViewModel();
                    @default.ClanViewModel = new ClanViewModel();
                    @default.ClanViewModel.Name = "ClanTitle";
                    @default.ClanViewModel.Estates.Add(new EstateViewModel()
                    {
                        EstateName = "Estate0",
                        OutputType = "Product0",
                        OutputValue = 0
                    });

                    @default.ClanViewModel.Estates.Add(new EstateViewModel()
                    {
                        EstateName = "Estate1",
                        OutputType = "Product1",
                        OutputValue = 1
                    });

                    @default.ClanViewModel.Estates.Add(new EstateViewModel()
                    {
                        EstateName = "Estate2",
                        OutputType = "Product2",
                        OutputValue = 2
                    }); ;
                }
                return @default;
            }
        }
    }
}

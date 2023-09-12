namespace Feudal.Scenes.Main
{
    partial class ClanDetailPanelViewModel
    {
        private static ClanDetailPanelViewModel @default;
        public static ClanDetailPanelViewModel Default
        {
            get
            {
                if(@default == null)
                {
                    @default = new ClanDetailPanelViewModel();
                    @default.ClanViewModel = ClanViewModel.Default;
                }
                return @default;
            }
        }
    }
}

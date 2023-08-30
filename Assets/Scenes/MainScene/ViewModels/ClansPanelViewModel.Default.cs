#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
#endif


namespace Feudal.Scenes.Main
{
    partial class ClansPanelViewModel
    {
        private static ClansPanelViewModel @default;
        public static ClansPanelViewModel Default
        {
            get
            {
                if(@default == null)
                {
                    @default = new ClansPanelViewModel();
                    @default.Title = "Clans";

                    @default.ClanItems.Add(new ClanViewModel("ClanId1")
                    {
                        Name = "Clan1"
                    });
                }

                return @default;
            }
        }
    }
}
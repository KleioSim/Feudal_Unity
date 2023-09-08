#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
#endif


namespace Feudal.Scenes.Main
{

    internal partial class MainViewModel
    {
        private static MainViewModel @default;
        public static MainViewModel Default
        {
            get
            {
                if(@default == null)
                {
                    @default = new MainViewModel();

                    @default.PlayerClan.Name = "PlayerClan";
                    @default.PlayerClan.PopCount = 1000;
                    @default.PlayerClan.Food = 10;
                    @default.PlayerClan.FoodSurplus = 2;

                    @default.PlayerClan.Estates.Add(new EstateViewModel());
                    @default.PlayerClan.Estates.Add(new EstateViewModel());

                    @default.DetailPanel.Add(ClansPanelViewModel.Default);
                    @default.DetailPanel.Add(MapDetailViewModel.Default);
                    @default.DetailPanel.Add(ClanPanelViewModel.Default);

                    @default.Tasks.Add(new TaskViewModel("ID0") { Desc = "Task Desc0", Percent = 33 });
                    @default.Tasks.Add(new TaskViewModel("ID1") { Desc = "Task Desc1", Percent = 22 });
                }

                return @default;
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feudal.Scenes.Main
{
    partial class ClanDetailPanelViewModel : DetailPanelViewModel
    {
        private ClanViewModel clanViewModel;
        public ClanViewModel ClanViewModel
        {
            get => clanViewModel;
            set => SetProperty(ref clanViewModel, value);
        }
    }
}

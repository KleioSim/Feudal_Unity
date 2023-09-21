using System;
using System.Linq;

namespace Feudal.Presents
{
    class Present_TopEstatesPanel : Present<TopEstatesPanel>
    {
        public override void Refresh(TopEstatesPanel view)
        {
            view.SetProductItems(session.playerClan.estates
                .Select(x => (Enum)x.Type)
                .Distinct()
                .ToArray());
        }
    }
}
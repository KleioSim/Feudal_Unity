using System;
using System.Linq;

namespace Feudal.Presents
{
    class Present_TopProductsPanel : Present<TopProductsPanel>
    {
        public override void Refresh(TopProductsPanel view)
        {
            view.SetProductItems(session.playerClan.ProductMgr.Keys.Select(x => x as Enum).ToArray());
        }
    }
}
using Feudal.Interfaces;
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

    class Present_TopProductItem : Present<TopProductItem>
    {
        public override void Refresh(TopProductItem view)
        {
            view.title.text = view.productType.ToString();
            view.count.text = session.playerClan.ProductMgr[(ProductType)view.productType].Current.ToString();
        }
    }
}
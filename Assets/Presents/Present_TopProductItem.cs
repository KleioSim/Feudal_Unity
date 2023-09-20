using Feudal.Interfaces;

namespace Feudal.Presents
{
    class Present_TopProductItem : Present<TopProductItem>
    {
        public override void Refresh(TopProductItem view)
        {
            view.title.text = view.productType.ToString();
            view.count.text = session.playerClan.ProductMgr[(ProductType)view.productType].Current.ToString();
        }
    }
}
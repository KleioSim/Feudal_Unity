using Feudal.Interfaces;
using System.Linq;

namespace Feudal.Presents
{
    class Present_TopEstateItem : Present<TopEstateItem>
    {
        public override void Refresh(TopEstateItem view)
        {
            view.title.text = view.EstateType.ToString();
            view.count.text = session.playerClan.estates.Count(x => x.Type == (EstateType)view.EstateType).ToString();
        }
    }
}
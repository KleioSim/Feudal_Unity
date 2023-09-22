using Feudal.Interfaces;
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

    class Present_TopEstateItem : Present<TopEstateItem>
    {
        public override void Refresh(TopEstateItem view)
        {
            view.title.text = view.EstateType.ToString();
            view.count.text = session.playerClan.estates.Count(x => x.Type == (EstateType)view.EstateType).ToString();
        }
    }
}
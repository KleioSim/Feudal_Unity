using Feudal.Interfaces;
using System.Linq;

namespace Feudal.Presents
{
    class Present_TopClanItem : Present<TopClanItem>
    {
        public override void Refresh(TopClanItem view)
        {
            view.title.text = view.ClanType.ToString();
            view.count.text = session.clans.Count(x => x.ClanType == (ClanType)view.ClanType).ToString();
        }
    }
}
using Feudal.Interfaces;
using System;
using System.Linq;

namespace Feudal.Presents
{
    class Present_TopClansPanel : Present<TopClansPanel>
    {
        public override void Refresh(TopClansPanel view)
        {
            view.SetClanItems(session.clans
                .Where(x => x.ClanType.GetAttributeOfType<CountryClanAttribute>() != null)
                .Select(x => (Enum)x.ClanType)
                .Distinct()
                .ToArray());
        }
    }

    class Present_TopClanItem : Present<TopClanItem>
    {
        public override void Refresh(TopClanItem view)
        {
            view.title.text = view.ClanType.ToString();
            view.count.text = session.clans.Count(x => x.ClanType == (ClanType)view.ClanType).ToString();
        }
    }
}
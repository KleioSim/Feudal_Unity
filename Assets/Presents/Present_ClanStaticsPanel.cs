using Feudal.Interfaces;
using System.Linq;

namespace Feudal.Presents
{
    class Present_ClanStaticsPanel : Present<ClanStaticsPanel>
    {
        public override void Refresh(ClanStaticsPanel view)
        {
            view.SetClanItems(session.clans
                .Where(x => x.ClanType.GetAttributeOfType<CountryClanAttribute>() != null)
                .Select(x => x.Id)
                .ToArray());
        }
    }
}
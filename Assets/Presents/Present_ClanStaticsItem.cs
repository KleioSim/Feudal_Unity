using System.Linq;

namespace Feudal.Presents
{
    class Present_ClanStaticsItem : Present<ClanStaticsItem>
    {
        public override void Refresh(ClanStaticsItem view)
        {
            var clan = session.clans.SingleOrDefault(x => x.Id == view.ClanId);

            view.title.text = clan.Name;

            if(clan.ClanType == Interfaces.ClanType.Guo)
            {
                view.clanType.text = $"{clan.ClanType}·{(session.playerClan == clan ? "公族" : "卿族")}";
            }
            else
            {
                view.clanType.text = clan.ClanType.ToString();
            }

            view.popCount.text = clan.PopCount.ToString();

            var estates = session.estates.Values.Where(x => x.OwnerId == view.ClanId);
            view.estateCount.text = estates.Count().ToString();
        }
    }
}